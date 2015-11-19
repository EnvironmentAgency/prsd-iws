﻿namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using RequestHandlers.NotificationAssessment;
    using Requests.NotificationAssessment;
    using TestHelpers.DomainFakes;
    using TestHelpers.Helpers;
    using Xunit;

    public class AddNotificationTransactionHandlerTests
    {
        private static readonly DateTime NewDate = new DateTime(2015, 11, 01);
        private readonly DateTime existingDate = new DateTime(2015, 10, 01);
        private readonly DateTime newerDate = new DateTime(2015, 11, 02);

        private static readonly Guid NotificationId = new Guid("8B557FD5-B2DB-4FE9-B80E-D4DA629B2B51");

        private readonly AddNotificationTransactionHandler handler;
        private readonly TestIwsContext context = new TestIwsContext();
        private readonly NotificationAssessment assessment;

        public AddNotificationTransactionHandlerTests()
        {
            var chargeCalculator = A.Fake<INotificationChargeCalculator>();
            A.CallTo(() => chargeCalculator.GetValue(A<PricingStructure[]>.Ignored, A<NotificationApplication>.Ignored, A<ShipmentInfo>.Ignored)).Returns(200.00m);

            var transactionsList = new List<NotificationTransaction>();
            transactionsList.Add(new NotificationTransaction(new NotificationTransactionData
            {
                Date = NewDate,
                NotificationId = NotificationId,
                Credit = 100.00m,
                PaymentMethod = 1
            }));

            var repository = A.Fake<INotificationTransactionRepository>();
            A.CallTo(() => repository.GetTransactions(NotificationId)).Returns(transactionsList);

            handler = new AddNotificationTransactionHandler(context, repository, chargeCalculator, new NotificationTransactionCalculator());

            context.ShipmentInfos.Add(new TestableShipmentInfo { NotificationId = NotificationId });

            assessment = new NotificationAssessment(NotificationId);
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.Status, NotificationStatus.NotificationReceived, assessment);
            context.NotificationAssessments.Add(assessment);

            context.NotificationApplications.Add(new TestableNotificationApplication { Id = NotificationId });
        }

        [Fact]
        public async Task PaymentComplete_PaymentReceivedDateNotSet_AddsDateToAssessment()
        {
            var newDate = NewDate;
            var data = GetNotificationTransactionData(newDate);
            data.Credit = 100.00m;

            var message = new AddNotificationTransaction(data);

            await handler.HandleAsync(message);

            Assert.Equal(newDate, context.NotificationAssessments.Single().Dates.PaymentReceivedDate);
        }

        [Fact]
        public async Task PaymentComplete_PaymentReceivedDateSet_AddsDateToAssessment()
        {
            assessment.PaymentReceived(existingDate);

            var data = GetNotificationTransactionData(NewDate);
            data.Credit = 100.00m;
            
            var message = new AddNotificationTransaction(data);

            await handler.HandleAsync(message);

            Assert.Equal(existingDate, context.NotificationAssessments.Single().Dates.PaymentReceivedDate);
        }

        [Fact]
        public async Task PaymentMadeButPaymentIncomplete_DoesNotAddDateToAssessment()
        {
            assessment.PaymentReceived(existingDate);
            
            var data = GetNotificationTransactionData(NewDate);
            data.Credit = 50.00m;

            var message = new AddNotificationTransaction(data);

            await handler.HandleAsync(message);

            Assert.Equal(existingDate, context.NotificationAssessments.Single().Dates.PaymentReceivedDate);
        }

        [Fact]
        public async Task RefundMade_PaymentComplete_DoesNotAddDateToAssessment()
        {
            assessment.PaymentReceived(existingDate);

            var creditData = GetNotificationTransactionData(existingDate);
            creditData.Debit = 500.00m;

            var debitData = GetNotificationTransactionData(NewDate);
            debitData.Debit = 50.00m;

            var message = new AddNotificationTransaction(creditData);
            await handler.HandleAsync(message);

            message = new AddNotificationTransaction(debitData);
            await handler.HandleAsync(message);

            Assert.Equal(existingDate, context.NotificationAssessments.Single().Dates.PaymentReceivedDate);
        }

        [Fact]
        public async Task PaymentComplete_RefundMade_PaymentCompletedAgain_DoesNotAddDateToAssessment()
        {
            assessment.PaymentReceived(existingDate);
            
            var paymentComplete = GetNotificationTransactionData(existingDate);
            paymentComplete.Credit = 200.00m;

            var refundMade = GetNotificationTransactionData(NewDate);
            refundMade.Debit = 50.00m;

            var paymentCompleteAgain = GetNotificationTransactionData(newerDate);
            paymentCompleteAgain.Credit = 200.00m;

            var message = new AddNotificationTransaction(paymentComplete);
            await handler.HandleAsync(message);

            message = new AddNotificationTransaction(refundMade);
            await handler.HandleAsync(message);

            message = new AddNotificationTransaction(paymentCompleteAgain);
            await handler.HandleAsync(message);

            Assert.Equal(existingDate, context.NotificationAssessments.Single().Dates.PaymentReceivedDate);
        }

        private NotificationTransactionData GetNotificationTransactionData(DateTime date)
        {
            return new NotificationTransactionData
            {
                Date = date,
                NotificationId = NotificationId,
                PaymentMethod = 1
            };
        }
    }
}

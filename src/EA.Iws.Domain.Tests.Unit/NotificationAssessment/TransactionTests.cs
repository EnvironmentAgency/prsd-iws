﻿namespace EA.Iws.Domain.Tests.Unit.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Core.Shared;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class TransactionTests
    {
        private readonly Transaction transaction;
        private readonly TestableNotificationAssessment assessment;
        private readonly Guid notificationId = new Guid("34C335B5-1329-47E1-8605-DE5532ADF9D7");

        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly INotificationTransactionRepository notificationTransactionRepository;
        private readonly INotificationTransactionCalculator notificationTransactionCalculator;

        public TransactionTests()
        {
            notificationAssessmentRepository = A.Fake<INotificationAssessmentRepository>();
            notificationTransactionRepository = A.Fake<INotificationTransactionRepository>();
            notificationTransactionCalculator = A.Fake<INotificationTransactionCalculator>();

            assessment = new TestableNotificationAssessment();
            assessment.NotificationApplicationId = notificationId;
            assessment.Dates = new NotificationDates();

            A.CallTo(() => notificationAssessmentRepository.GetByNotificationId(notificationId))
                .Returns(assessment);

            A.CallTo(() => notificationTransactionCalculator.Balance(notificationId))
                .Returns(1000);

            transaction = new Transaction(
                notificationAssessmentRepository, 
                notificationTransactionRepository,
                notificationTransactionCalculator);
        }

        private NotificationTransaction CreateNotificationTransaction(int credit)
        {
            return new NotificationTransaction(
                new NotificationTransactionData
                {
                    Credit = credit,
                    Date = new DateTime(2017, 1, 1),
                    NotificationId = notificationId,
                    PaymentMethod = PaymentMethod.Card
                });
        }

        private NotificationTransaction CreateNotificationTransaction(int credit, DateTime date)
        {
            return new NotificationTransaction(
                new NotificationTransactionData
                {
                    Credit = credit,
                    Date = date,
                    NotificationId = notificationId,
                    PaymentMethod = PaymentMethod.Card
                });
        }

        [Fact]
        public async Task Save_AddsNotificationTransaction()
        {
            var notificationTransaction = CreateNotificationTransaction(100);

            await transaction.Save(notificationTransaction);

            A.CallTo(() => notificationTransactionRepository.Add(notificationTransaction))
                .MustHaveHappened();
        }

        [Fact]
        public async Task Save_PaymentFullyReceived_SetsReceivedDate()
        {
            var notificationTransaction = CreateNotificationTransaction(1000);
            var transactionsList = new List<NotificationTransaction> { notificationTransaction };

            A.CallTo(() => notificationTransactionRepository.GetTransactions(notificationId))
                .Returns(transactionsList);

            await transaction.Save(notificationTransaction);

            Assert.Equal(assessment.Dates.PaymentReceivedDate, new DateTime(2017, 1, 1));
        }

        [Fact]
        public async Task Save_PaymentFullyReceived_CorrectPaymentReceivedDate()
        {
            var notificationTransaction = CreateNotificationTransaction(300, new DateTime(2018, 4, 4));
            var transactionsList = new List<NotificationTransaction>
            {
                CreateNotificationTransaction(300, new DateTime(2018, 1, 1)),
                CreateNotificationTransaction(300, new DateTime(2018, 2, 2)),
                // This will be the payment that reaches 1000 when ordered by date.
                CreateNotificationTransaction(400, new DateTime(2018, 3, 3)),
                notificationTransaction
            };

            // Set payment to fully received
            A.CallTo(() => notificationTransactionCalculator.Balance(notificationId))
                .Returns(0);

            A.CallTo(() => notificationTransactionRepository.GetTransactions(notificationId))
                .Returns(transactionsList);

            await transaction.Save(notificationTransaction);

            Assert.Equal(assessment.Dates.PaymentReceivedDate, new DateTime(2018, 3, 3));
        }

        [Fact]
        public async Task Save_PaymentNotFullyReceived_ReceivedDateNull()
        {
            var notificationTransaction = CreateNotificationTransaction(999);
            var transactionsList = new List<NotificationTransaction> { notificationTransaction };

            A.CallTo(() => notificationTransactionRepository.GetTransactions(notificationId))
                .Returns(transactionsList);

            await transaction.Save(notificationTransaction);

            Assert.Null(assessment.Dates.PaymentReceivedDate);
        }

        [Fact]
        public async Task Delete_PaymentNotFullyReceived_ReceivedDateNull()
        {
            var transactionId = new Guid("F7DF1DD7-E356-47E2-8C9C-281C4A824F94");
            var notificationTransaction = CreateNotificationTransaction(600);
            var transactionsList = new List<NotificationTransaction> { notificationTransaction };

            A.CallTo(() => notificationTransactionRepository.GetById(transactionId))
                .Returns(notificationTransaction);

            // Set payment to fully received
            A.CallTo(() => notificationTransactionCalculator.Balance(notificationId))
                .Returns(0);

            A.CallTo(() => notificationTransactionRepository.GetTransactions(notificationId))
                .Returns(transactionsList);

            assessment.Dates.PaymentReceivedDate = new DateTime(2017, 2, 2);

            // Delete payment, balance now £600
            await transaction.Delete(notificationId, transactionId);

            Assert.Null(assessment.Dates.PaymentReceivedDate);
        }

        [Fact]
        public async Task Delete_PaymentStillFullyReceived_ReceivedDateUpdated()
        {
            var transactionId = new Guid("F7DF1DD7-E356-47E2-8C9C-281C4A824F94");
            var notificationTransaction = CreateNotificationTransaction(100);
            var transactionsList = new List<NotificationTransaction>
            {
                CreateNotificationTransaction(300, new DateTime(2018, 1, 1)),
                // This will be the payment that reaches 1000 when ordered by date.
                CreateNotificationTransaction(700, new DateTime(2018, 2, 2)),
                CreateNotificationTransaction(100, new DateTime(2018, 3, 3))
            };

            A.CallTo(() => notificationTransactionRepository.GetById(transactionId))
                .Returns(notificationTransaction);

            // Set payment to overpaid
            A.CallTo(() => notificationTransactionCalculator.Balance(notificationId))
                .Returns(-200);

            A.CallTo(() => notificationTransactionRepository.GetTransactions(notificationId))
               .Returns(transactionsList);

            assessment.Dates.PaymentReceivedDate = new DateTime(2017, 2, 2);

            // Delete payment, balance now -£100
            await transaction.Delete(notificationId, transactionId);

            Assert.Equal(assessment.Dates.PaymentReceivedDate, new DateTime(2018, 2, 2));
        }
    }
}

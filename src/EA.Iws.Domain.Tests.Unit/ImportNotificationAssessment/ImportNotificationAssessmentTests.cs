﻿namespace EA.Iws.Domain.Tests.Unit.ImportNotificationAssessment
{
    using System;
    using Core.ImportNotificationAssessment;
    using Domain.ImportNotificationAssessment;
    using TestHelpers.Helpers;
    using Xunit;

    public class ImportNotificationAssessmentTests
    {
        private readonly ImportNotificationAssessment assessment;

        private static readonly DateTimeOffset AnyDate = new DateTimeOffset(2016, 1, 1, 0, 0, 0, TimeSpan.Zero);

        public ImportNotificationAssessmentTests()
        {
            assessment = new ImportNotificationAssessment(Guid.Empty);
        }

        [Fact]
        public void ReceiveNotification_SetsStatus()
        {
            assessment.Receive(AnyDate);

            Assert.Equal(ImportNotificationStatus.NotificationReceived, assessment.Status);
        }

        [Fact]
        public void ReceiveNotification_SetsDate()
        {
            assessment.Receive(AnyDate);

            Assert.Equal(AnyDate, assessment.Dates.NotificationReceivedDate);
        }

        [Fact]
        public void CannotReceiveReceivedNotification()
        {
            SetNotificationAssessmentStatus(ImportNotificationStatus.NotificationReceived);

            Assert.Throws<InvalidOperationException>(() => assessment.Receive(AnyDate));
        }

        [Fact]
        public void CannotReceiveNotificationAwaitingAssessment()
        {
            SetNotificationAssessmentStatus(ImportNotificationStatus.AwaitingAssessment);

            Assert.Throws<InvalidOperationException>(() => assessment.Receive(AnyDate));
        }

        [Fact]
        public void SubmitNotification_SetsStatus()
        {
            SetNotificationAssessmentStatus(ImportNotificationStatus.NotificationReceived);

            assessment.Submit();

            Assert.Equal(ImportNotificationStatus.AwaitingPayment, assessment.Status);
        }

        [Fact]
        public void CannotResubmit()
        {
            SetNotificationAssessmentStatus(ImportNotificationStatus.AwaitingPayment);

            Assert.Throws<InvalidOperationException>(() => assessment.Submit());
        }

        [Fact]
        public void PaymentFullyReceived_SetsStatus()
        {
            SetNotificationAssessmentStatus(ImportNotificationStatus.AwaitingPayment);

            assessment.PaymentComplete(AnyDate);

            Assert.Equal(ImportNotificationStatus.AwaitingAssessment, assessment.Status);
        }

        [Fact]
        public void PaymentFullyReceived_SetsDate()
        {
            SetNotificationAssessmentStatus(ImportNotificationStatus.AwaitingPayment);

            assessment.PaymentComplete(AnyDate);

            Assert.Equal(AnyDate, assessment.Dates.PaymentReceivedDate);
        }

        private void SetNotificationAssessmentStatus(ImportNotificationStatus status)
        {
            ObjectInstantiator<ImportNotificationAssessment>.SetProperty(x => x.Status, status, assessment);
        }
    }
}

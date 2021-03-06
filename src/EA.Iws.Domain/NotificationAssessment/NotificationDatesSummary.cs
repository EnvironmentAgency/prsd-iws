﻿namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using Core.NotificationAssessment;

    public class NotificationDatesSummary
    {
        public NotificationStatus CurrentStatus { get; private set; }

        public DateTime? NotificationReceivedDate { get; private set; }

        public Guid NotificationId { get; private set; }

        public DateTime? PaymentReceivedDate { get; private set; }

        public bool PaymentIsComplete { get; private set; }

        public DateTime? CommencementDate { get; private set; }

        public string NameOfOfficer { get; private set; }

        public DateTime? CompletedDate { get; private set; }

        public DateTime? TransmittedDate { get; private set; }

        public DateTime? AcknowledgedDate { get; private set; }

        public DateTime? DecisionRequiredDate { get; private set; }

        public DateTime? FileClosedDate { get; private set; }

        public string ArchiveReference { get; private set; }

        public static NotificationDatesSummary Load(
            NotificationStatus currentStatus,
            DateTime? notificationReceivedDate,
            Guid notificationId,
            DateTime? paymentReceivedDate,
            bool paymentIsComplete,
            DateTime? commencementDate,
            string nameOfOfficer,
            DateTime? completedDate,
            DateTime? transmittedDate,
            DateTime? acknowledgedDate,
            DateTime? decisionRequiredDate,
            DateTime? fileClosedDate,
            string archiveReference)
        {
            return new NotificationDatesSummary
            {
                CurrentStatus = currentStatus,
                NotificationReceivedDate = notificationReceivedDate,
                NotificationId = notificationId,
                PaymentReceivedDate = paymentReceivedDate,
                PaymentIsComplete = paymentIsComplete,
                CommencementDate = commencementDate,
                NameOfOfficer = nameOfOfficer,
                CompletedDate = completedDate,
                TransmittedDate = transmittedDate,
                AcknowledgedDate = acknowledgedDate,
                DecisionRequiredDate = decisionRequiredDate,
                FileClosedDate = fileClosedDate,
                ArchiveReference = archiveReference
            };
        }
    }
}
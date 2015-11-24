﻿namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;

    public class NotificationAssessmentDatesSummaryRepository : INotificationAssessmentDatesSummaryRepository
    {
        private readonly DecisionRequiredBy decisionRequiredBy;
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly INotificationTransactionCalculator transactionCalculator;

        public NotificationAssessmentDatesSummaryRepository(DecisionRequiredBy decisionRequiredBy,
            INotificationAssessmentRepository notificationAssessmentRepository,
            INotificationApplicationRepository notificationApplicationRepository,
            INotificationTransactionCalculator transactionCalculator)
        {
            this.decisionRequiredBy = decisionRequiredBy;
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.notificationAssessmentRepository = notificationAssessmentRepository;
            this.transactionCalculator = transactionCalculator;
        }

        public async Task<NotificationDatesSummary> GetById(Guid notificationId)
        {
            var assessment = await notificationAssessmentRepository.GetByNotificationId(notificationId);
            var notification = await notificationApplicationRepository.GetById(notificationId);

            var latestPayment = await transactionCalculator.LastestPayment(notificationId);
            DateTime? lastestPaymentDate = null;

            if (latestPayment != null)
            {
                lastestPaymentDate = latestPayment.Date;
            }

            return NotificationDatesSummary.Load(
                assessment.Dates.NotificationReceivedDate.GetValueOrDefault(),
                notificationId,
                lastestPaymentDate,
                await transactionCalculator.IsPaymentComplete(notificationId),
                assessment.Dates.NotificationReceivedDate.GetValueOrDefault(),
                assessment.Dates.NameOfOfficer,
                assessment.Dates.CommencementDate.GetValueOrDefault(),
                assessment.Dates.TransmittedDate.GetValueOrDefault(),
                assessment.Dates.AcknowledgedDate.GetValueOrDefault(),
                decisionRequiredBy.GetDecisionRequiredByDate(notification, assessment));
        }
    }
}

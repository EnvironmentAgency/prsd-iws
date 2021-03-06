﻿namespace EA.Iws.RequestHandlers.Mappings.Reports
{
    using System;
    using Core.Admin.Reports;
    using Core.Notification;
    using Core.Shared;
    using Domain;
    using Domain.NotificationAssessment;
    using Domain.Reports;
    using Prsd.Core.Mapper;

    internal class DataExportNotificationMap : IMapWithParameter<DataExportNotification, UKCompetentAuthority, DataExportNotificationData>
    {
        private readonly IWorkingDayCalculator workingDayCalculator;
        private readonly IDecisionRequiredByCalculator decisionRequiredByCalculator;

        public DataExportNotificationMap(IWorkingDayCalculator workingDayCalculator, IDecisionRequiredByCalculator decisionRequiredByCalculator)
        {
            this.workingDayCalculator = workingDayCalculator;
            this.decisionRequiredByCalculator = decisionRequiredByCalculator;
        }

        public DataExportNotificationData Map(DataExportNotification source, UKCompetentAuthority parameter)
        {
            return new DataExportNotificationData
            {
                NotificationType = source.NotificationType,
                Status = source.Status,
                Preconsented = GetPreconsented(source, source.NotificationType),
                NotificationNumber = source.NotificationNumber,
                Acknowledged = source.Acknowledged,
                ApplicationCompleted = source.ApplicationCompleted,
                AssessmentStarted = source.AssessmentStarted,
                DecisionDate = source.DecisionDate,
                NotificationReceived = source.NotificationReceived,
                PaymentReceived = source.PaymentReceived,
                Transmitted = source.Transmitted,
                AssessmentStartedElapsedWorkingDays = GetAssessmentStartedElapsedWorkingDays(source, parameter),
                TransmittedElapsedWorkingDays = GetTransmittedElapsedWorkingDays(source, parameter),
                DecisionRequiredDate = GetDecisionRequiredByDate(source, parameter),
                ConsentDate = source.Consented,
                Officer = source.Officer,
                SubmittedBy = source.SubmittedBy,
                ConsentTo = source.ConsentTo
            };
        }

        private static string GetPreconsented(DataExportNotification source, NotificationType notificationType)
        {
            if (notificationType == NotificationType.Disposal)
            {
                return "No";
            }

            if (source.Preconsented.HasValue)
            {
                return source.Preconsented.Value ? "Yes" : "No";
            }

            return null;
        }

        private int? GetAssessmentStartedElapsedWorkingDays(DataExportNotification source, UKCompetentAuthority parameter)
        {
            if (!source.PaymentReceived.HasValue || !source.AssessmentStarted.HasValue || !source.NotificationReceived.HasValue)
            {
                return null;
            }

            return workingDayCalculator.GetWorkingDays(
                source.PaymentReceived.Value > source.NotificationReceived.Value ? source.PaymentReceived.Value : source.NotificationReceived.Value, 
                source.AssessmentStarted.Value,
                false, parameter);
        }

        private int? GetTransmittedElapsedWorkingDays(DataExportNotification source, UKCompetentAuthority parameter)
        {
            if (!source.ApplicationCompleted.HasValue || !source.Transmitted.HasValue)
            {
                return null;
            }

            return workingDayCalculator.GetWorkingDays(source.ApplicationCompleted.Value, source.Transmitted.Value,
                false, parameter);
        }

        private DateTime? GetDecisionRequiredByDate(DataExportNotification source, UKCompetentAuthority parameter)
        {
            if (!source.Acknowledged.HasValue || !source.Preconsented.HasValue)
            {
                return null;
            }

            return decisionRequiredByCalculator.Get(source.Preconsented.Value, source.Acknowledged.Value, parameter);
        }
    }
}
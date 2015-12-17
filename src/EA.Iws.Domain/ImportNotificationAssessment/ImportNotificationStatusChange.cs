﻿namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using System;
    using Core.ImportNotificationAssessment;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class ImportNotificationStatusChange : Entity
    {
        protected ImportNotificationStatusChange()
        {
        }

        public ImportNotificationStatusChange(ImportNotificationStatus oldStatus, ImportNotificationStatus newStatus)
        {
            PreviousStatus = oldStatus;
            NewStatus = newStatus;
            ChangeDate = new DateTimeOffset(SystemTime.UtcNow, TimeSpan.Zero);
        }

        public ImportNotificationStatus PreviousStatus { get; private set; }

        public ImportNotificationStatus NewStatus { get; private set; }
        
        public DateTimeOffset ChangeDate { get; private set; }
    }
}
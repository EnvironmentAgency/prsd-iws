﻿namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationAssessment;

    internal class NotificationStatusChangeMapping : EntityTypeConfiguration<NotificationStatusChange>
    {
        public NotificationStatusChangeMapping()
        {
            ToTable("NotificationStatusChange", "Notification");
        }
    }
}
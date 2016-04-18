﻿namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication.Exporter;

    internal class ExporterMapping : EntityTypeConfiguration<Exporter>
    {
        public ExporterMapping()
        {
            ToTable("Exporter", "Notification");
        }
    }
}

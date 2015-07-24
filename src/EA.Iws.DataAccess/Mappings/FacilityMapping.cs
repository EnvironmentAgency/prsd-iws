﻿namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;
    using Domain.NotificationApplication;

    internal class FacilityMapping : EntityTypeConfiguration<Facility>
    {
        public FacilityMapping()
        {
            ToTable("Facility", "Business");

            Property(x => x.IsActualSiteOfTreatment).IsRequired();
        }
    }
}

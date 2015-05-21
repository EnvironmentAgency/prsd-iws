﻿namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class OrganisationMapping : EntityTypeConfiguration<Organisation>
    {
        public OrganisationMapping()
        {
            this.ToTable("Organisation", "Business");

            Property(x => x.Name).IsRequired().HasMaxLength(2048);
            Property(x => x.RegistrationNumber).HasMaxLength(64);
            Property(x => x.Type).IsRequired().HasMaxLength(64);
        }
    }
}

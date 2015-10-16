﻿namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Movement;

    internal class MovementReceiptMapping : EntityTypeConfiguration<MovementReceipt>
    {
        public MovementReceiptMapping()
        {
            ToTable("MovementReceipt", "Notification");
        }
    }
}

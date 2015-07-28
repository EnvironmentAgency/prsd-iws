﻿namespace EA.Iws.DataAccess.Mappings
{
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Helpers;

    internal class FinancialGuaranteeMapping : EntityTypeConfiguration<FinancialGuarantee>
    {
        public FinancialGuaranteeMapping()
        {
            ToTable("FinancialGuarantee", "Notification");

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<FinancialGuarantee, ICollection<FinancialGuaranteeStatusChange>>(
                    "StatusChangeCollection"))
                .WithRequired()
                .Map(m => m.MapKey("FinancialGuaranteeId"));
        }
    }
}

namespace EA.Iws.Core.Admin
{
    using System;
    using FinancialGuarantee;

    public class FinancialGuaranteeData
    {
        public DateTime ReceivedDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public DateTime? DecisionRequiredDate { get; set; }

        public FinancialGuaranteeStatus Status { get; set; }

        public FinancialGuaranteeDecision Decision { get; set; }

        public DateTime? DecisionDate { get; set; }

        public int? ActiveLoadsPermitted { get; set; }

        public string RefusalReason { get; set; }

        public string ReferenceNumber { get; set; }

        public bool? IsBlanketBond { get; set; }
    }
}
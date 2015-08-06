﻿namespace EA.Iws.Requests.Admin.FinancialGuarantee
{
    using System;
    using Prsd.Core;

    public class ApproveFinancialGuarantee : FinancialGuaranteeDecisionRequest
    {
        public DateTime ApprovedFrom { get; private set; }

        public DateTime ApprovedTo { get; private set; }

        public int ActiveLoadsPermitted { get; set; }

        public ApproveFinancialGuarantee(Guid notificationId, 
            DateTime decisionDate,
            DateTime approvedFrom,
            DateTime approvedTo,
            int activeLoadsPermitted)
        {
            if (approvedFrom > approvedTo)
            {
                throw new ArgumentException("Approved from date must be before approved to date.");
            }

            Guard.ArgumentNotZeroOrNegative(() => activeLoadsPermitted, activeLoadsPermitted);

            NotificationId = notificationId;
            DecisionDate = decisionDate;
            ApprovedFrom = approvedFrom;
            ApprovedTo = approvedTo;
            ActiveLoadsPermitted = activeLoadsPermitted;
        }
    }
}

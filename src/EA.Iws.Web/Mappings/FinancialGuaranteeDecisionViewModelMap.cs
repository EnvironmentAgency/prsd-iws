﻿namespace EA.Iws.Web.Mappings
{
    using System;
    using Areas.AdminExportAssessment.ViewModels.FinancialGuarantee;
    using Core.Admin;
    using Core.FinancialGuarantee;
    using Prsd.Core.Mapper;
    using Requests.Admin.FinancialGuarantee;

    public class FinancialGuaranteeDecisionViewModelMap : IMapWithParameter<FinancialGuaranteeDecisionViewModel, Guid, FinancialGuaranteeDecisionRequest>
    {
        public FinancialGuaranteeDecisionRequest Map(FinancialGuaranteeDecisionViewModel source, Guid id)
        {
            switch (source.Decision)
            {
                case FinancialGuaranteeDecision.Approved:
                    return new ApproveFinancialGuarantee(id,
                        source.FinancialGuaranteeId,
                        source.DecisionMadeDate.AsDateTime().Value,
                        source.ReferenceNumber,
                        source.ActiveLoadsPermitted.Value,
                        source.IsBlanketBond.GetValueOrDefault());
                case FinancialGuaranteeDecision.Refused:
                    return new RefuseFinancialGuarantee(id,
                        source.FinancialGuaranteeId,
                        source.DecisionMadeDate.AsDateTime().Value, 
                        source.ReasonForRefusal);
                case FinancialGuaranteeDecision.Released:
                    return new ReleaseFinancialGuarantee(id,
                        source.FinancialGuaranteeId,
                        source.DecisionMadeDate.AsDateTime().Value);
            }

            return null;
        }
    }
}
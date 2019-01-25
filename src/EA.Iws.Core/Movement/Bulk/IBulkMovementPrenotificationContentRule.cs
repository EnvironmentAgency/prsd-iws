﻿namespace EA.Iws.Core.Movement.Bulk
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;

    public interface IBulkMovementPrenotificationContentRule
    {
        Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<ContentRulesDTO> shipments);
    }
}

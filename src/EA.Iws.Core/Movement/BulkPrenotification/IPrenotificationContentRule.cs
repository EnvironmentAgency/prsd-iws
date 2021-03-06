﻿namespace EA.Iws.Core.Movement.BulkPrenotification
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPrenotificationContentRule
    {
        Task<PrenotificationContentRuleResult<PrenotificationContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId);
    }
}

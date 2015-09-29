﻿namespace EA.Iws.Requests.WasteCodes
{
    using System;
    using System.Collections.Generic;

    [NotificationReadOnlyAuthorize]
    public class SetHCodes : BaseSetCodes
    {
        public SetHCodes(Guid id, IEnumerable<Guid> codes, bool isNotApplicable)
            : base(id, codes, isNotApplicable)
        {
        }
    }
}

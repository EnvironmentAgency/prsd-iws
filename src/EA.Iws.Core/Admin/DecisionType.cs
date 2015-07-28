﻿namespace EA.Iws.Core.Admin
{
    using System.ComponentModel.DataAnnotations;

    public enum DecisionType
    {
        [Display(Name = "Consent")]
        Consent = 1,
        [Display(Name = "Object")]
        Object = 2,
        [Display(Name = "Withdrawn")]
        Withdrawn = 3,
        [Display(Name = "Consent Withdrawn")]
        ConsentWithdrawn = 4
    }
}

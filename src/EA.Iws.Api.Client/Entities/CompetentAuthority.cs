﻿namespace EA.Iws.Api.Client.Entities
{
    using System.ComponentModel.DataAnnotations;

    public enum CompetentAuthority
    {
        [Display(Name = "Environment Agency")]
        England = 1,
        [Display(Name = "Scottish Environment Protection Agency")]
        Scotland = 2,
        [Display(Name = "Northern Ireland Environment Agency")]
        NorthernIreland = 3,
        [Display(Name = "Natural Resources Wales")]
        Wales = 4
    }
}
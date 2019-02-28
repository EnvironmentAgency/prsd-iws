﻿namespace EA.Iws.Core.Reports.Producer
{
    using System.ComponentModel.DataAnnotations;

    public enum ProducerReportTextFields
    {
        [Display(Name = "Notifier name")]
        NotifierName,

        [Display(Name = "Consignee name")]
        ConsigneeName,

        [Display(Name = "Waste type")]
        WasteType,

        [Display(Name = "EWC code")]
        EwcCode,

        [Display(Name = "Y code")]
        YCode,

        [Display(Name = "Point of exit")]
        PointOfExit,

        [Display(Name = "Point of entry")]
        PointOfEntry,

        [Display(Name = "Country of despatch")]
        CountryOfDespatch,

        [Display(Name = "Country of destination")]
        CountryOfDestination,

        [Display(Name = "Site of export name")]
        SiteOfExportName,

        [Display(Name = "Facility name")]
        FacilityName
    }
}
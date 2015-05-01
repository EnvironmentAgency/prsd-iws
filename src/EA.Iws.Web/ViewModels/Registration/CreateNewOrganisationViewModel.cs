﻿namespace EA.Iws.Web.ViewModels.Registration
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;
    using Api.Client.Entities;

    public class CreateNewOrganisationViewModel
    {
        public IEnumerable<CountryData> Countries { get; set; } 

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrganisationId { get; set; }

        [Required]
        [Display(Name = "Organisation name")]
        public string Name { get; set; }

        [Display(Name = "Companies House Number")]
        public string CompaniesHouseReference { get; set; }

        [Required]
        [Display(Name = "Address line 3")]
        public string Address3 { get; set; }

        public string CountyOrProvince { get; set; }

        [Required]
        [Display(Name = "Address line 1")]
        public string Address1 { get; set; }

        [Display(Name = "Address line 2")]
        public string Address2 { get; set; }

        [Required]
        public string Postcode { get; set; }
        public string Country { get; set; }

        [Required]
        [Display(Name = "Organisation Type")]
        public string EntityType { get; set; }
    }
}
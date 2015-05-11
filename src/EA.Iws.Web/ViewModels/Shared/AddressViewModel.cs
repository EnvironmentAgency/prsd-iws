﻿namespace EA.Iws.Web.ViewModels.Shared
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Api.Client.Entities;

    public class AddressViewModel
    {
        private const string DefaultCountryName = "United Kingdom";

        public IEnumerable<CountryData> Countries { get; set; }

        [Required]
        [Display(Name = "Building name or number")]
        public string Building { get; set; }

        [Required]
        [Display(Name = "Address line 1")]
        public string Address1 { get; set; }

        [Display(Name = "Address line 2")]
        public string Address2 { get; set; }

        [Required]
        [Display(Name = "Town or city")]
        public string TownOrCity { get; set; }

        [Display(Name = "County")]
        public string County { get; set; }

        [Required]
        public string Postcode { get; set; }

        [Required]
        public string Country { get; set; }

        public CountryData DefaultCountry
        {
            get
            {
                if (Countries == null || !Countries.Any())
                {
                    return null;
                }
                var country = Countries.SingleOrDefault(c => c.Name.Equals(DefaultCountryName));
                return country ?? Countries.First();
            }
        }
    }
}
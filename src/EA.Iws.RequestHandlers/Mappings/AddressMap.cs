﻿namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Domain;
    using Prsd.Core.Mapper;
    using Requests.Shared;

    public class AddressMap : IMap<Address, AddressData>
    {
        private readonly IwsContext context;

        public AddressMap(IwsContext context)
        {
            this.context = context;            
        }

        public AddressData Map(Address source)
        {
            var countryId = context.Countries.Single(c => c.Name.Equals(source.Country, StringComparison.InvariantCultureIgnoreCase)).Id;

            return new AddressData
            {
                Building = source.Building,
                StreetOrSuburb = source.Address1,
                Address2 = source.Address2,
                TownOrCity = source.TownOrCity,
                Region = source.Region,
                PostalCode = source.PostalCode,
                CountryName = source.Country,
                CountryId = countryId
            };
        }
    }
}

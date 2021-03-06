﻿namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Shared;
    using Domain;
    using Prsd.Core.Mapper;

    internal class CompetentAuthorityMap : IMap<CompetentAuthority, CompetentAuthorityData>
    {
        public CompetentAuthorityData Map(CompetentAuthority source)
        {
            return new CompetentAuthorityData
            {
                Id = source.Id,
                CountryId = source.Country.Id,
                Abbreviation = source.Abbreviation,
                Code = source.Code,
                IsSystemUser = source.IsSystemUser,
                Name = source.Name,
            };
        }
    }
}

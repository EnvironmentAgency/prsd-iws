﻿namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain;
    using Domain.TransportRoute;

    public class EntryOrExitPointRepository : IEntryOrExitPointRepository
    {
        private readonly IwsContext context;

        public EntryOrExitPointRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<EntryOrExitPoint> GetById(Guid id)
        {
            return await context.EntryOrExitPoints.SingleAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<EntryOrExitPoint>> GetForCountry(Country country)
        {
            return await GetForCountry(country.Id);
        }

        public async Task<IEnumerable<EntryOrExitPoint>> GetForCountry(Guid countryId)
        {
            return await context.EntryOrExitPoints.Where(e => e.Country.Id == countryId).ToArrayAsync();
        }

        public async Task<IEnumerable<EntryOrExitPoint>> GetAll()
        {
            return await context.EntryOrExitPoints.ToArrayAsync();
        }
    }
}

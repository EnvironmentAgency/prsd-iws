﻿namespace EA.Iws.RequestHandlers.StateOfImport
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.StateOfImport;

    internal class AddStateOfImportToNotificationHandler : IRequestHandler<AddStateOfImportToNotification, Guid>
    {
        private readonly IwsContext context;

        public AddStateOfImportToNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(AddStateOfImportToNotification message)
        {
            var notification = await context.NotificationApplications.SingleAsync(na => na.Id == message.NotificationId);

            var country = await context.Countries.SingleAsync(c => c.Id == message.CountryId);
            var competentAuthority =
                await context.CompetentAuthorities.SingleAsync(ca => ca.Id == message.CompetentAuthorityId);
            var entryPoint = await context.EntryOrExitPoints.SingleAsync(ep => ep.Id == message.EntryOrExitPointId);

            var stateOfImport = new StateOfImport(country, competentAuthority, entryPoint);

            notification.AddStateOfImportToNotification(stateOfImport);

            await context.SaveChangesAsync();

            return stateOfImport.Id;
        }
    }
}

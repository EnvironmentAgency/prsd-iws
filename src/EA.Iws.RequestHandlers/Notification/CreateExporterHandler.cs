﻿namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.Notification;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    public class CreateExporterHandler : IRequestHandler<CreateExporter, Guid>
    {
        private readonly IwsContext context;

        public CreateExporterHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(CreateExporter command)
        {
            var country = await context.Countries.SingleAsync(c => c.Id == command.CountryId);

            var address = new Address(command.Building, command.Address1, command.Address2, command.City, command.County, command.PostCode, country.Name);

            var contact = new Contact(command.FirstName, command.LastName, command.Phone, command.Email, command.Fax);

            var business = new Business(command.Name, command.Type, command.RegistrationNumber, command.AdditionalRegistrationNumber);

            var exporter = new Exporter(business, address, contact);

            var notification = await context.NotificationApplications.FindAsync(command.NotificationId);
            notification.AddExporter(exporter);

            await context.SaveChangesAsync();

            return exporter.Id;
        }
    }
}
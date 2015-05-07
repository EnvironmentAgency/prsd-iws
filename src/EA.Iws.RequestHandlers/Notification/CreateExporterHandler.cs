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

            var address = new Address(command.Building, command.Address1, command.City, command.PostCode,
                country, command.Address2);

            var contact = new Contact(command.FirstName, command.LastName, command.Phone, command.Email, command.Fax);

            var exporter = new Exporter(command.Name, command.Type, address, contact, command.CompanyHouseNumber,
                command.RegistrationNumber1, command.RegistrationNumber2);

            var notification = await context.NotificationApplications.FindAsync(command.NotificationId);
            notification.AddExporter(exporter);

            await context.SaveChangesAsync();

            return exporter.Id;
        }
    }
}
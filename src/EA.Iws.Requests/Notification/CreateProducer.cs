﻿namespace EA.Iws.Requests.Notification
{
    using System;
    using Organisations;
    using Prsd.Core.Mediator;

    public class CreateProducer : IRequest<Guid>
    {
        public Guid NotificationId { get; set; }

        public bool IsSiteOfExport { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }
        public string RegistrationNumber { get; set; }
        public string AdditionalRegistrationNumber { get; set; }

        public string Building { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string TownOrCity { get; set; }
        public string County { get; set; }
        public string PostalCode { get; set; }
        public Guid CountryId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
    }
}
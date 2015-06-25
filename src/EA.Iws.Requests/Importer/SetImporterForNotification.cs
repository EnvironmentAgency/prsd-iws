﻿namespace EA.Iws.Requests.Importer
{
    using System;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Shared;

    public class SetImporterForNotification : IRequest<Guid>
    {
        public BusinessData Business { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public Guid NotificationId { get; set; }
    }
}

﻿namespace EA.Iws.Requests.Carriers
{
    using System;
    using Prsd.Core.Mediator;
    using Shared;

    public class AddCarrierToNotification : IRequest<Guid>
    {
        public Guid NotificationId { get; set; }

        public BusinessData Business { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }
    }
}

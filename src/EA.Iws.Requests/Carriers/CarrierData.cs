﻿namespace EA.Iws.Requests.Carriers
{
    using System;
    using Shared;

    public class CarrierData
    {
        public Guid Id { get; set; }

        public Guid NotificationId { get; set; }

        public BusinessData Business { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }
    }
}

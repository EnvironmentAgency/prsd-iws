﻿namespace EA.Iws.Requests.CustomsOffice
{
    using System;
    using Prsd.Core.Mediator;

    public class SetExitCustomsOfficeForNotificationById : IRequest<Guid>
    {
        public Guid Id { get; private set; }
        public Guid CountryId { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        
        public SetExitCustomsOfficeForNotificationById(Guid id, string name, string address, Guid countryId)
        {
            Id = id;
            Name = name;
            Address = address;
            CountryId = countryId;
        }
    }
}

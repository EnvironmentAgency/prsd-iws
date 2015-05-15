﻿namespace EA.Iws.Requests.Organisations
{
    using System;
    using Shared;

    public class OrganisationData
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public AddressData Address { get; set; }
    }
}

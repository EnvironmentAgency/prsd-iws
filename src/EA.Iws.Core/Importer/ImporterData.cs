﻿namespace EA.Iws.Core.Importer
{
    using System;
    using Shared;

    public class ImporterData
    {
        public Guid Id { get; set; }

        public bool HasImporter { get; set; }

        public BusinessInfoData Business { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public Guid NotificationId { get; set; }

        public ImporterData()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new BusinessInfoData();
        }
    }
}

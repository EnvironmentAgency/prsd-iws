﻿namespace EA.Iws.Domain.Notification
{
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class Producer : Entity
    {
        public Producer(Business business, Address address, Contact contact)
        {
            Guard.ArgumentNotNull(business);
            Guard.ArgumentNotNull(address);
            Guard.ArgumentNotNull(contact);

            Business = business;
            Address = address;
            Contact = contact;
        }

        protected Producer()
        {
        }

        public bool IsSiteOfExport { get; internal set; }

        public Business Business { get; private set; }

        public Address Address { get; private set; }

        public Contact Contact { get; private set; }
    }
}
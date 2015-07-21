﻿namespace EA.Iws.Domain
{
    using System;
    using Core.Admin;
    using Prsd.Core;

    public class User
    {
        protected User()
        {
        }

        public User(string id, string firstName, string surname, string phoneNumber, string email)
        {
            Guard.ArgumentNotNullOrEmpty(() => firstName, firstName);
            Guard.ArgumentNotNullOrEmpty(() => surname, surname);
            Guard.ArgumentNotNullOrEmpty(() => phoneNumber, phoneNumber);
            Guard.ArgumentNotNullOrEmpty(() => email, email);
            Guard.ArgumentNotNullOrEmpty(() => id, id);

            FirstName = firstName;
            Surname = surname;
            PhoneNumber = phoneNumber;
            Email = email;
            Id = id;
        }

        public string Id { get; private set; }

        public string FirstName { get; private set; }

        public string Surname { get; private set; }

        public string PhoneNumber { get; private set; }

        public string Email { get; private set; }

        public string JobTitle { get; private set; }

        public string CompetentAuthority { get; private set; }

        public string LocalArea { get; private set; }

        public bool IsAdmin { get; private set; }

        public InternalUserStatus? InternalUserStatus { get; private set; }

        public virtual Organisation Organisation { get; private set; }

        public void LinkToOrganisation(Organisation organisation)
        {
            Guard.ArgumentNotNull(() => organisation, organisation);

            if (Organisation != null)
            {
                throw new InvalidOperationException(
                    string.Format("User {0} is already linked to an organisation and may not be linked to another. This user is linked to organisation: {1}", Id, Organisation.Id));
            }

            Organisation = organisation;
        }

        public void Approve()
        {
            if (!IsAdmin)
            {
                throw new InvalidOperationException(string.Format("Cannot set an internal user status of approved for an external user. Id: {0}", Id));
            }

            InternalUserStatus = Core.Admin.InternalUserStatus.Approved;
        }

        public void Reject()
        {
            if (!IsAdmin)
            {
                throw new InvalidOperationException(string.Format("Cannot set an internal user status of rejected for an external user. Id: {0}", Id));
            }

            InternalUserStatus = Core.Admin.InternalUserStatus.Rejected;
        }
    }
}
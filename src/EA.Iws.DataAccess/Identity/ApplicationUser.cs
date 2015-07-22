﻿namespace EA.Iws.DataAccess.Identity
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Core.Admin;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string JobTitle { get; set; }
        public string LocalArea { get; set; }
        public string CompetentAuthority { get; set; }
        public bool IsInternal { get; set; }
        public InternalUserStatus? InternalUserStatus { get; set; }
        public Guid? OrganisationId { get; private set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
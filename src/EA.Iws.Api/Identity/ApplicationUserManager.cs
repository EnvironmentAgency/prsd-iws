﻿namespace EA.Iws.Api.Identity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using DataAccess.Identity;
    using Infrastructure.Services;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security.DataProtection;
    using RequestHandlers.Authorization;
    using Services;
    using ClaimTypes = Core.Shared.ClaimTypes;

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        private readonly IClaimsRepository claimsRepository;
        private readonly ConfigurationService configurationService;

        public ApplicationUserManager(IUserStore<ApplicationUser> store,
            IDataProtectionProvider dataProtectionProvider,
            IClaimsRepository claimsRepository,
            ConfigurationService configurationService)
            : base(store)
        {
            this.claimsRepository = claimsRepository;
            this.configurationService = configurationService;

            UserValidator = new UserValidator<ApplicationUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            PasswordValidator = new PasswordPolicy();

            // Configure user lockout defaults
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            UserTokenProvider =
                new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
        }

        public override async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            SetEmailConfirmedIfRequired(user);

            return await base.CreateAsync(user);
        }

        private void SetEmailConfirmedIfRequired(ApplicationUser user)
        {
            // We only auto-verify email where the environment is set to development and the user email is valid.
            if (string.IsNullOrWhiteSpace(configurationService.CurrentConfiguration.Environment)
                || !configurationService.CurrentConfiguration.Environment.Equals("Development", StringComparison.InvariantCultureIgnoreCase)
                || string.IsNullOrWhiteSpace(user.Email)
                || !user.Email.Contains('@'))
            {
                return;
            }

            List<string> excludedDomains = new List<string>();

            if (!string.IsNullOrWhiteSpace(configurationService.CurrentConfiguration.VerificationEmailTestDomains))
            {
                // Get the domains for which email verification is still required.
                excludedDomains =
                    configurationService.CurrentConfiguration.VerificationEmailTestDomains.Split(new[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            int domainStarts = user.Email.LastIndexOf("@");
            var excludeThisEmail = excludedDomains.Any(d => user.Email.Substring(domainStarts).Contains(d));

            if (!excludeThisEmail)
            {
                user.EmailConfirmed = true;
            }
        }

        public override async Task<IList<Claim>> GetClaimsAsync(string userId)
        {
            var user = await Store.FindByIdAsync(userId);

            var claims = await base.GetClaimsAsync(userId);

            if (user == null)
            {
                return claims;
            }

            if (user.OrganisationId.HasValue)
            {
                claims.Add(new Claim(ClaimTypes.OrganisationId, user.OrganisationId.Value.ToString()));
            }

            claims.Add(new Claim(System.Security.Claims.ClaimTypes.Name, string.Format("{0} {1}", user.FirstName, user.Surname)));
            claims.Add(new Claim(System.Security.Claims.ClaimTypes.Email, user.Email));

            var userClaims = await claimsRepository.GetUserClaims(userId);

            foreach (var claim in userClaims)
            {
                claims.Add(claim);
            }

            return claims;
        }
    }
}
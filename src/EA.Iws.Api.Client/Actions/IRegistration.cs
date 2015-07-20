﻿namespace EA.Iws.Api.Client.Actions
{
    using System.Threading.Tasks;
    using Entities;

    public interface IRegistration
    {
        Task<string> RegisterApplicantAsync(ApplicantRegistrationData applicantRegistrationData);
        Task<string> RegisterAdminAsync(AdminRegistrationData adminRegistrationData);
        Task<bool> VerifyEmailAsync(VerifiedEmailData verifiedEmailData);
        Task<string> GetUserEmailVerificationTokenAsync(string accessToken);
    }
}
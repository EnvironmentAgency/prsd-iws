﻿namespace EA.Iws.Api.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using Client.Entities;
    using Core.Authorization;
    using DataAccess.Identity;
    using EmailMessaging;
    using Identity;
    using Microsoft.AspNet.Identity;
    using Prsd.Core.Domain;

    [RoutePrefix("api/Registration")]
    [Authorize]
    public class RegistrationController : ApiController
    {
        private readonly IUserContext userContext;
        private readonly IEmailService emailService;
        private readonly ApplicationUserManager userManager;

        public RegistrationController(ApplicationUserManager userManager,
            IUserContext userContext,
            IEmailService emailService)
        {
            this.userContext = userContext;
            this.emailService = emailService;
            this.userManager = userManager;
        }

        [Route("Register")]
        public async Task<IHttpActionResult> Register(ApplicantRegistrationData model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.Phone,
                FirstName = model.FirstName,
                Surname = model.Surname,
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await userManager.AddClaimAsync(user.Id,
                new Claim(ClaimTypes.Role, UserRole.External.ToString().ToLowerInvariant()));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok(user.Id);
        }

        [Route("RegisterAdmin")]
        public async Task<IHttpActionResult> RegisterAdmin(AdminRegistrationData model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                Surname = model.Surname
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await userManager.AddClaimAsync(user.Id,
                new Claim(ClaimTypes.Role, UserRole.Internal.ToString().ToLowerInvariant()));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok(user.Id);
        }

        [HttpPost]
        [Route("SendEmailVerification")]
        public async Task<IHttpActionResult> SendEmailVerification(EmailVerificationData model)
        {
            var userId = userContext.UserId.ToString();
            var token = await userManager.GenerateEmailConfirmationTokenAsync(userId);
            var email = await userManager.GetEmailAsync(userId);

            var emailModel = new { VerifyLink = GetEmailVerificationUrl(model.Url, token, userId) };

            var result = await emailService.SendEmail("VerifyEmailAddress", email, "Verify your email address", emailModel);

            return Ok(result);
        }

        [HttpPost]
        [Route("VerifyEmail")]
        public async Task<IHttpActionResult> VerifyEmail(VerifiedEmailData model)
        {
            var result = await userManager.ConfirmEmailAsync(model.Id.ToString(), model.Code);

            return Ok(result.Succeeded);
        }

        [HttpGet]
        [Route("GetApplicantDetails")]
        public async Task<EditApplicantRegistrationData> GetApplicantDetails()
        {
            var result = await userManager.FindByIdAsync(userContext.UserId.ToString());
            return new EditApplicantRegistrationData()
            {
                Id = new Guid(result.Id),
                FirstName = result.FirstName,
                Surname = result.Surname,
                Phone = result.PhoneNumber,
                Email = result.Email
            };
        }

        [HttpPost]
        [Route("UpdateApplicantDetails")]
        public async Task<IHttpActionResult> UpdateApplicantDetails(EditApplicantRegistrationData model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = model.Id.ToString();
            var user = await userManager.FindByIdAsync(userId);
            user.FirstName = model.FirstName;
            user.Surname = model.Surname;
            user.PhoneNumber = model.Phone;

            if (!user.Email.Equals(model.Email))
            {
                user.Email = model.Email;
                user.UserName = model.Email;
            }

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok(user.Id);
        }

        [HttpPost]
        [Route("ResetPasswordRequest")]
        public async Task<IHttpActionResult> ResetPasswordRequest(PasswordResetRequest model)
        {
            var user = await userManager.FindByEmailAsync(model.EmailAddress);
            if (user != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user.Id);

                var emailModel = new { PasswordResetUrl = GetEmailVerificationUrl(model.Url, token, user.Id) };

                await emailService.SendEmail("PasswordResetRequest", model.EmailAddress, "Reset your IWS password", emailModel);
                return Ok(true);
            }

            return Ok(true);
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IHttpActionResult> ResetPassword(PasswordResetData model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await userManager.ResetPasswordAsync(model.UserId.ToString(), model.Token, model.Password);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
            }
            catch (InvalidOperationException)
            {
                // Because an invalid token or an invalid password does not throw an error on reset, we can say the only other parameter (user Id) is invalid
                ModelState.AddModelError(string.Empty, "User not recognised");
                return BadRequest(ModelState);
            }

            return Ok(true);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        //We are using the email address as the username so this avoids duplicate validation error message
                        if (!error.StartsWith("Name"))
                        {
                            ModelState.AddModelError(string.Empty, error);
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        /// <summary>
        /// Generates the correct verification URL for a user to verify their email.
        /// </summary>
        private string GetEmailVerificationUrl(string baseUrl, string verificationToken, string userId)
        {
            var uriBuilder = new UriBuilder(baseUrl);
            uriBuilder.Path += "/" + userId;
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["code"] = verificationToken;
            uriBuilder.Query = parameters.ToString();
            return uriBuilder.Uri.ToString();
        }
    }
}
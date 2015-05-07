﻿namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Api.Client.Entities;
    using Infrastructure;
    using Microsoft.Owin.Security;
    using Requests.Organisations;
    using Requests.Registration;
    using ViewModels.Registration;

    [Authorize]
    public class RegistrationController : Controller
    {
        private readonly IAuthenticationManager authenticationManager;
        private readonly Func<IIwsOAuthClient> oauthClient;
        private readonly Func<IIwsClient> apiClient;

        public RegistrationController(Func<IIwsOAuthClient> oauthClient, Func<IIwsClient> apiClient, IAuthenticationManager authenticationManager)
        {
            this.oauthClient = oauthClient;
            this.apiClient = apiClient;
            this.authenticationManager = authenticationManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ApplicantRegistration()
        {
            return View(new ApplicantRegistrationViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ApplicantRegistration(ApplicantRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var client = apiClient())
                {
                    var applicantRegistrationData = new ApplicantRegistrationData
                    {
                        Email = model.Email,
                        FirstName = model.Name,
                        Surname = model.Surname,
                        Phone = model.PhoneNumber,
                        Password = model.Password,
                        ConfirmPassword = model.ConfirmPassword
                    };

                    var response = await client.Registration.RegisterApplicantAsync(applicantRegistrationData);

                    if (!response.HasErrors)
                    {
                        var signInResponse = await oauthClient().GetAccessTokenAsync(model.Email, model.Password);
                        authenticationManager.SignIn(signInResponse.GenerateUserIdentity());

                        return RedirectToAction("SelectOrganisation", new { organisationName = model.OrganisationName });
                    }
                    this.AddValidationErrorsToModelState(response);
                    return View("ApplicantRegistration", model);
                }
            }
            return View("ApplicantRegistration", model);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> SelectOrganisation(string organisationName)
        {
            var model = new SelectOrganisationViewModel
            {
                Name = organisationName
            };

            if (string.IsNullOrEmpty(organisationName))
            {
                model.Organisations = null;
            }
            else
            {
                using (var client = apiClient())
                {
                    var response = await client.SendAsync(User.GetAccessToken(), new FindMatchingOrganisations(organisationName));

                    if (response.HasErrors)
                    {
                        // TODO - error handling
                    }

                    model.Organisations = response.Result;
                }
            }

            return View("SelectOrganisation", model);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> SelectOrganisation(SelectOrganisationViewModel model, string submitButton)
        {
            Guid selectedGuid;
            if (!Guid.TryParse(submitButton, out selectedGuid) ||
                model.Organisations.SingleOrDefault(o => o.Id == selectedGuid) == null)
            {
                return RedirectToAction("SelectOrganisation", new { organisationName = model.Name });
            }

            using (var client = apiClient())
            {
                var response = await client.SendAsync(User.GetAccessToken(), new LinkUserToOrganisation(selectedGuid));

                if (response.HasErrors)
                {
                    // TODO - error handling
                }

                if (response.Result)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("SelectOrganisation", new { organisationName = model.Name });
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> CreateNewOrganisation(string organisationName)
        {
            var model = new CreateNewOrganisationViewModel { Name = organisationName, Countries = await GetCountries() };
            return View("CreateNewOrganisation", model);
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewOrganisation(CreateNewOrganisationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Countries = await GetCountries();
                return View("CreateNewOrganisation", model);
            }

            var organisationRegistrationData = new OrganisationRegistrationData
            {
                Name = model.Name,
                Building = model.Building,
                Address1 = model.Address1,
                Address2 = model.Address2,
                TownOrCity = model.TownOrCity,
                Postcode = model.Postcode,
                CountryId = model.CountryId,
                EntityType = model.EntityType,
                CompaniesHouseNumber = model.CompaniesHouseReference
            };

            using (var client = apiClient())
            {
                var response =
                    await
                        client.SendAsync(User.GetAccessToken(), new CreateOrganisation(organisationRegistrationData));

                if (!response.HasErrors)
                {
                    return RedirectToAction("Home", "Applicant");
                }

                this.AddValidationErrorsToModelState(response);
                return View("CreateNewOrganisation", "Registration", model);
            }
        }

        // TODO - duplicated in NotificationApplicationController, need to refactor.
        private async Task<IEnumerable<CountryData>> GetCountries()
        {
            using (var client = apiClient())
            {
                var response = await client.SendAsync(new GetCountries());

                if (response.HasErrors)
                {
                    // TODO - error handling
                }

                return response.Result;
            }
        }
    }
}
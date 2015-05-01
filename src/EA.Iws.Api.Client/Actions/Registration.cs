﻿namespace EA.Iws.Api.Client.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Entities;
    using Extensions;
    using Newtonsoft.Json;

    internal class Registration : IRegistration
    {
        private readonly HttpClient httpClient;
        private readonly string controller = "Registration/";

        public Registration(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> RegisterApplicantAsync(ApplicantRegistrationData applicantRegistrationData)
        {
            return await httpClient.PostAsJsonAsync(controller + "Register", applicantRegistrationData);
        }

        public async Task<HttpResponseMessage> RegisterOrganisationAsync(string accessToken, OrganisationRegistrationData organisationRegistrationData)
        {
            return await httpClient.PostAsJsonAsync(accessToken, controller + "Register", organisationRegistrationData);
        }

        public async Task<OrganisationData[]> SearchOrganisationAsync(string accessToken, string organisationName)
        {
            organisationName = organisationName.Replace(".", string.Empty);

            OrganisationData[] organisations = await httpClient.GetAsync<OrganisationData[]>(accessToken, controller + "OrganisationSearch/" + organisationName);

            return organisations;
        }

        public async Task<HttpResponseMessage> LinkUserToOrganisationAsync(string accessToken, Guid organisationId)
        {
            return await httpClient.PostAsJsonAsync(accessToken, controller + "OrganisationSelect", new OrganisationLinkData{ OrganisationId = organisationId});
        }
        public async Task<IEnumerable<CountryData>> GetCountriesAsync()
        {
            var task = await httpClient.GetAsync("Registration/GetCountries");
            var jsonString = await task.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CountryData[]>(jsonString).ToList();
        }

    }
}
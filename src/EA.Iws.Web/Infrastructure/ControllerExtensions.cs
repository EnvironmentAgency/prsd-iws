﻿namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Requests.Registration;

    public static class ControllerExtensions
    {
        public static async Task<Guid> BindCountryList(this Controller controller, Func<IIwsClient> apiClient)
        {
            using (var client = apiClient())
            {
                return await controller.BindCountryList(client);
            }
        }

        public static async Task<Guid> BindCountryList(this Controller controller, IIwsClient client)
        {
            var response = await client.SendAsync(new GetCountries());

            var defaultId = response.Single(c => c.Name.Equals("United Kingdom", StringComparison.InvariantCultureIgnoreCase)).Id;

            controller.ViewBag.Countries = new SelectList(response, "Id", "Name", defaultId);

            return defaultId;
        }
    }
}
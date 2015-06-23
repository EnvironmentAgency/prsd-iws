﻿namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core;
    using Core.CustomsOffice;
    using Core.Shared;
    using Infrastructure;
    using Requests.CustomsOffice;
    using Requests.Shared;
    using ViewModels;
    using ViewModels.CustomsOffice;

    [Authorize]
    public class ExitCustomsOfficeController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public ExitCustomsOfficeController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id)
        {
            using (var client = apiClient())
            {
                var data = await client.SendAsync(User.GetAccessToken(), new GetExitCustomsOfficeAddDataByNotificationId(id));

                if (data.CustomsOffices != CustomsOffices.EntryAndExit
                    && data.CustomsOffices != CustomsOffices.Exit)
                {
                    return RedirectToAction("Index", "CustomsOffice", new { id });
                }

                CustomsOfficeViewModel model;
                if (data.CustomsOfficeData != null)
                {
                    model = new CustomsOfficeViewModel
                    {
                        Address = data.CustomsOfficeData.Address,
                        Name = data.CustomsOfficeData.Name,
                        SelectedCountry = data.CustomsOfficeData.Country.Id,
                        Countries = new SelectList(data.Countries, "Id", "Name", data.CustomsOfficeData.Country.Id)
                    };
                }
                else
                {
                    model = new CustomsOfficeViewModel
                    {
                        Countries = new SelectList(data.Countries, "Id", "Name")
                    };
                }

                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Add(Guid id, CustomsOfficeViewModel model)
        {
            using (var client = apiClient())
            {
                var countries = await client.SendAsync(User.GetAccessToken(), new GetEuropeanUnionCountries());

                model.Countries = model.SelectedCountry.HasValue
                    ? new SelectList(countries, "Id", "Name", model.SelectedCountry.Value)
                    : new SelectList(countries, "Id", "Name");

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                await client.SendAsync(User.GetAccessToken(), new SetExitCustomsOfficeForNotificationById(id,
                    model.Name,
                    model.Address,
                    model.SelectedCountry.Value));

                return RedirectToAction("Index", "CustomsOffice", new { id });
            }
        }
    }
}
﻿namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.Shared;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Facilities;
    using Requests.Importer;
    using Requests.Notification;
    using ViewModels.Facility;
    using Web.ViewModels.Shared;

    [Authorize]
    public class FacilityController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public FacilityController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id, bool? copy)
        {
            var facility = new AddFacilityViewModel();
            using (var client = apiClient())
            {
                if (copy.HasValue && copy.Value)
                {
                    var importer = await client.SendAsync(User.GetAccessToken(), new GetImporterByNotificationId(id));

                    facility.Address = importer.Address;
                    facility.Contact = importer.Contact;
                    facility.Business = new BusinessTypeViewModel(importer.Business);
                }

                var response =
                    await client.SendAsync(User.GetAccessToken(), new GetNotificationInfo(id));
                facility.NotificationType = response.NotificationType;
                facility.NotificationId = id;

                await this.BindCountryList(client, false);
                facility.Address.DefaultCountryId = this.GetDefaultCountryId();
            }
            return View(facility);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(AddFacilityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(apiClient, false);
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    await client.SendAsync(User.GetAccessToken(), model.ToRequest());

                    return RedirectToAction("List", "Facility",
                        new { id = model.NotificationId });
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);
                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }
                await this.BindCountryList(client, false);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id, Guid entityId)
        {
            using (var client = apiClient())
            {
                var facility =
                    await
                        client.SendAsync(User.GetAccessToken(),
                            new GetFacilityForNotification(id, entityId));

                var response =
                    await client.SendAsync(User.GetAccessToken(), new GetNotificationInfo(id));

                var model = new EditFacilityViewModel(facility) { NotificationType = response.NotificationType };

                await this.BindCountryList(client, false);
                facility.Address.DefaultCountryId = this.GetDefaultCountryId();

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditFacilityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(apiClient, false);
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    var request = model.ToRequest();

                    await client.SendAsync(User.GetAccessToken(), request);

                    return RedirectToAction("List", "Facility",
                        new { id = model.NotificationId });
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);

                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }
                await this.BindCountryList(client, false);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> List(Guid id)
        {
            var model = new MultipleFacilitiesViewModel();
            if (TempData["errorRemoveFacility"] != null)
            {
                ModelState.AddModelError("RemoveFacility", TempData["errorRemoveFacility"].ToString());
            }

            using (var client = apiClient())
            {
                var response =
                    await client.SendAsync(User.GetAccessToken(), new GetFacilitiesByNotificationId(id));

                var notificationInfo =
                    await client.SendAsync(User.GetAccessToken(), new GetNotificationInfo(id));

                model.NotificationId = id;
                model.FacilityData = response.ToList();
                model.NotificationType = notificationInfo.NotificationType;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> List(MultipleFacilitiesViewModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.SelectedValue))
            {
                using (var client = apiClient())
                {
                    try
                    {
                        await
                            client.SendAsync(User.GetAccessToken(),
                                new SetActualSiteOfTreatment(new Guid(model.SelectedValue), model.NotificationId));
                    }
                    catch (ApiBadRequestException ex)
                    {
                        this.HandleBadRequest(ex);

                        if (ModelState.IsValid)
                        {
                            throw;
                        }
                    }
                }
            }

            if (model.NotificationType == NotificationType.Recovery)
            {
                return RedirectToAction("RecoveryPreconsent", "Facility", new { id = model.NotificationId });
            }
            else
            {
                return RedirectToAction("OperationCodes", "WasteOperations", new { id = model.NotificationId });
            }
        }

        [HttpGet]
        public async Task<ActionResult> RecoveryPreconsent(Guid id)
        {
            using (var client = apiClient())
            {
                var preconsentedFacilityData =
                    await client.SendAsync(User.GetAccessToken(), new GetIsPreconsentedRecoveryFacility(id));

                if (preconsentedFacilityData.NotificationType == NotificationType.Disposal)
                {
                    return RedirectToAction("OperationCodes", "WasteOperations", new { id });
                }

                var model = new TrueFalseViewModel { Value = preconsentedFacilityData.IsPreconsentedRecoveryFacility };

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RecoveryPreconsent(Guid id, TrueFalseViewModel model)
        {
            if (!ModelState.IsValid || !model.Value.HasValue)
            {
                ViewBag.NotificationId = id;
                return View(model);
            }

            bool isPreconsented = model.Value.Value;

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), new SetPreconsentedRecoveryFacility(id, isPreconsented));
            }

            return RedirectToAction("OperationCodes", "WasteOperations", new { id });
        }

        [HttpGet]
        public async Task<ActionResult> CopyFromImporter(Guid id)
        {
            var model = new YesNoChoiceViewModel();
            ViewBag.NotificationId = id;

            using (var client = apiClient())
            {
                var notificationInfo =
                    await client.SendAsync(User.GetAccessToken(), new GetNotificationInfo(id));
                ViewBag.NotificationType = notificationInfo.NotificationType.ToString().ToLowerInvariant();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CopyFromImporter(Guid id, YesNoChoiceViewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.NotificationId = id;

                using (var client = apiClient())
                {
                    var notificationInfo =
                        await client.SendAsync(User.GetAccessToken(), new GetNotificationInfo(id));
                    ViewBag.NotificationType = notificationInfo.NotificationType.ToString().ToLowerInvariant();
                }

                return View(inputModel);
            }

            if (inputModel.Choices.SelectedValue.Equals("No"))
            {
                return RedirectToAction("Add", new { id });
            }

            return RedirectToAction("Add", new { id, copy = true });
        }

        [HttpGet]
        public async Task<ActionResult> Remove(Guid id, Guid entityId, string facilityType)
        {
            using (var client = apiClient())
            {
                try
                {
                    var response = await client.SendAsync(User.GetAccessToken(), new GetFacilitiesByNotificationId(id));
                    var facilities = response.ToList();
                    int facilitiesCount = facilities.Count;
                    bool isActualSiteOfTreatment = facilities.Single(x => x.Id == entityId).IsActualSiteOfTreatment;

                    if (isActualSiteOfTreatment && facilitiesCount > 1)
                    {
                        TempData["errorRemoveFacility"] = String.Format("You have chosen to remove the actual site. " +
                        "You will need to select an alternative actual site of {0} before you can remove this facility.", facilityType.ToLowerInvariant());
                        return RedirectToAction("List", "Facility", new { id });
                    }

                    await client.SendAsync(User.GetAccessToken(), new DeleteFacilityForNotification(id, entityId));
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);
                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction("List", "Facility", new { id });
        }
    }
}
﻿namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Shipment;
    using ViewModels.Shared;
    using ViewModels.Shipment;

    [Authorize]
    public class ShipmentController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public ShipmentController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Info(Guid id)
        {
            using (var client = apiClient())
            {
                var shipmentData =
                    await
                        client.SendAsync(User.GetAccessToken(),
                            new GetShipmentInfoForNotification(id));

                var model = new ShipmentInfoViewModel(shipmentData);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Info(ShipmentInfoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    await client.SendAsync(User.GetAccessToken(),
                        model.ToRequest());

                    return RedirectToAction("ChemicalComposition", "WasteType", new { id = model.NotificationId });
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);

                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }

                return View(model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> PackagingTypes(Guid id)
        {
            var packagingTypes = CheckBoxCollectionViewModel.CreateFromEnum<PackagingType>();
            packagingTypes.ShowEnumValue = true;

            //We need to exclude 'other' as this will be handled separately
            packagingTypes.PossibleValues =
                packagingTypes.PossibleValues.Where(p => (PackagingType)Convert.ToInt32(p.Value) != PackagingType.Other)
                    .ToList();

            var model = new PackagingTypesViewModel
            {
                PackagingTypes = packagingTypes,
                NotificationId = id
            };

            using (var client = apiClient())
            {
                var packagingData =
                    await client.SendAsync(User.GetAccessToken(), new GetPackagingTypesForNotification(id));

                model.PackagingTypes.SetSelectedValues(packagingData.PackagingTypes);
                if (!string.IsNullOrWhiteSpace(packagingData.OtherDescription))
                {
                    model.OtherSelected = true;
                    model.OtherDescription = packagingData.OtherDescription;
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PackagingTypes(PackagingTypesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    var selectedPackagingTypes =
                        model.PackagingTypes.PossibleValues.Where(p => p.Selected)
                            .Select(p => (PackagingType)(Convert.ToInt32(p.Value)))
                            .ToList();

                    if (model.OtherSelected)
                    {
                        selectedPackagingTypes.Add(PackagingType.Other);
                    }

                    if (!selectedPackagingTypes.Any())
                    {
                        ModelState.AddModelError(string.Empty, "Please select at least one option");
                        return View(model);
                    }

                    await client.SendAsync(User.GetAccessToken(),
                        new SetPackagingTypeOnShipmentInfo(selectedPackagingTypes, model.NotificationId,
                            model.OtherDescription));

                    return RedirectToAction("Index", "SpecialHandling", new { id = model.NotificationId });
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);

                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }
                return View(model);
            }
        }
    }
}
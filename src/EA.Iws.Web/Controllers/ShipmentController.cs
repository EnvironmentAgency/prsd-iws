﻿namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Notification;
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
        public ActionResult Info(Guid id)
        {
            var model = new ShipmentInfoViewModel { NotificationId = id, UnitsSelectList = GetUnits() };
            return View(model);
        }

        private IEnumerable<SelectListItem> GetUnits()
        {
            // Get the enum fields if present.
            var fields = typeof(ShipmentQuantityUnits).GetFields(BindingFlags.Public | BindingFlags.Static);
            var fieldNames = new Dictionary<string, int>();

            foreach (var field in fields)
            {
                // Get the display attributes for the enum.
                var displayAttribute = (DisplayAttribute)field.GetCustomAttributes(typeof(DisplayAttribute)).SingleOrDefault();

                // Set field name to either the enum name or the display name.
                var name = (displayAttribute == null) ? field.Name : displayAttribute.Name;

                fieldNames.Add(name, (int)field.GetValue(ShipmentQuantityUnits.Tonnes));
            }

            return new SelectList(fieldNames, "Value", "Key");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Info(ShipmentInfoViewModel model)
        {
            var startDate = new DateTime(model.StartYear, model.StartMonth, model.StartDay);
            var endDate = new DateTime(model.EndYear, model.EndMonth, model.EndDay);
            if (endDate <= startDate)
            {
                ModelState.AddModelError(string.Empty, "The start date must be before the end date");
                model.UnitsSelectList = GetUnits();
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    await client.SendAsync(User.GetAccessToken(), 
                        new CreateNumberOfShipmentsInfo(
                                model.NotificationId,
                                model.NumberOfShipments,
                                model.Quantity,
                                model.Units,
                                startDate,
                                endDate));

                    return RedirectToAction("Home", "Applicant");
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);

                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }
                model.UnitsSelectList = GetUnits();
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult PackagingTypes(Guid id)
        {
            var packagingTypes = CheckBoxCollectionViewModel.CreateFromEnum<PackagingType>();

            //We need to exclude 'other' as this will be handled separately
            packagingTypes.PossibleValues = packagingTypes.PossibleValues.Where(p => (PackagingType)Convert.ToInt32(p.Value) != PackagingType.Other).ToList();

            var model = new PackagingTypesViewModel
            {
                PackagingTypes = packagingTypes,
                NotificationId = id
            };

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
                        new SetPackagingTypeOnShipmentInfo(selectedPackagingTypes, model.NotificationId, model.OtherDescription));

                    return RedirectToAction("SpecialHandling", "NotificationApplication", new { id = model.NotificationId });
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
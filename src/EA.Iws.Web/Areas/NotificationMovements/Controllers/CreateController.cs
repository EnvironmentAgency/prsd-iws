﻿namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using Core.Carriers;
    using Core.Movement;
    using Core.PackagingType;
    using Core.Rules;
    using Core.Shared;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Carriers;
    using Requests.Movement;
    using Requests.Notification;
    using Requests.NotificationMovements;
    using Requests.NotificationMovements.Create;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.Create;
    using ViewModels.Summary;

    [AuthorizeActivity(typeof(CreateMovements))]
    public class CreateController : Controller
    {
        private readonly IMediator mediator;

        public CreateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid notificationId, bool overrideRule = false)
        {
            ViewBag.NotificationId = notificationId;

            var ruleSummary = await mediator.SendAsync(new GetMovementRulesSummary(notificationId));

            if (!ruleSummary.IsSuccess && !overrideRule)
            {
                return GetRuleErrorView(ruleSummary);
            }

            var shipmentInfo = await mediator.SendAsync(new GetShipmentInfo(notificationId));

            var model = new CreateMovementsViewModel(shipmentInfo);
            model.OverrideRule = overrideRule;
            if (TempData["TempMovement"] != null)
            {
                var tempMovement = (TempMovement)TempData["TempMovement"];
                model.NumberToCreate = tempMovement.NumberToCreate;
                model.Quantity = tempMovement.Quantity.ToString();
                model.Units = tempMovement.ShipmentQuantityUnits;
                model.PackagingTypes.SetSelectedValues(tempMovement.PackagingTypes);
            }

            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid notificationId, CreateMovementsViewModel model)
        {
            ViewBag.NotificationId = notificationId;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var proposedMovementDate =
                await mediator.SendAsync(new IsProposedMovementDateValid(notificationId, model.ShipmentDate.Value));

            if (proposedMovementDate.IsOutOfRange)
            {
                ModelState.AddModelError("Day",
                    "The actual date of shipment cannot be more than 30 calendar days in the future. Please enter a different date.");
            }

            if (proposedMovementDate.IsOutsideConsentPeriod)
            {
                ModelState.AddModelError("Day",
                    "The actual date of shipment cannot be outside of the consent validity period. Please enter a different date.");
            }

            var hasExceededTotalQuantity = await mediator.SendAsync(new HasExceededConsentedQuantity(notificationId,
                Convert.ToDecimal(model.Quantity) * model.NumberToCreate.Value, model.Units.Value));

            if (hasExceededTotalQuantity)
            {
                ModelState.AddModelError("Quantity", CreateMovementsViewModelResources.HasExceededTotalQuantity);
            }

            var remainingShipmentsData = await mediator.SendAsync(new GetRemainingShipments(notificationId, model.ShipmentDate));

            if (model.NumberToCreate > remainingShipmentsData.ShipmentsRemaining)
            {
                ModelState.AddModelError("NumberToCreate", 
                    string.Format("You cannot create {0} shipments as there are only {1} remaining", model.NumberToCreate, remainingShipmentsData.ShipmentsRemaining));
            }
            else if (model.NumberToCreate > remainingShipmentsData.ActiveLoadsPermitted)
            {
                ModelState.AddModelError("NumberToCreate",
                    string.Format(
                        "You can't generate more than {0} prenotifications on {1}, as this will exceed your permitted active loads.",
                        remainingShipmentsData.ActiveLoadsPermitted,
                        model.ShipmentDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));
            }
            else if (model.NumberToCreate > remainingShipmentsData.ActiveLoadsRemainingByDate)
            {
                ModelState.AddModelError("NumberToCreate",
                    string.Format("You already have {0} prenotifications for {1} and you are only permitted to prenotify {2} shipments. {3} shipment will exceed your limit on that date.",
                        remainingShipmentsData.ActiveLoadsPermitted - remainingShipmentsData.ActiveLoadsRemainingByDate,
                        model.ShipmentDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        remainingShipmentsData.ActiveLoadsPermitted,
                        model.NumberToCreate));
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var workingDaysUntilShipment =
                await
                    mediator.SendAsync(new GetWorkingDaysUntil(notificationId, model.ShipmentDate.GetValueOrDefault()));

            if (workingDaysUntilShipment < 4)
            {
                var tempMovement = new TempMovement(model.NumberToCreate.Value,
                    model.ShipmentDate.Value,
                    Convert.ToDecimal(model.Quantity),
                    model.Units.Value,
                    model.SelectedPackagingTypes);

                TempData["TempMovement"] = tempMovement;

                return RedirectToAction("ThreeWorkingDaysWarning", "Create", new {rejectRules = model.OverrideRule });
            }

            var newMovementIds = await mediator.SendAsync(new CreateMovements(
                notificationId,
                model.NumberToCreate.Value,
                model.ShipmentDate.Value,
                Convert.ToDecimal(model.Quantity),
                model.Units.Value,
                model.SelectedPackagingTypes));

            return RedirectToAction("WhoAreYourCarriers", newMovementIds.ToRouteValueDictionary("newMovementIds"));
        }

        private ActionResult GetRuleErrorView(MovementRulesSummary ruleSummary)
        {
            if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.TotalShipmentsReached && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("TotalMovementsReached");
            }
            if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.TotalIntendedQuantityReached && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("TotalIntendedQuantityReached");
            }
            if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.TotalIntendedQuantityExceeded && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("TotalIntendedQuantityExceeded");
            }
            if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.HasApprovedFinancialGuarantee && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("NoApprovedFinancialGuarantee");
            }
            if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.ConsentPeriodExpired && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("ConsentPeriodExpired");
            }
            if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.ConsentExpiresInFourWorkingDays && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("ConsentExpiresInFourWorkingDays");
            }
            if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.ConsentExpiresInThreeOrLessWorkingDays && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("ConsentExpiresInThreeOrLessWorkingDays");
            }
            if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.ConsentWithdrawn && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("ConsentWithdrawn");
            }
            if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.FileClosed && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("FileClosed");
            }

            throw new InvalidOperationException("Unknown rule view");
        }

        [HttpGet]
        public ActionResult ThreeWorkingDaysWarning(Guid notificationId, bool rejectRules)
        {
            var model = new ThreeWorkingDaysWarningViewModel();
            model.RejectRules = rejectRules;
            return View("ThreeWorkingDays", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ThreeWorkingDaysWarning(Guid notificationId, ThreeWorkingDaysWarningViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ThreeWorkingDays", model);
            }

            if (model.Selection == ThreeWorkingDaysSelection.ChangeDate)
            {
                return RedirectToAction("Index", new {overrideRule = model.RejectRules});
            }

            if (TempData["TempMovement"] == null)
            {
                return RedirectToAction("Index");
            }

            var tempMovement = (TempMovement)TempData["TempMovement"];

            var newMovementIds = await mediator.SendAsync(new CreateMovements(
                notificationId,
                tempMovement.NumberToCreate,
                tempMovement.ShipmentDate,
                tempMovement.Quantity,
                tempMovement.ShipmentQuantityUnits,
                tempMovement.PackagingTypes));

            TempData["TempMovement"] = null;

            return RedirectToAction("WhoAreYourCarriers", newMovementIds.ToRouteValueDictionary("newMovementIds"));
        }

        [HttpGet]
        public async Task<ActionResult> Summary(Guid notificationId, Guid[] newMovementIds)
        {
            var movements = await mediator.SendAsync(new GetMovementsByIds(notificationId, newMovementIds));
            var model = new SummaryViewModel(movements);
            return View(model);
        }

        [HttpGet]
        public ActionResult WhoAreYourCarriers(Guid notificationId, Guid[] newMovementIds)
        {
            TempData.Remove("SelectedCarriers");

            var model = new WhoAreYourCarrierViewModel();
            model.MovementIds = newMovementIds;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WhoAreYourCarriers(Guid notificationId, Guid[] newMovementIds, WhoAreYourCarrierViewModel model)
        {
            if (!model.AddCarriersLater)
            {
                ModelState.AddModelError("AddCarriersLater", "Select an option for adding carriers");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
         return RedirectToAction("Summary", model.MovementIds.ToRouteValueDictionary("newMovementIds"));
        }

        [HttpGet]
        public ActionResult TotalMovementsReached(Guid notificationId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult TotalIntendedQuantityReached(Guid notificationId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult TotalIntendedQuantityExceeded(Guid notificationId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult ConsentPeriodExpired(Guid notificationId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult ConsentExpiresInFourWorkingDays(Guid notificationId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult ConsentExpiresInThreeOrLessWorkingDays(Guid notificationId)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConsentExpiresInThreeOrLessWorkingDays(Guid notificationId, ThreeOrLessDaysViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ConsentExpiresInThreeOrLessWorkingDays", model);
            }

            if (model.Selection == ThreeOrLessWorkingDaysSelection.ContinueAnyway)
            {
                return RedirectToAction("Index", new { overrideRule = true});
            }

            if (model.Selection == ThreeOrLessWorkingDaysSelection.AbandonCreation)
            {
                return RedirectToAction("Index", "Options", new { area = "NotificationApplication", id = notificationId });
            }
            return View();
        }

        [HttpGet]
        public ActionResult ConsentWithdrawn(Guid notificationId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult FileClosed(Guid notificationId)
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> NoApprovedFinancialGuarantee(Guid notificationId)
        {
            var competentAuthority =
                await mediator.SendAsync(new GetUnitedKingdomCompetentAuthorityByNotificationId(notificationId));

            return View(competentAuthority.AsUKCompetantAuthority());
        }

        [HttpGet]
        public async Task<ActionResult> AddIntendedCarrier(Guid notificationId, Guid[] newMovementIds)
        {
            var carriers =
              await mediator.SendAsync(new GetCarriersByNotificationId(notificationId));
            var model = new CarrierViewModel();
            model.SetCarriers(carriers);
            model.MovementIds = newMovementIds;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddIntendedCarrier(Guid notificationId, Guid[] newMovementIds,
            CarrierViewModel model,
            string command, string remove, string up, string down)
        {
            if (ModelState.IsValid)
            {
                int selectedCarriersCount;
                object carriersObj;

                if (TempData.TryGetValue("SelectedCarriers", out carriersObj))
                {
                    var carriersTempData = carriersObj as List<CarrierList>;

                    if (carriersTempData != null)
                    {
                        model.SelectedCarriers = carriersTempData;
                    }
                }

                if (command != null && command == "addcarrier")
                {
                    if (model.SelectedCarrier.HasValue && model.SelectedCarriers.All(x => x.Id != model.SelectedCarrier))
                    {
                        selectedCarriersCount = model.SelectedCarriers.Count;
                        model.SelectedCarriers.Add(new CarrierList
                        {
                            Id = model.SelectedCarrier.Value,
                            Order = (selectedCarriersCount + 1),
                            OrderName = AddOrdinal((selectedCarriersCount + 1))
                        });
                    }
                }
                else if (command != null && command == "continue")
                {
                    if (model.SelectedCarriers.Any())
                    {
                        TempData.Remove("SelectedCarriers");

                        var selectedCarriers = model.SelectedCarriers.ToDictionary(carrier => carrier.Order,
                            carrier => carrier.Id);

                        await
                            mediator.SendAsync(new CreateMovementCarriers(notificationId, model.MovementIds,
                                selectedCarriers));

                        return RedirectToAction("Summary", model.MovementIds.ToRouteValueDictionary("newMovementIds"));
                    }

                    ModelState.AddModelError("SelectedCarrier", "Select a carrier from the list");
                }
                else if (remove != null)
                {
                    var indexToRemove = model.SelectedCarriers.FirstOrDefault(c => c.Id.ToString() == remove);
                    model.SelectedCarriers.RemoveAll(c => c.Id.ToString() == remove);
                    foreach (var newOrder in model.SelectedCarriers.Where(w => w.Order > indexToRemove.Order))
                    {
                        if (newOrder.Order != 1)
                        {
                            newOrder.Order = newOrder.Order - 1;
                            newOrder.OrderName = AddOrdinal(newOrder.Order);
                        }
                    }
                    ModelState.Clear();
                }
                else if (!string.IsNullOrEmpty(up) || !string.IsNullOrEmpty(down))
                {
                    model.SelectedCarriers = ReOrderCarriers(up, down, model.SelectedCarriers);
                }

                // Ensure the order label is correct.
                if (command != null && command == "addcarrier" ||
                    remove != null ||
                    !string.IsNullOrEmpty(up) ||
                    !string.IsNullOrEmpty(down))
                {
                    selectedCarriersCount = model.SelectedCarriers.Count;
                    if (selectedCarriersCount > 2)
                    {
                        foreach (var carrier in model.SelectedCarriers)
                        {
                            carrier.OrderName = carrier.Order == selectedCarriersCount
                                ? "Last"
                                : AddOrdinal(carrier.Order);
                        }
                    }
                }

                TempData["SelectedCarriers"] = model.SelectedCarriers;
            }

            var carriers = await mediator.SendAsync(new GetCarriersByNotificationId(notificationId));
            model.SetCarriers(carriers);
            return View(model);
        }

        public static string AddOrdinal(int number)
        {
            if (number <= 0)
            {
                return number.ToString();
            }

            switch (number % 100)
            {
                case 11:
                case 12:
                case 13:
                    return number + "th";
            }

            switch (number % 10)
            {
                case 1:
                    return number + "st";
                case 2:
                    return number + "nd";
                case 3:
                    return number + "rd";
                default:
                    return number + "th";
            }
        }

        private static List<CarrierList> ReOrderCarriers(string up, string down, List<CarrierList> carrierList)
        {
            if (carrierList.Any())
            {
                if (!string.IsNullOrEmpty(up))
                {
                    var selectedCarrier = carrierList.FirstOrDefault(c => c.Id.ToString() == up);

                    if (selectedCarrier != null)
                    {
                        var higherCarrier = carrierList.FirstOrDefault(c => c.Order == selectedCarrier.Order - 1);

                        if (higherCarrier != null)
                        {
                            higherCarrier.Order++;
                            higherCarrier.OrderName = AddOrdinal(higherCarrier.Order);

                            selectedCarrier.Order--;
                            selectedCarrier.OrderName = AddOrdinal(selectedCarrier.Order);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(down))
                {
                    var selectedCarrier = carrierList.FirstOrDefault(c => c.Id.ToString() == down);

                    if (selectedCarrier != null)
                    {
                        var lowerCarrier = carrierList.FirstOrDefault(c => c.Order == selectedCarrier.Order + 1);

                        if (lowerCarrier != null)
                        {
                            lowerCarrier.Order--;
                            lowerCarrier.OrderName = AddOrdinal(lowerCarrier.Order);

                            selectedCarrier.Order++;
                            selectedCarrier.OrderName = AddOrdinal(selectedCarrier.Order);
                        }
                    }
                }
            }

            return carrierList.OrderBy(c => c.Order).ToList();
        }

        [Serializable]
        private class TempMovement
        {
            public TempMovement(int numberToCreate, DateTime shipmentDate, decimal quantity,
                ShipmentQuantityUnits shipmentQuantityUnits, IList<PackagingType> packagingTypes)
            {
                NumberToCreate = numberToCreate;
                ShipmentDate = shipmentDate;
                Quantity = quantity;
                ShipmentQuantityUnits = shipmentQuantityUnits;
                PackagingTypes = packagingTypes;
            }

            public int NumberToCreate { get; set; }

            public DateTime ShipmentDate { get; set; }

            public decimal Quantity { get; set; }

            public ShipmentQuantityUnits ShipmentQuantityUnits { get; set; }

            public IList<PackagingType> PackagingTypes { get; set; }
        }
    }
}
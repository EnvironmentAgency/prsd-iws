﻿namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Movement;
    using Core.PackagingType;
    using Core.Rules;
    using Core.Shared;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Notification;
    using Requests.NotificationMovements;
    using Requests.NotificationMovements.Create;
    using ViewModels.Create;

    [AuthorizeActivity(typeof(CreateMovements))]
    public class CreateController : Controller
    {
        private readonly IMediator mediator;

        public CreateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid notificationId)
        {
            var ruleSummary = await mediator.SendAsync(new GetMovementRulesSummary(notificationId));

            if (!ruleSummary.IsSuccess)
            {
                return GetRuleErrorView(ruleSummary);
            }

            var shipmentInfo = await mediator.SendAsync(new GetShipmentInfo(notificationId));

            var model = new CreateMovementsViewModel(shipmentInfo);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid notificationId, CreateMovementsViewModel model)
        {
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

            var remainingShipmentsData = await mediator.SendAsync(new GetRemainingShipments(notificationId));

            if (model.NumberToCreate > remainingShipmentsData.ShipmentsRemaining)
            {
                ModelState.AddModelError("NumberToCreate", 
                    string.Format("You cannot create {0} shipments as there are only {1} remaining", model.NumberToCreate, remainingShipmentsData.ShipmentsRemaining));
            }
            else if (model.NumberToCreate > remainingShipmentsData.ActiveLoadsRemaining)
            {
                ModelState.AddModelError("NumberToCreate",
                    string.Format("You cannot create {0} shipments as there are only {1} active loads remaining", model.NumberToCreate, remainingShipmentsData.ActiveLoadsRemaining));
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

                return RedirectToAction("ThreeWorkingDaysWarning", "Create");
            }

            var newMovementIds = await mediator.SendAsync(new CreateMovements(
                notificationId,
                model.NumberToCreate.Value,
                model.ShipmentDate.Value,
                Convert.ToDecimal(model.Quantity),
                model.Units.Value,
                model.SelectedPackagingTypes));

            return RedirectToAction("Summary", newMovementIds.ToRouteValueDictionary("newMovementIds"));
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
            if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.ActiveLoadsReached && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("TotalActiveLoadsReached");
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
        public ActionResult ThreeWorkingDaysWarning(Guid notificationId)
        {
            var model = new ThreeWorkingDaysWarningViewModel();

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
                return RedirectToAction("Index");
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

            return RedirectToAction("Summary", newMovementIds.ToRouteValueDictionary("newMovementIds"));
        }

        [HttpGet]
        public async Task<ActionResult> Summary(Guid notificationId, Guid[] newMovementIds)
        {
            var movements = await mediator.SendAsync(new GetMovementsByIds(notificationId, newMovementIds));
            return View(movements);
        }

        [HttpGet]
        public ActionResult TotalMovementsReached(Guid notificationId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult TotalActiveLoadsReached(Guid notificationId)
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
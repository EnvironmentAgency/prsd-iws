﻿namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.MovementReceipt;
    using Infrastructure;
    using Requests.MovementReceipt;
    using ViewModels.ShipmentAcceptance;

    [Authorize]
    public class ShipmentAcceptanceController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public ShipmentAcceptanceController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            using (var client = apiClient())
            {
                var acceptanceInfo = await client.SendAsync(User.GetAccessToken(), new GetMovementAcceptanceDataByMovementId(id));

                var model = new ShipmentAcceptanceViewModel(acceptanceInfo);

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, ShipmentAcceptanceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                await
                    client.SendAsync(User.GetAccessToken(),
                        new UpdateShipmentAcceptanceDataByMovementId(id, model.Decision.GetValueOrDefault(), model.RejectReason));

                if (model.Decision == Decision.Rejected)
                {
                    // Change to redirect to completed page when it is available
                    return RedirectToAction("Index", "ReceiptComplete", new { id });
                }

                return RedirectToAction("Index", "QuantityReceived", new { id });
            }
        }
    }
}
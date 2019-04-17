﻿namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement;
    using Requests.ImportNotification;
    using ViewModels.ShipmentAudit;
    using Web.ViewModels;

    [AuthorizeActivity(typeof(GetImportMovementAuditByNotificationId))]
    public class ShipmentAuditController : Controller
    {
        private readonly IMediator mediator;

        public ShipmentAuditController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, ShipmentAuditFilterType? filter, int? number = null, int page = 1)
        {
            var response = await mediator.SendAsync(new GetImportMovementAuditByNotificationId(id, page));
            var model = new ShipmentAuditViewModel(response)
            {
                NotificationId = id,
                SelectedFilter = filter,
                NotificationNumber = await mediator.SendAsync(new GetImportNotificationNumberById(id))
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, ShipmentAuditViewModel model)
        {
            if (model.SelectedFilter == null)
            {
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                var selectedFilter = model.SelectedFilter;
                var response = await mediator.SendAsync(new GetImportMovementAuditByNotificationId(id, 1));
                model = new ShipmentAuditViewModel(response)
                {
                    NotificationId = id,
                    SelectedFilter = selectedFilter,
                    NotificationNumber = await mediator.SendAsync(new GetImportNotificationNumberById(id))
                };
                return View(model);
            }

            return RedirectToAction("Index", new { id = id, filter = model.SelectedFilter, number = model.ShipmentNumber, page = 1 });
        }
    }
}
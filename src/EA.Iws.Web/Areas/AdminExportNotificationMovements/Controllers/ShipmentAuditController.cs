﻿namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Notification;
    using ViewModels.ShipmentAudit;

    [AuthorizeActivity(typeof(GetMovementAuditByNotificationId))]
    public class ShipmentAuditController : Controller
    {
        private readonly IMediator mediator;

        public ShipmentAuditController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, int page = 1)
        {
            var response = await mediator.SendAsync(new GetMovementAuditByNotificationId(id, page));
            var model = new ShipmentAuditViewModel(response);
            model.NotificationId = id;
            model.NotificationNumber = await mediator.SendAsync(new GetNotificationNumber(id));
            return View(model);
        }
    }
}
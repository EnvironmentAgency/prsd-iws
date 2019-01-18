﻿namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using ViewModels.PrenotificationBulkUpload;

    public class PrenotificationBulkUploadController : Controller
    {
        private readonly IMediator mediator;

        public PrenotificationBulkUploadController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult UploadPrenotifications(Guid notificationId)
        {
            var model = new PrenotificationBulkUploadViewModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadPrenotifications(Guid notificationId, PrenotificationBulkUploadViewModel model2)
        {
            PrenotificationBulkUploadViewModel model = new PrenotificationBulkUploadViewModel();

            if (!ModelState.IsValid)
            {
                ViewBag.NotificationId = notificationId;
                model = new PrenotificationBulkUploadViewModel();
                return View(model);
            }

            return View(model);
        }
    }
}
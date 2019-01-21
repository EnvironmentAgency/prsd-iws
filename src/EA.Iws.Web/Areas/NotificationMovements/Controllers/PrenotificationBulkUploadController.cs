﻿namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using ViewModels.PrenotificationBulkUpload;

    [Authorize]
    public class PrenotificationBulkUploadController : Controller
    {
        private readonly IMediator mediator;

        public PrenotificationBulkUploadController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index(Guid notificationId)
        {
            ViewBag.NotificationId = notificationId;
            var model = new PrenotificationBulkUploadViewModel(notificationId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid notificationId, PrenotificationBulkUploadViewModel model)
        {
            return RedirectToAction("UploadPrenotifications");
        }

        [HttpGet]
        public ActionResult UploadPrenotifications(Guid notificationId)
        {
            var model = new PrenotificationBulkUploadViewModel(notificationId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadPrenotifications(Guid notificationId, PrenotificationBulkUploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.NotificationId = notificationId;
                model = new PrenotificationBulkUploadViewModel(notificationId);
                return View(model);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Warning()
        {
            var model = new WarningChoiceViewModel();

            return View(model);
        }
    }
}
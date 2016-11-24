﻿namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Web.Mvc;
    using Core.Authorization.Permissions;
    using Infrastructure.Authorization;
    using ViewModels.NumberOfShipments;

    [AuthorizeActivity(ExportNotificationPermissions.CanChangeNumberOfShipmentsOnExportNotification)]
    public class NumberOfShipmentsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid id, IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Confirm");
        }

        [HttpGet]
        public ActionResult Confirm()
        {
            return View();
        }
    }
}
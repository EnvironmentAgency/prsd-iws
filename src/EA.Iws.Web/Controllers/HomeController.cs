﻿namespace EA.Iws.Web.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(actionName: "Home", controllerName: "Applicant");
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult LandingPage()
        {
           return View();
        }
    }
}
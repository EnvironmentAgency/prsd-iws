﻿namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.Registration;

    public class RegistrationController : Controller
    {
        public ActionResult ApplicantRegistration()
        {
            return View(new ApplicantRegistrationViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitApplicantRegistration(ApplicantRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ApplicantRegistration", model);
            }

            return View("ApplicantRegistration", model);
        }

        [HttpGet]
        public ActionResult OrganisationGrid(IList<OrganisationViewModel> model)
        {
            return PartialView("_OrganisationGrid", model);
        }

        [HttpGet]
        public ActionResult SelectOrganisation(string name)
        {
            var model = new SelectOrganisationViewModel
            {
                Name = name,
                Organisations = GetTestOrganisations()
            };

            return View("SelectOrganisation", model);
        }

        [HttpPost]
        public ActionResult SelectOrganisation(SelectOrganisationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("SelectOrganisation", model);
            }

            return RedirectToAction("SelectOrganisation");
        }

        private IList<OrganisationViewModel> GetTestOrganisations()
        {
            return new[]
            {
                new OrganisationViewModel
                {
                    Id = Guid.NewGuid(),
                    Postcode = "GU22 7mx",
                    TownOrCity = "Woking",
                    Name = "SFW Ltd"
                },
                new OrganisationViewModel
                {
                    Id = Guid.NewGuid(),
                    Postcode = "GU22 7UY",
                    TownOrCity = "Woking",
                    Name = "SWF Plumbing"
                }
            };
        }
    }
}
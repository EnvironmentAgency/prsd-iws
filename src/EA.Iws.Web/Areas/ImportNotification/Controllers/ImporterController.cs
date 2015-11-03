﻿namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotification.Draft;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Requests.Shared;
    using ViewModels.Importer;

    [Authorize(Roles = "internal")]
    public class ImporterController : Controller
    {
        private readonly IMediator mediator;

        public ImporterController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var importer = await mediator.SendAsync(new GetDraftData<Importer>(id));

            var model = new ImporterViewModel(importer);

            var countries = await mediator.SendAsync(new GetCountries());

            model.Address.Countries = countries;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, ImporterViewModel model)
        {
            var importer = new Importer
            {
                Address = model.Address.AsAddress(),
                BusinessName = model.BusinessName,
                Type = model.Type,
                RegistrationNumber = model.RegistrationNumber,
                Contact = model.Contact.AsContact()
            };

            await mediator.SendAsync(new SetDraftData<Importer>(id, importer));

            return RedirectToAction("Index", "Home", new { area = "Admin" });
        } 
    }
}
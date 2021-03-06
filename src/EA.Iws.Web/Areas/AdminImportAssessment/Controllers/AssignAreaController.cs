﻿namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;
    using Requests.NotificationAssessment;
    using ViewModels.AssignArea;

    [AuthorizeActivity(typeof(SetImportNotificationConsultation))]
    public class AssignAreaController : Controller
    {
        private readonly IMediator mediator;

        public AssignAreaController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var consultation = await mediator.SendAsync(new GetImportNotificationConsultation(id));

            var model = new AssignAreaViewModel
            {
                NotificationId = id,
                Areas = await GetAreas(),
                LocalAreaId = consultation.LocalAreaId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(AssignAreaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Areas = await GetAreas();

                return View(model);
            }

            await mediator.SendAsync(new SetImportNotificationConsultation(
                model.NotificationId, 
                model.LocalAreaId.GetValueOrDefault()));

            return RedirectToAction("Index", "KeyDates", new { id = model.NotificationId, area = "AdminImportAssessment" });
        }

        private async Task<SelectList> GetAreas()
        {
            var areas = await mediator.SendAsync(new GetLocalAreasByUserCa());

            return new SelectList(areas.Select(area => new SelectListItem { Text = area.Name, Value = area.Id.ToString() }), "Value", "Text");
        }
    }
}
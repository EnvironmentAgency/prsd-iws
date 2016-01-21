﻿namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;
    using ViewModels.Decision;

    [Authorize(Roles = "internal")]
    public class DecisionController : Controller
    {
        private readonly IMediator mediator;

        public DecisionController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var data = await mediator.SendAsync(new GetImportNotificationAssessmentDecisionData(id));

            return View(new DecisionViewModel(data));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, DecisionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            switch (model.Decision)
            {
                case DecisionType.Consent:
                    await PostConsent(id, model);
                    break;
                case DecisionType.ConsentWithdraw:
                    await PostConsentWithdrawn(id, model);
                    break;
                case DecisionType.Object:
                    await PostObjection(id, model);
                    break;
                default:
                    break;
            }

            return RedirectToAction("Index", "KeyDates");
        }

        private async Task PostConsent(Guid id, DecisionViewModel model)
        {
            await mediator.SendAsync(new Consent(id,
                model.ConsentValidFromDate.AsDateTime().Value,
                model.ConsentValidToDate.AsDateTime().Value,
                model.ConsentConditions,
                model.ConsentGivenDate.AsDateTime().Value));
        }

        private async Task PostConsentWithdrawn(Guid id, DecisionViewModel model)
        {
            await mediator.SendAsync(new WithdrawConsentForImportNotification(id, model.ReasonsForConsentWithdrawal));
        }

        private async Task PostObjection(Guid id, DecisionViewModel model)
        {
            var date = new DateTime(model.ObjectionDate.Year.GetValueOrDefault(), model.ObjectionDate.Month.GetValueOrDefault(), model.ObjectionDate.Day.GetValueOrDefault());

            var request = new ObjectToImportNotification(id, model.ReasonForObjection, date);
            await mediator.SendAsync(request);
        }
    }
}
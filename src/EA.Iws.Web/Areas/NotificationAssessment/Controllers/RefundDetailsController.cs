﻿namespace EA.Iws.Web.Areas.NotificationAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using ViewModels.RefundDetails;

    [Authorize(Roles = "internal")]
    public class RefundDetailsController : Controller
    {
        private readonly IMediator mediator;

        public RefundDetailsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            var model = new RefundDetailsViewModel { NotificationId = id };

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Index(RefundDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var refundData = new NotificationTransactionData
            {
                Date = model.Date(),
                NotificationId = model.NotificationId,
                Debit = Convert.ToDecimal(model.Amount),
                Comments = model.Comments
            };

            await mediator.SendAsync(new AddNotificationTransaction(refundData));

            return RedirectToAction("index", "Home", new { id = model.NotificationId });
        }
    }
}
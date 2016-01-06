﻿namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Notification;
    using Infrastructure;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Notification;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class WhatToDoNextController : Controller
    {
        private readonly IMediator mediator;

        public WhatToDoNextController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            var model = new WhatToDoNextData { Id = id };
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Print(Guid id)
        {
            var response = await mediator.SendAsync(new GetWhatToDoNextDataForNotification(id));

            return View(response);
        }

        [HttpGet]
        public async Task<ActionResult> PostageLabel(Guid id)
        {
            var response = await mediator.SendAsync(new GetWhatToDoNextDataForNotification(id));

            return View(response);
        }

        [HttpGet]
        public async Task<ActionResult> FinancialGuarantee(Guid id)
        {
            var response = await mediator.SendAsync(new GetWhatToDoNextDataForNotification(id));
            
            return View(response);
        }

        [HttpGet]
        public async Task<ActionResult> Payment(Guid id)
        {
            var response = await mediator.SendAsync(new GetWhatToDoNextPaymentDataForNotification(id));

            return View(response);
        }

        [HttpGet]
        public ActionResult CardPaymentInformation(CompetentAuthority competentAuthority)
        {
            switch (competentAuthority)
            {
                case CompetentAuthority.England:
                    return PartialView("_EaCard");
                case CompetentAuthority.Scotland:
                    return PartialView("_SepaCard");
                case CompetentAuthority.Wales:
                    return PartialView("_NrwCard");
                default:
                    return new EmptyResult();
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetPostageLabel(CompetentAuthority competentAuthority)
        {
            try
            {
                var response = await mediator.SendAsync(new GeneratePostageLabel(competentAuthority));

                var downloadName = "CompetentAuthorityPostageLabel" + SystemTime.UtcNow + ".pdf";

                return File(response, "application/pdf", downloadName);
            }
            catch (ApiBadRequestException ex)
            {
                this.HandleBadRequest(ex);
                if (ModelState.IsValid)
                {
                    throw;
                }
                return HttpNotFound();
            }
        }
    }
}
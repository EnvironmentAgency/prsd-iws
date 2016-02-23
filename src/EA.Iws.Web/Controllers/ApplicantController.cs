﻿namespace EA.Iws.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    [AuthorizeActivity(typeof(GetExportNotificationsByUser))]
    public class ApplicantController : Controller
    {
        private readonly IMediator mediator;

        public ApplicantController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Home()
        {
            var response = (await mediator.SendAsync(new GetExportNotificationsByUser())).ToList();

            return View(response);
        }
    }
}
﻿namespace EA.Iws.Web.Areas.Reports.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin.Reports;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.Admin.Reports;
    using ViewModels.ImportNotifications;

    [Authorize(Roles = "internal")]
    public class ImportNotificationsController : Controller
    {
        private readonly IMediator mediator;

        public ImportNotificationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new IndexViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var from = model.From.AsDateTime().Value;
            var to = model.To.AsDateTime().Value;

            var report = await mediator.SendAsync(new GetImportNotificationsReport(from, to));

            var fileName = string.Format("BDU-Import-notifications-{0}-{1}.csv", from.ToShortDateString(), to.ToShortDateString());

            return new CsvActionResult<DataImportNotificationData>(report.ToList(), fileName);
        }
    }
}
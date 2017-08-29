﻿namespace EA.Iws.Web.Areas.Reports.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin.Reports;
    using Core.WasteType;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Admin.Reports;
    using ViewModels.Shipments;

    [AuthorizeActivity(typeof(GetShipmentsReport))]
    public class ShipmentsController : Controller
    {
        private readonly IMediator mediator;

        public ShipmentsController(IMediator mediator)
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

            var from = model.From.AsDateTime().GetValueOrDefault();
            var to = model.To.AsDateTime().GetValueOrDefault();
            var chemicalComposition = model.ChemicalComposition;

            if (chemicalComposition == default(ChemicalComposition))
            {
                chemicalComposition = null;
            }

            var report = await mediator.SendAsync(new GetShipmentsReport(from, to, model.DateType, chemicalComposition));

            var fileName = string.Format("shipments-{0}-{1}.xlsx", from.ToShortDateString(), to.ToShortDateString());

            return new XlsxActionResult<ShipmentData>(report, fileName);
        }
    }
}
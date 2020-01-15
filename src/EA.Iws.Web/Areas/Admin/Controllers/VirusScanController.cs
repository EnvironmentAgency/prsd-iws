﻿namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure;
    using Scanning;
    using ViewModels.VirusScan;

    [Authorize(Roles = "internal,readonly")]
    public class VirusScanController : Controller
    {
        private readonly IWriteFileVirusWrapper virusScanner;

        public VirusScanController(IWriteFileVirusWrapper virusScanner)
        {
            this.virusScanner = virusScanner;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(VirusScanViewModel model)
        {
            var timeStart = DateTime.Now;

            var fileContents = new MemoryStream();
            model.File.InputStream.CopyTo(fileContents);
            
            var result = await virusScanner.ScanFile(fileContents.ToArray(), User.GetAccessToken());

            ViewBag.Message = string.Format("Started: {0} Ended: {1} Result: {2}", timeStart, DateTime.UtcNow, result.ToString());

            return View();
        }
    }
}

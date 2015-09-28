﻿namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.RecoveryInfo;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Notification;
    using Requests.RecoveryInfo;
    using ViewModels.RecoveryInfo;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class RecoveryInfoController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public RecoveryInfoController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> RecoveryPercentage(Guid id, bool? backToOverview = null)
        {
            using (var client = apiClient())
            {
                var recoveryPercentageData = await client.SendAsync(User.GetAccessToken(), new GetRecoveryPercentageData(id));

                var model = new RecoveryPercentageViewModel(recoveryPercentageData);

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RecoveryPercentage(RecoveryPercentageViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    await client.SendAsync(User.GetAccessToken(), model.ToRequest());

                    return GetNextScreenForRecoveryPercentageView(model, backToOverview);
                }
                catch (ApiBadRequestException e)
                {
                    this.HandleBadRequest(e);
                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }

                return View(model);
            }
        }

        private ActionResult GetNextScreenForRecoveryPercentageView(RecoveryPercentageViewModel model, bool? backToOverview)
        {
            if (model.IsProvidedByImporter)
            {
                return RedirectToAction("Index", "Home", new { id = model.NotificationId });
            }

            return model.PercentageRecoverable.GetValueOrDefault() == 100 ? 
                RedirectToAction("RecoveryValues", "RecoveryInfo", new { isDisposal = false, backToOverview }) 
                : RedirectToAction("MethodOfDisposal", "RecoveryInfo", new { id = model.NotificationId, backToOverview });
        }

        [HttpGet]
        public async Task<ActionResult> MethodOfDisposal(Guid id, bool? backToOverview = null)
        {
            using (var client = apiClient())
            {
                var recoveryPercentageData = await client.SendAsync(User.GetAccessToken(), new GetRecoveryPercentageData(id));
                var model = new MethodOfDisposalViewModel(recoveryPercentageData);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MethodOfDisposal(MethodOfDisposalViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    await client.SendAsync(User.GetAccessToken(), model.ToRequest());
                    return RedirectToAction("RecoveryValues", "RecoveryInfo", new { isDisposal = true, backToOverview });
                }
                catch (ApiBadRequestException e)
                {
                    this.HandleBadRequest(e);
                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }

                return View(model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> RecoveryValues(Guid id, bool isDisposal, bool? backToOverview = null)
        {
            using (var client = apiClient())
            {
                var recoveryValuesData = await client.SendAsync(User.GetAccessToken(), new GetRecoveryValuesData(id)) ?? new RecoveryInfoData();

                var model = new RecoveryInfoValuesViewModel(recoveryValuesData);

                model.NotificationId = id;
                model.IsDisposal = isDisposal;

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RecoveryValues(RecoveryInfoValuesViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View("RecoveryValues", model);
            }

            using (var client = apiClient())
            {
                try
                {
                    if (!model.IsDisposal)
                    {
                        model.DisposalAmount = null;
                        model.DisposalUnit = null;
                    }

                    await client.SendAsync(User.GetAccessToken(), model.ToRequest());
                    return RedirectToAction("Index", "Home");
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);
                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }
                return View("RecoveryValues", model);
            }
        }
    }
}
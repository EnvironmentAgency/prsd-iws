﻿namespace EA.Iws.Web.Areas.NotificationAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Requests.Admin.NotificationAssessment;
    using ViewModels;

    [Authorize(Roles = "internal")]
    public class KeyDatesController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public KeyDatesController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            using (var client = apiClient())
            {
                var dates = await client.SendAsync(User.GetAccessToken(), new GetDates(id));
                return View(new DateInputViewModel(dates));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(DateInputViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Command == DateInputViewModel.NotificationReceived)
            {
                await SetNotificationReceived(model);
            }
            else if (model.Command == DateInputViewModel.PaymentReceived)
            {
                await SetPaymentReceived(model);
            }
            else if (model.Command == DateInputViewModel.AssessmentCommenced)
            {
                await SetAssessmentCommenced(model);
            }
            else
            {
                var setDates = new SetDates
                {
                    CompleteDate = model.NotificationCompleteDate.AsDateTime(),
                    TransmittedDate = model.NotificationTransmittedDate.AsDateTime(),
                    AcknowledgedDate = model.NotificationAcknowledgedDate.AsDateTime(),
                    DecisionDate = model.DecisionDate.AsDateTime(),
                    NotificationApplicationId = model.NotificationId
                };

                using (var client = apiClient())
                {
                    await client.SendAsync(User.GetAccessToken(), setDates);
                }
            }

            return RedirectToAction("Index", new { id = model.NotificationId });
        }

        private async Task SetPaymentReceived(DateInputViewModel model)
        {
            var setPaymentReceivedDate = new SetPaymentReceivedDate(model.NotificationId,
                model.PaymentReceivedDate.AsDateTime().GetValueOrDefault());

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), setPaymentReceivedDate);
            }
        }

        private async Task SetNotificationReceived(DateInputViewModel model)
        {
            var setNotificationReceivedDate = new SetNotificationReceivedDate(model.NotificationId,
                model.NotificationReceivedDate.AsDateTime().GetValueOrDefault());

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), setNotificationReceivedDate);
            }
        }

        private async Task SetAssessmentCommenced(DateInputViewModel model)
        {
            var setAssessmentCommenced = new SetCommencedDate(model.NotificationId, 
                model.CommencementDate.AsDateTime().GetValueOrDefault(), model.NameOfOfficer);

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), setAssessmentCommenced);
            }
        }
    }
}
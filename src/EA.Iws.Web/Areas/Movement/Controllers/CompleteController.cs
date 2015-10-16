﻿namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Movement.Complete;
    using Requests.Notification;
    using ViewModels.Complete;

    public class CompleteController : Controller
    {
        private readonly IMediator mediator;
        private const string DateKey = "Date";

        public CompleteController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Date(Guid id)
        {
            var notificationId = await mediator.SendAsync(new GetNotificationIdByMovementId(id));
            var data = await mediator.SendAsync(new GetNotificationBasicInfo(notificationId));
            var model = new DateViewModel(data.NotificationType);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Date(Guid id, DateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData[DateKey] = model.GetDateComplete();

            return RedirectToAction("Upload", "Complete");
        }

        [HttpGet]
        public async Task<ActionResult> Upload(Guid id)
        {
            object value;
            if (TempData.TryGetValue(DateKey, out value))
            {
                var completedDate = (DateTime)value;
                var notificationId = await mediator.SendAsync(new GetNotificationIdByMovementId(id));
                var notification = await mediator.SendAsync(new GetNotificationBasicInfo(notificationId));

                var model = new UploadViewModel
                {
                    NotificationId = notificationId,
                    NotificationType = notification.NotificationType,
                    CompletedDate = completedDate
                };

                return View(model);
            }

            return RedirectToAction("Date", "Complete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(Guid id, UploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var fileExtension = Path.GetExtension(model.File.FileName);
            var uploadedFile = new byte[model.File.InputStream.Length];
            model.File.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

            await mediator.SendAsync(new SaveMovementCompletedReceipt(id, model.CompletedDate, uploadedFile, fileExtension));

            return RedirectToAction("Success", "Complete");
        }

        [HttpGet]
        public async Task<ActionResult> Success(Guid id)
        {
            var notificationId = await mediator.SendAsync(new GetNotificationIdByMovementId(id));
            var notification = await mediator.SendAsync(new GetNotificationBasicInfo(notificationId));
            var model = new SuccessViewModel
            {
                NotificationId = id,
                NotificationType = notification.NotificationType
            };

            return View(model);
        }
    }
}
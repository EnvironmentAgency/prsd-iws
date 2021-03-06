﻿namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.CustomsOffice;
    using EA.Iws.Core.Notification;
    using EA.Iws.Requests.Notification;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class CustomsOfficeController : Controller
    {
        private readonly IMediator mediator;
        private readonly Func<Guid, bool?, RedirectToRouteResult> noCustomsOffice;
        private readonly Func<Guid, bool?, RedirectToRouteResult> transportRouteSummary;
        private readonly Func<Guid, bool?, RedirectToRouteResult> addExitCustomsOffice;
        private readonly Func<Guid, bool?, RedirectToRouteResult> addEntryCustomsOffice;

        public CustomsOfficeController(IMediator mediator)
        {
            this.mediator = mediator;

            noCustomsOffice = (id, backToOverview) => RedirectToAction("NoCustomsOffice", "CustomsOffice", new { id, backToOverview });
            transportRouteSummary = (id, backToOverview) => RedirectToAction("Summary", "TransportRoute", new { id, backToOverview });
            addExitCustomsOffice = (id, backToOverview) => RedirectToAction("Index", "ExitCustomsOffice", new { id, backToOverview });
            addEntryCustomsOffice = (id, backToOverview) => RedirectToAction("Index", "EntryCustomsOffice", new { id, backToOverview });
        }

        public async Task<ActionResult> Index(Guid id, bool? backToOverview = null)
        {
            var customsOffice = await mediator.SendAsync(new GetCustomsCompletionStatusByNotificationId(id));
            var notificationCompetentAutority = await mediator.SendAsync(new GetNotificationCompetentAuthority(id));

            if (notificationCompetentAutority.Equals(UKCompetentAuthority.NorthernIreland))
            {
                switch (customsOffice.CustomsOfficesRequired)
                {
                    case CustomsOffices.None:
                        return noCustomsOffice(id, backToOverview);

                    case CustomsOffices.EntryAndExit:
                    case CustomsOffices.Exit:
                        return addExitCustomsOffice(id, backToOverview);

                    case CustomsOffices.Entry:
                        return addEntryCustomsOffice(id, backToOverview);                    

                    default:
                        return transportRouteSummary(id, backToOverview);
                }
            }
            else
            {
                switch (customsOffice.CustomsOfficesRequired)
                {
                    case CustomsOffices.None:
                        return noCustomsOffice(id, backToOverview);

                    case CustomsOffices.EntryAndExit:
                    case CustomsOffices.Entry:
                        return addEntryCustomsOffice(id, backToOverview);

                    case CustomsOffices.Exit:
                        return addExitCustomsOffice(id, backToOverview);

                    default:
                        return transportRouteSummary(id, backToOverview);
                }
            }            
        }

        public ActionResult NoCustomsOffice(Guid id, bool? backToOverview = null)
        {
            return View();
        }
    }
}
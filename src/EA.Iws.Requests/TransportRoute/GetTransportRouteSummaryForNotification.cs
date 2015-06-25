﻿namespace EA.Iws.Requests.TransportRoute
{
    using System;
    using Core.TransportRoute;
    using Prsd.Core.Mediator;

    public class GetTransportRouteSummaryForNotification : IRequest<TransportRouteData>
    {
        public Guid NotificationId { get; private set; }

        public GetTransportRouteSummaryForNotification(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}

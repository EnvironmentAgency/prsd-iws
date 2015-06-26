﻿namespace EA.Iws.Requests.StateOfExport
{
    using System;
    using Prsd.Core.Mediator;

    public class GetStateOfExportWithTransportRouteDataByNotificationId : IRequest<StateOfExportWithTransportRouteData>
    {
        public Guid Id { get; private set; }

        public GetStateOfExportWithTransportRouteDataByNotificationId(Guid id)
        {
            Id = id;
        }
    }
}

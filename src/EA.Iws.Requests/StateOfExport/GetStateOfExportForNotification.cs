﻿namespace EA.Iws.Requests.StateOfExport
{
    using System;
    using Prsd.Core.Mediator;

    public class GetStateOfExportForNotification : IRequest<StateOfExportData>
    {
        public Guid NotificationId { get; private set; }

        public GetStateOfExportForNotification(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}

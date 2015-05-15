﻿namespace EA.Iws.Requests.Notification
{
    using System;
    using System.Collections.Generic;
    using Prsd.Core.Mediator;

    public class GetProducersByNotificationId : IRequest<IList<ProducerData>>
    {
        public Guid NotificationId { get; set; }

        public GetProducersByNotificationId(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
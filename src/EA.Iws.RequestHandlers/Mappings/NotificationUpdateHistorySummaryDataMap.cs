﻿namespace EA.Iws.RequestHandlers.Mappings
{
    using Domain;
    using Prsd.Core.Mapper;
    using Requests.Notification;

    internal class NotificationUpdateHistorySummaryDataMap : IMap<NotificationUpdateHistorySummaryData, NotificationUpdateHistory>
    {
        public NotificationUpdateHistorySummaryDataMap(INotificationUserRepository notificationUserRepository)
        {
        }

        public NotificationUpdateHistory Map(NotificationUpdateHistorySummaryData source)
        {
            return new NotificationUpdateHistory
            {
                Id = source.Id,
                UserName = source.Name,
                DateAdded = source.DateAdded,
                InformationChange = source.InformationChange,
                TypeOfChange = source.TypeOfChange
            };
        }
    }
}
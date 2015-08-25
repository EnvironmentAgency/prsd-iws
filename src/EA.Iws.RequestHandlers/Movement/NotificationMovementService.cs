﻿namespace EA.Iws.RequestHandlers.Movement
{
    using System;
    using System.Linq;
    using Core.NotificationAssessment;
    using DataAccess;
    using Domain;

    internal class NotificationMovementService : INotificationMovementService
    {
        private readonly IwsContext context;

        public NotificationMovementService(IwsContext context)
        {
            this.context = context;
        }

        public bool CanCreateNewMovementForNotification(Guid notificationId)
        {
            var notification = context.NotificationApplications.Single(na => na.Id == notificationId);

            var notificationAssessment =
                context.NotificationAssessments.SingleOrDefault(na => na.NotificationApplicationId == notificationId);

            var currentMovements = context.Movements.Count(m => m.NotificationApplicationId == notificationId);

            var isSubmitted = false;
            if (notificationAssessment != null)
            {
                isSubmitted = notificationAssessment.Status != NotificationStatus.NotSubmitted;
            }

            var doesNotExceedActiveLoads = false;
            if (notification.HasShipmentInfo)
            {
                doesNotExceedActiveLoads = currentMovements < notification.ShipmentInfo.NumberOfShipments;
            }

            return doesNotExceedActiveLoads && isSubmitted;
        }

        public int GetNextMovementNumber(Guid notificationApplicationId)
        {
            if (!context.NotificationApplications.Any(na => na.Id == notificationApplicationId))
            {
                throw new InvalidOperationException("Cannot get next movement number for non-existent notification " + notificationApplicationId);
            }

            return context.Movements.Count(m => m.NotificationApplicationId == notificationApplicationId) + 1;
        }

        public bool DateIsValid(Guid notificationId, DateTime date)
        {
            var notification = context.NotificationApplications.Single(na => na.Id == notificationId);

            bool isGreaterThanOrEqualToFirstDate = date >= notification.ShipmentInfo.FirstDate;
            bool isLessThanOrEqualToLastDate = date <= notification.ShipmentInfo.LastDate;

            return isGreaterThanOrEqualToFirstDate && isLessThanOrEqualToLastDate;
        }
    }
}

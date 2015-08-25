﻿namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using Domain;

    internal class TestService : INotificationMovementService 
    {
        public bool CanCreate { get; set; }

        public int NextNumber { get; set; }

        public bool DateValid { get; set; }

        public bool CanCreateNewMovementForNotification(Guid notificationId)
        {
            return CanCreate;
        }

        public int GetNextMovementNumber(Guid notificationApplicationId)
        {
            return NextNumber;
        }

        public bool DateIsValid(Guid notificationId, DateTime date)
        {
            return DateValid;
        }
    }
}
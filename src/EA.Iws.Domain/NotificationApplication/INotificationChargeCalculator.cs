﻿namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Threading.Tasks;

    public interface INotificationChargeCalculator
    {
        Task<decimal> GetValue(Guid notificationId);
    }
}
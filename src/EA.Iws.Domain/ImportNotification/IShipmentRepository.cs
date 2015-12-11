﻿namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Threading.Tasks;

    public interface IShipmentRepository
    {
        Task<Shipment> GetByNotificationId(Guid notificationId);

        void Add(Shipment shipment);
    }
}
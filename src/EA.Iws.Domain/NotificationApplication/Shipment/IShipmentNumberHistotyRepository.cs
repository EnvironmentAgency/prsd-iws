﻿namespace EA.Iws.Domain.NotificationApplication.Shipment
{
    using System;
    using System.Threading.Tasks;

    public interface IShipmentNumberHistotyRepository
    {
        Task<ShipmentNumberHistory> GetOriginalNumberOfShipments(Guid notificationId);
    }
}

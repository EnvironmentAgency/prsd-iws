﻿namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using Core.IntendedShipments;
    using Core.Notification;
    using Core.Notification.Overview;
    using Core.Shared;

    public class AmountsAndDatesViewModel
    {
        public Guid NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }
        public bool IsIntendedShipmentsCompleted { get; set; }
        public IntendedShipmentData IntendedShipmentData { get; set; }
        public bool ShowChangeShipmentNumberLink { get; set; }

        public AmountsAndDatesViewModel()
        {
        }

        public AmountsAndDatesViewModel(ShipmentOverview amountAndDatesInfo, NotificationApplicationCompletionProgress progress)
        {
            NotificationId = amountAndDatesInfo.NotificationId;
            NotificationType = amountAndDatesInfo.NotificationType;
            IsIntendedShipmentsCompleted = progress.HasShipmentInfo;
            IntendedShipmentData = amountAndDatesInfo.IntendedShipmentData ?? new IntendedShipmentData();
        }
    }
}
﻿namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.NumberOfShipments
{
    using System;
    using Core.ImportNotification;

    public class ConfirmViewModel
    {
        public int NewNumberOfShipments { get; set; }

        public int OldNumberOfShipments { get; set; }

        public Guid NotificationId { get; set; }

        public decimal CurrentCharge { get; set; }

        public decimal NewCharge { get; set; }

        public ConfirmViewModel()
        {
        }

        public ConfirmViewModel(ConfirmNumberOfShipmentsChangeData data)
        {
            NotificationId = data.NotificationId;
            CurrentCharge = data.CurrentCharge;
            OldNumberOfShipments = data.CurrentNumberOfShipments;
            NewCharge = data.NewCharge;
        }

        public bool IsIncrease
        {
            get { return NewNumberOfShipments > OldNumberOfShipments; }
        }

        public decimal IncreaseInCharge
        {
            get { return NewCharge - CurrentCharge; }
        }
    }
}
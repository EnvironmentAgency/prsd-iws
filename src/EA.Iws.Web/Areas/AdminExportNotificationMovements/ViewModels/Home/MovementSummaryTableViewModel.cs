﻿namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.Home
{
    using System;
    using Core.Movement;
    using Core.Shared;
    using Prsd.Core;

    public class MovementSummaryTableViewModel
    {
        public Guid Id { get; set; }

        public int Number { get; set; }

        public MovementStatus Status { get; set; }

        public DateTime? PreNotification { get; set; }

        public DateTime? ShipmentDate { get; set; }

        public DateTime? Received { get; set; }

        public decimal? Quantity { get; set; }

        public ShipmentQuantityUnits? Unit { get; set; }

        public DateTime? RecoveredOrDisposedOf { get; set; }

        public MovementSummaryTableViewModel(MovementTableDataRow data)
        {
            Id = data.Id;
            Number = data.Number;
            Status = data.Status;
            PreNotification = data.SubmittedDate;
            ShipmentDate = data.ShipmentDate;
            Received = data.ReceivedDate;
            Quantity = data.Quantity;
            Unit = data.QuantityUnits;
            RecoveredOrDisposedOf = data.CompletedDate;
        }

        public bool IsShipped()
        {
            return Status == MovementStatus.Submitted && ShipmentDate < SystemTime.UtcNow;
        }

        public bool IsShipmentActive()
        {
            return (Status == MovementStatus.New || Status == MovementStatus.Captured) && ShipmentDate <= SystemTime.UtcNow;
        }
    }
}
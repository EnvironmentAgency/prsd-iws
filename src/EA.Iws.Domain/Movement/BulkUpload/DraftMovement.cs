﻿namespace EA.Iws.Domain.Movement.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using Core.Shared;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;

    public class DraftMovement : Entity
    {
        public Guid BulkUploadId { get; set; }

        public string NotificationNumber { get; set; }

        public int ShipmentNumber { get; set; }

        public decimal Quantity { get; set; }

        public ShipmentQuantityUnits Units { get; set; }

        // Actual Date of Shipment
        public DateTime? Date { get; set; }

        public IEnumerable<DraftPackagingInfo> PackagingInfos
        {
            get { return PackagingInfosCollection.ToSafeIEnumerable(); }
        }

        protected virtual ICollection<DraftPackagingInfo> PackagingInfosCollection { get; set; }

        public DateTime? ReceivedDate { get; set; }

        public DateTime? RecoveredDisposedDate { get; set; }

        public DraftMovement()
        {
        }

        public DraftMovement(Guid draftMovementId,
            string notificationNumber,
            int shipmentNumber,
            decimal quantity,
            ShipmentQuantityUnits units,
            DateTime date,
            ICollection<DraftPackagingInfo> packagingInfos)
        {
            BulkUploadId = draftMovementId;
            NotificationNumber = notificationNumber;
            ShipmentNumber = shipmentNumber;
            Quantity = quantity;
            Units = units;
            Date = date;

            PackagingInfosCollection = packagingInfos;
        }

        public DraftMovement(Guid draftMovementId,
            string notificationNumber,
            int shipmentNumber,
            DateTime? receivedDate,
            decimal quantity,
            ShipmentQuantityUnits units,
            DateTime? recoveredDisposedDate)
        {
            BulkUploadId = draftMovementId;
            NotificationNumber = notificationNumber;
            ShipmentNumber = shipmentNumber;
            ReceivedDate = receivedDate;
            Quantity = quantity;
            Units = units;
            RecoveredDisposedDate = recoveredDisposedDate;
        }
    }
}

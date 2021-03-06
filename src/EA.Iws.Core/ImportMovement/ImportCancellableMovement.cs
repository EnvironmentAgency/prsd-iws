﻿namespace EA.Iws.Core.ImportMovement
{
    using System;

    public class ImportCancellableMovement
    {
        public Guid MovementId { get; set; }

        public Guid NotificationId { get; set; }

        public int Number { get; set; }

        public DateTime ActualShipmentDate { get; set; }

        public DateTime? PrenotificationDate { get; set; }
    }
}
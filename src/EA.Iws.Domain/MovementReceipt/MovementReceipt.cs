﻿namespace EA.Iws.Domain.MovementReceipt
{
    using System;
    using Core.MovementReceipt;
    using Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class MovementReceipt : Entity
    {
        protected MovementReceipt()
        {
        }

        internal MovementReceipt(DateTime dateReceived)
        {
            Date = dateReceived;
        }
        
        public DateTime Date { get; internal set; }

        public Decision? Decision { get; internal set; }

        public string RejectReason { get; internal set; }

        public decimal? Quantity { get; internal set; }

        public void SetQuantity(decimal quantity, 
            ShipmentQuantityUnits displayUnits, 
            ShipmentQuantityUnits notificationUnits)
        {
            Guard.ArgumentNotZeroOrNegative(() => quantity, quantity);

            if (Decision.HasValue && Decision.Value == Core.MovementReceipt.Decision.Accepted)
            {
                Quantity = ShipmentQuantityUnitConverter.ConvertToTarget(displayUnits, notificationUnits, quantity);
            }
            else
            {
                throw new InvalidOperationException(
                    "Cannot set quantity for a movement receipt where the movement has not been accepted. Receipt: "
                    + Id);
            }
        }
    }
}

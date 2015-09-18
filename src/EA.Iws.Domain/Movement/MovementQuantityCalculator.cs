﻿namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Shared;
    using NotificationApplication;

    public class MovementQuantityCalculator
    {
        private readonly ReceivedMovementCalculator receivedMovementCalculator;

        public MovementQuantityCalculator(ReceivedMovementCalculator receivedMovementCalculator)
        {
            this.receivedMovementCalculator = receivedMovementCalculator;
        }

        public decimal QuantityReceived(IList<Movement> movements)
        {
            var receivedMovements = receivedMovementCalculator.ReceivedMovements(movements);

            if (!HasReceivedMovements(receivedMovements))
            {
                return 0;
            }

            ShipmentQuantityUnits unit;
            if (TryParseMovementsUnit(receivedMovements, out unit))
            {
                return receivedMovements.Sum(m => m.Receipt.Quantity.Value);
            }
            
            throw new InvalidOperationException("Cannot sum movement quantities that have different units.");
        }

        public decimal QuantityRemaining(ShipmentInfo shipmentInfo, IList<Movement> movements)
        {
            var receivedMovements = receivedMovementCalculator.ReceivedMovements(movements);

            if (!HasReceivedMovements(receivedMovements))
            {
                return shipmentInfo.Quantity;
            }

            ShipmentQuantityUnits unit;
            if (TryParseMovementsUnit(receivedMovements, out unit) && unit == shipmentInfo.Units)
            {
                return shipmentInfo.Quantity - QuantityReceived(receivedMovements);
            }
            else
            {
                throw new InvalidOperationException("Cannot sum quantites where the movements units and the original shipment units differ.");
            }
        }

        private bool TryParseMovementsUnit(IList<Movement> movements, out ShipmentQuantityUnits unit)
        {
            if (movements.Any(m => !m.Units.HasValue))
            {
                unit = default(ShipmentQuantityUnits);
                return false;
            }

            var units = movements.Select(m => m.Units.Value).Distinct();

            if (units.Count() > 1)
            {
                unit = default(ShipmentQuantityUnits);
                return false;
            }
            else
            {
                unit = units.Single();
                return true;
            }
        }

        private bool HasReceivedMovements(IList<Movement> receivedMovements)
        {
            return receivedMovements != null && receivedMovements.Count > 0;
        }
    }
}

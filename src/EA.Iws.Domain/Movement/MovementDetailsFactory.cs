﻿namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NotificationApplication;

    public class MovementDetailsFactory
    {
        private readonly NotificationMovementsQuantity movementsQuantity;

        public MovementDetailsFactory(NotificationMovementsQuantity movementsQuantity)
        {
            this.movementsQuantity = movementsQuantity;
        }

        public async Task<MovementDetails> Create(Movement movement, ShipmentQuantity shipmentQuantity, int numberOfPackages,
            IEnumerable<MovementCarrier> carriers, IEnumerable<PackagingInfo> packages)
        {
            var remaining = await movementsQuantity.Remaining(movement.NotificationId);

            if (shipmentQuantity > remaining)
            {
                throw new InvalidOperationException(string.Format(
                    "Cannot create new movement details for movement {0} as the quantity exceeds what is remaining", movement.Id));
            }

            return new MovementDetails(movement.Id, shipmentQuantity, numberOfPackages, carriers, packages);
        }
    }
}
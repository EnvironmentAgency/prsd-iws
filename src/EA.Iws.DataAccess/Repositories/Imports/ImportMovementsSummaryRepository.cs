﻿namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ImportNotificationMovements;
    using Core.Shared;
    using Domain;
    using Domain.ImportMovement;
    using Domain.ImportNotification;

    internal class ImportMovementsSummaryRepository : IImportMovementsSummaryRepository
    {
        private readonly ImportNotificationContext context;

        public ImportMovementsSummaryRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public async Task<Summary> GetById(Guid importNotificationId)
        {
            var totalMovements = await context.ImportMovements.Where(m => m.NotificationId == importNotificationId).CountAsync();

            var shipment = await context.Shipments.Where(s => s.ImportNotificationId == importNotificationId).SingleAsync();

            var received = await TotalQuantityReceived(importNotificationId, shipment);

            return new Summary
            {
                IntendedShipments = shipment.NumberOfShipments,
                UsedShipments = totalMovements,
                DisplayUnit = shipment.Quantity.Units,
                QuantityReceivedTotal = received.Quantity,
                QuantityRemainingTotal = shipment.Quantity.Quantity - received.Quantity
            };
        }

        private async Task<ShipmentQuantity> TotalQuantityReceived(Guid importNotificationId, Shipment shipment)
        {
            var movements = await context.ImportMovements.Where(m => m.NotificationId == importNotificationId).ToArrayAsync();

            var allMovementReceipts = new List<ImportMovementReceipt>();

            foreach (var movement in movements)
            {
                var movementReceipts = await context.ImportMovementReceipts.Where(mr => mr.MovementId == movement.Id).ToListAsync();
                allMovementReceipts = allMovementReceipts.Union(movementReceipts).ToList();
            }

            if (!allMovementReceipts.Any())
            {
                return new ShipmentQuantity(0, shipment == null ? ShipmentQuantityUnits.Tonnes : shipment.Quantity.Units);
            }
            
            var totalReceived = allMovementReceipts.Sum(m =>
                ShipmentQuantityUnitConverter.ConvertToTarget(
                    m.Unit,
                    shipment.Quantity.Units,
                    m.Quantity));

            return new ShipmentQuantity(totalReceived, shipment.Quantity.Units);
        }
    }
}

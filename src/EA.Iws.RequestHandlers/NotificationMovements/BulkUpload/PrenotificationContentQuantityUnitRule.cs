﻿namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Core.Shared;
    using Domain.NotificationApplication.Shipment;

    public class PrenotificationContentQuantityUnitRule : IBulkMovementPrenotificationContentRule
    {
        private readonly IShipmentInfoRepository shipmentInfoRepository;

        public PrenotificationContentQuantityUnitRule(IShipmentInfoRepository shipmentInfoRepository)
        {
            this.shipmentInfoRepository = shipmentInfoRepository;
        }

        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(
            List<PrenotificationMovement> movements, Guid notificationId)
        {
            var shipment = await shipmentInfoRepository.GetByNotificationId(notificationId);
            var units = shipment == null ? default(ShipmentQuantityUnits) : shipment.Units;

            return await Task.Run(() =>
            {
                var result = MessageLevel.Success;
                var failedShipments = new List<string>();

                foreach (var movement in movements)
                {
                    if (movement.ShipmentNumber.HasValue && movement.Unit.HasValue && movement.Unit.Value != units)
                    {
                        result = MessageLevel.Error;
                        failedShipments.Add(movement.ShipmentNumber.Value.ToString());
                    }
                }

                var shipmentNumbers = string.Join(", ", failedShipments);
                var errorMessage =
                    string.Format(
                        Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.QuantityUnit),
                        shipmentNumbers);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.QuantityUnit,
                    result, errorMessage);
            });
        }
    }
}
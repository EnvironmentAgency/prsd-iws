﻿namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;

    public class PrenotificationContentDuplicateShipmentNumberRule : IBulkMovementPrenotificationContentRule
    {
        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> shipments, Guid notificationId)
        {
            return await Task.Run(() =>
            {
                var duplicateShipmentNumberResult = MessageLevel.Success;
                var duplicateShipmentNumbers = shipments.Where(h => h.HasShipmentNumber)
                    .GroupBy(x => x.ShipmentNumber)
                    .Where(g => g.Count() > 1)
                    .Select(y => y.Key)
                    .ToList();

                if (duplicateShipmentNumbers.Count > 0)
                {
                    duplicateShipmentNumberResult = MessageLevel.Error;
                }

                var shipmentNumbers = string.Join(", ", duplicateShipmentNumbers);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.DuplicateShipmentNumber), shipmentNumbers);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.DuplicateShipmentNumber, duplicateShipmentNumberResult, errorMessage);
            });
        }
    }
}

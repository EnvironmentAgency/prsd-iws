﻿namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;

    public class PrenotificationContentMissingDataRule : IBulkMovementPrenotificationContentRule
    {
        public PrenotificationContentMissingDataRule()
        {
        }

        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> shipments)
        {
            return await Task.Run(() =>
            {
                var missingDataResult = MessageLevel.Success;
                var missingDataShipmentNumbers = new List<string>();

                foreach (PrenotificationMovement dto in shipments)
                {
                    if (dto.ActualDateOfShipment.Equals(string.Empty) ||
                    dto.NotificationNumber.Equals(string.Empty) ||
                    dto.PackagingType.Equals(string.Empty) ||
                    dto.Quantity.Equals(string.Empty) ||
                    dto.Unit.Equals(string.Empty))
                    {
                        missingDataResult = MessageLevel.Error;
                        missingDataShipmentNumbers.Add(dto.ShipmentNumber);
                    }
                }

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.MissingData, missingDataResult, missingDataShipmentNumbers);
            });
        }
    }
}
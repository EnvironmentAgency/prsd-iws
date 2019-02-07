﻿namespace EA.Iws.RequestHandlers.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using EA.Iws.Core.Movement.BulkReceiptRecovery;
    using EA.Iws.Core.Rules;

    public class ReceiptRecoveryReceiptDateFormatRule : IReceiptRecoveryContentRule
    {
        public async Task<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>> GetResult(List<ReceiptRecoveryMovement> movements, Guid notificationId)
        {
            return await Task.Run(() =>
            {
                var shipments =
                    movements.Where(m => m.ShipmentNumber.HasValue && !m.ReceivedDate.HasValue)
                        .Select(m => m.ShipmentNumber.Value)
                        .ToList();

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;

                var shipmentNumbers = string.Join(", ", shipments);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.ReceiptDateFormat), shipmentNumbers);

                return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.ReceiptDateFormat, result, errorMessage);
            });
        }
    }
}

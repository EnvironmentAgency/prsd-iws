﻿namespace EA.Iws.RequestHandlers.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.BulkUpload;

    internal class PerformReceiptRecoveryContentValidationHandler : IRequestHandler<PerformReceiptRecoveryContentValidation, ReceiptRecoveryRulesSummary>
    {
        private readonly IEnumerable<IReceiptRecoveryContentRule> contentRules;
        private readonly IMap<DataTable, List<ReceiptRecoveryMovement>> mapper;
        private const int MaxShipments = 50;

        public PerformReceiptRecoveryContentValidationHandler(IEnumerable<IReceiptRecoveryContentRule> contentRules,
            IMap<DataTable, List<ReceiptRecoveryMovement>> mapper)
        {
            this.contentRules = contentRules;
            this.mapper = mapper;
        }

        public async Task<ReceiptRecoveryRulesSummary> HandleAsync(PerformReceiptRecoveryContentValidation message)
        {
            var result = message.BulkMovementRulesSummary;

            var movements = mapper.Map(message.DataTable);

            result.ContentRulesResults = await GetOrderedContentRules(movements, message.NotificationId);

            if (result.IsContentRulesSuccess)
            {
                result.ShipmentNumbers =
                    movements.Where(m => m.ShipmentNumber.HasValue).Select(m => m.ShipmentNumber.Value);

                //result.DraftBulkUploadId = await repository.Add(message.NotificationId, movements, message.FileName);
            }

            return result;
        }

        private async Task<List<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>>> GetOrderedContentRules(List<ReceiptRecoveryMovement> movements,
            Guid notificationId)
        {
            var rules = new List<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>>();

            var maxShipments = await GetMaxShipments(movements);

            rules.Add(maxShipments);

            if (maxShipments.MessageLevel == MessageLevel.Success)
            {
                var missingNotificationNumbersOrShipmentNumbers = await GetMissingNotificationNumbersOrShipmentNumbers(movements);

                rules.Add(missingNotificationNumbersOrShipmentNumbers);

                if (missingNotificationNumbersOrShipmentNumbers.MessageLevel == MessageLevel.Success)
                {
                    var missingData = await GetMissingReceiptDataResult(movements, notificationId);

                    rules.Add(missingData);

                    // Only run rest of validations if there are no missing/blank data.
                    if (missingData.MessageLevel == MessageLevel.Success)
                    {
                        foreach (var rule in contentRules)
                        {
                            rules.Add(await rule.GetResult(movements, notificationId));
                        }
                    }
                }
            }
            
            return rules.OrderBy(r => r.Rule).ToList();
        }

        private static async Task<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>> GetMaxShipments(
            IReadOnlyCollection<ReceiptRecoveryMovement> movements)
        {
            return await Task.Run(() =>
            {
                var result = movements.Count > MaxShipments ? MessageLevel.Error : MessageLevel.Success;

                var errorMessage =
                    string.Format(
                        Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.MaximumShipments),
                        MaxShipments);

                return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.MaximumShipments,
                    result, errorMessage);
            });
        }

        private static async Task<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>> GetMissingNotificationNumbersOrShipmentNumbers(
            IReadOnlyCollection<ReceiptRecoveryMovement> movements)
        {
            return await Task.Run(() =>
            {
                var result = movements.Any(m => m.MissingShipmentNumber || !m.ShipmentNumber.HasValue 
                    || m.MissingNotificationNumber || string.IsNullOrEmpty(m.NotificationNumber))
                    ? MessageLevel.Error
                    : MessageLevel.Success;

                var errorMessage = Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.InvalidNotificationOrShipmentNumbers);

                return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.InvalidNotificationOrShipmentNumbers,
                    result, errorMessage);
            });
        }

        private async Task<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>> GetMissingReceiptDataResult(List<ReceiptRecoveryMovement> movements, Guid notificationId)
        {
            return await Task.Run(() =>
            {
                var missingDataResult = MessageLevel.Success;
                var missingDataShipmentNumbers = new List<string>();

                foreach (var movement in movements)
                {
                    // Only report an error if record has a shipment number, otherwise record will be picked up by the GetMissingShipmentNumbers method
                    // Only report an error if record has a notification number, otherwise record will be picked up by the GetMissingNotificationNumbers method
                    if (movement.ShipmentNumber.HasValue &&
                        !string.IsNullOrEmpty(movement.NotificationNumber) &&
                        ((movement.MissingReceivedDate && movement.MissingRecoveredDisposedDate) ||
                        movement.MissingQuantity ||
                        movement.MissingUnits))
                    {
                        missingDataResult = MessageLevel.Error;
                        missingDataShipmentNumbers.Add(movement.ShipmentNumber.ToString());
                    }
                }

                var shipmentNumbers = string.Join(", ", missingDataShipmentNumbers);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.MissingReceiptData), shipmentNumbers);

                return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.MissingReceiptData, missingDataResult, errorMessage);
            });
        }
    }
}
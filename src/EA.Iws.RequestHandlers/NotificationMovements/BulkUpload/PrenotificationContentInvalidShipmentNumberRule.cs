﻿namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Domain.Movement;

    public class PrenotificationContentInvalidShipmentNumberRule : IBulkMovementPrenotificationContentRule
    {
        private readonly INotificationMovementsSummaryRepository repo;

        public PrenotificationContentInvalidShipmentNumberRule(INotificationMovementsSummaryRepository repo)
        {
            this.repo = repo;
        }

        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            var movementSummary = await this.repo.GetById(notificationId);

            return await Task.Run(() =>
            {
                var lastNumber = movements.OrderByDescending(m => m.ShipmentNumber).First().ShipmentNumber.GetValueOrDefault();

                var result = lastNumber > movementSummary.IntendedTotalShipments
                    ? MessageLevel.Error
                    : MessageLevel.Success;

                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.InvalidShipmentNumber), lastNumber);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.InvalidShipmentNumber, result, errorMessage);
            });
        }
    }
}
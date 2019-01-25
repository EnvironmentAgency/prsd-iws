﻿namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement.Bulk;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanEditExportMovements)]
    public class PerformBulkUploadContentValidation : IRequest<BulkMovementRulesSummary>
    {
        public BulkMovementRulesSummary BulkMovementRulesSummary { get; private set; }

        public Guid NotificationId { get; private set; }

        public PerformBulkUploadContentValidation(BulkMovementRulesSummary bulkMovementRulesSummary, Guid notificationId)
        {
            BulkMovementRulesSummary = bulkMovementRulesSummary;
            NotificationId = notificationId;
        }
    }
}
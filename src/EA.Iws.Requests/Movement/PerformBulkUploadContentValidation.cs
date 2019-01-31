﻿namespace EA.Iws.Requests.Movement
{
    using System;
    using System.Data;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement.Bulk;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanEditExportMovements)]
    public class PerformBulkUploadContentValidation : IRequest<BulkMovementRulesSummary>
    {
        public BulkMovementRulesSummary BulkMovementRulesSummary { get; private set; }

        public Guid NotificationId { get; private set; }

        public DataTable DataTable { get; private set; }

        public string FileName { get; private set; }

        public PerformBulkUploadContentValidation(BulkMovementRulesSummary bulkMovementRulesSummary, 
            Guid notificationId, 
            DataTable dataTable,
            string fileName)
        {
            BulkMovementRulesSummary = bulkMovementRulesSummary;
            NotificationId = notificationId;
            DataTable = dataTable;
            FileName = fileName;
        }
    }
}
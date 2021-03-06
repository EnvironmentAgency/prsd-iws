﻿namespace EA.Iws.Requests.ImportMovement.Reject
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanEditImportMovements)]
    public class RecordRejection : IRequest<bool>
    {
        public Guid ImportMovementId { get; private set; }

        public DateTime Date { get; private set; }

        public string Reason { get; private set; }

        public RecordRejection(Guid importMovementId, 
            DateTime date, 
            string reason)
        {
            ImportMovementId = importMovementId;
            Date = date;
            Reason = reason;
        }
    }
}

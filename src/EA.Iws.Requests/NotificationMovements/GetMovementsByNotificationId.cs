﻿namespace EA.Iws.Requests.NotificationMovements
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GetMovementsByNotificationId : IRequest<IList<MovementTableDataRow>>
    {
        public Guid Id { get; private set; }

        public GetMovementsByNotificationId(Guid id)
        {
            Id = id;
        }
    }
}

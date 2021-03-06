﻿namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanEditExportMovementsExternal)]
    public class SetMovementFileId : IRequest<Guid>
    {
        public SetMovementFileId(Guid movementId, byte[] movementBytes, string fileType)
        {
            MovementId = movementId;
            MovementBytes = movementBytes;
            FileType = fileType;
        }

        public Guid MovementId { get; private set; }

        public byte[] MovementBytes { get; private set; }

        public string FileType { get; private set; }
    }
}

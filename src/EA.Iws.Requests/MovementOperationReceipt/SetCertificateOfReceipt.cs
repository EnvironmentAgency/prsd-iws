﻿namespace EA.Iws.Requests.MovementOperationReceipt
{
    using System;
    using Prsd.Core.Mediator;

    public class SetCertificateOfReceipt : IRequest<Guid>
    {
        public SetCertificateOfReceipt(Guid movementId, byte[] certificateBytes, string fileType)
        {
            MovementId = movementId;
            CertificateBytes = certificateBytes;
            FileType = fileType;
        }

        public Guid MovementId { get; private set; }

        public byte[] CertificateBytes { get; private set; }

        public string FileType { get; private set; }
    }
}
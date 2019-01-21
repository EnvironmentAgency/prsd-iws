﻿namespace EA.Iws.Domain
{
    using System;
    using System.Threading.Tasks;
    using Core.Documents;

    public interface IMovementDocumentGenerator
    {
        Task<byte[]> Generate(Guid movementId);
        Task<byte[]> GenerateMultiple(Guid[] movementIds);
        byte[] GenerateBulkUploadTemplate(BulkType bulkType);
    }
}

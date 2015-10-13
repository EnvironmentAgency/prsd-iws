﻿namespace EA.Iws.RequestHandlers.MovementReceipt
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.FileStore;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.MovementReceipt;

    internal class SetCertificateOfReceiptHandler : IRequestHandler<SetCertificateOfReceipt, Guid>
    {
        private readonly IMovementRepository movementRepository;
        private readonly CertificateOfReceiptNameGenerator nameGenerator;
        private readonly CertificateFactory certificateFactory;
        private readonly IwsContext context;
        private readonly IFileRepository fileRepository;

        public SetCertificateOfReceiptHandler(IwsContext context,
            IFileRepository fileRepository,
            IMovementRepository movementRepository,
            CertificateFactory certificateFactory,
            CertificateOfReceiptNameGenerator nameGenerator)
        {
            this.context = context;
            this.fileRepository = fileRepository;
            this.certificateFactory = certificateFactory;
            this.nameGenerator = nameGenerator;
            this.movementRepository = movementRepository;
        }

        public async Task<Guid> HandleAsync(SetCertificateOfReceipt message)
        {
            var movement = await movementRepository.GetById(message.MovementId);

            var receipt = await certificateFactory.CreateForMovement(nameGenerator, movement, message.CertificateBytes, message.FileType);

            var fileId = await fileRepository.Store(receipt);

            movement.Receipt.SetCertificateFile(fileId);

            await context.SaveChangesAsync();

            return fileId;
        }
    }
}
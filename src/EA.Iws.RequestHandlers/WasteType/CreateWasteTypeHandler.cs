﻿namespace EA.Iws.RequestHandlers.WasteType
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.Notification;
    using Prsd.Core.Mediator;
    using Requests.WasteType;

    public class CreateWasteTypeHandler : IRequestHandler<CreateWasteType, Guid>
    {
        private readonly IwsContext db;

        public CreateWasteTypeHandler(IwsContext db)
        {
            this.db = db;
        }

        public async Task<Guid> HandleAsync(CreateWasteType command)
        {
            ChemicalComposition chemicalComposition;
            switch (command.ChemicalCompositionType)
            {
                case ChemicalCompositionType.RDF:
                    chemicalComposition = ChemicalComposition.RDF;
                    break;
                case ChemicalCompositionType.SRF:
                    chemicalComposition = ChemicalComposition.SRF;
                    break;
                case ChemicalCompositionType.Wood:
                    chemicalComposition = ChemicalComposition.Wood;
                    break;
                case ChemicalCompositionType.Other:
                    chemicalComposition = ChemicalComposition.Other;
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Unknown Chemical Composition Type: {0}", command.ChemicalCompositionType));
            }

            var notification = await db.NotificationApplications.SingleAsync(n => n.Id == command.NotificationId);
            notification.AddWasteType(chemicalComposition, command.ChemicalCompositionName, command.ChemicalCompositionDescription);

            if (command.ChemicalCompositionType == ChemicalCompositionType.RDF || command.ChemicalCompositionType == ChemicalCompositionType.SRF)
            {
                foreach (var item in command.WasteCompositions)
                {
                    notification.AddWasteComposition(item.Constituent, item.MinConcentration, item.MaxConcentration);
                }
            }

            await db.SaveChangesAsync();

            return notification.WasteType.Id;
        }
    }
}

﻿namespace EA.Iws.Requests.WasteType
{
    using System;
    using Prsd.Core.Mediator;
    using Security;

    [NotificationReadOnlyAuthorize]
    public class SetEnergy : IRequest<Guid>
    {
        public SetEnergy(string energyInformation, Guid notificationId)
        {
            EnergyInformation = energyInformation;
            NotificationId = notificationId;
        }

        public string EnergyInformation { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}
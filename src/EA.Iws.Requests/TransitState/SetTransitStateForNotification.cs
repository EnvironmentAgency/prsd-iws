﻿namespace EA.Iws.Requests.TransitState
{
    using System;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    public class SetTransitStateForNotification : IRequest<Guid>
    {
        public Guid? TransitStateId { get; private set; }

        public Guid NotificationId { get; private set; }

        public Guid CountryId { get; private set; }

        public Guid EntryPointId { get; private set; }

        public Guid ExitPointId { get; set; }

        public Guid CompetentAuthorityId { get; private set; }

        public int? OrdinalPosition { get; set; }

        public SetTransitStateForNotification(Guid notificationId, 
            Guid countryId, 
            Guid entryPointId, 
            Guid exitPointId, 
            Guid competentAuthorityId,
            Guid? transitStateId,
            int? ordinalPosition)
        {
            NotificationId = notificationId;
            CountryId = countryId;
            EntryPointId = entryPointId;
            ExitPointId = exitPointId;
            CompetentAuthorityId = competentAuthorityId;
            TransitStateId = transitStateId;
            OrdinalPosition = ordinalPosition;
        }
    }
}

﻿namespace EA.Iws.TestHelpers.Helpers
{
    using System;
    using Domain;
    using Domain.TransportRoute;

    public class EntryOrExitPointFactory
    {
        public static EntryOrExitPoint Create(Guid id, Country country)
        {
            var point = ObjectInstantiator<EntryOrExitPoint>.CreateNew();
            ObjectInstantiator<EntryOrExitPoint>.SetProperty(x => x.Id, id, point);
            ObjectInstantiator<EntryOrExitPoint>.SetProperty(x => x.Country, country, point);

            return point;
        }
    }
}

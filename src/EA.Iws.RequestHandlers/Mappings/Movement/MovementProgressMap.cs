﻿namespace EA.Iws.RequestHandlers.Mappings.Movement
{
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;
    using System.Linq;

    public class MovementProgressMap : IMap<Movement, ProgressData>
    {
        public ProgressData Map(Movement source)
        {
            if (source == null)
            {
                return new ProgressData();
            }

            return new ProgressData
            {
                IsActualDateCompleted = source.Date.HasValue,
                IsActualQuantityCompleted = source.Quantity.HasValue,
                IsNumberOfPackagesCompleted = source.NumberOfPackages.HasValue,
                AreIntendedCarriersCompleted = (source.MovementCarriers != null) && source.MovementCarriers.Any(),
                ArePackagingTypesCompleted = (source.PackagingInfos != null) && source.PackagingInfos.Any()
            };
        }
    }
}

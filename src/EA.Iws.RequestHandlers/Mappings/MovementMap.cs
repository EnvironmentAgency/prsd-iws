﻿namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;
    using System.Linq;

    public class MovementMap : IMap<Movement, ProgressData>
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
                AreIntendedCarriersCompleted = false,
                ArePackagingTypesCompleted = (source.PackagingInfos != null) && source.PackagingInfos.Any()
            };
        }
    }
}

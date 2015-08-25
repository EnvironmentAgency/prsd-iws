﻿namespace EA.Iws.Core.Shared
{
    using System.Collections.Generic;
    using System.Linq;

    public static class ShipmentQuantityUnitsMetadata
    {
        public static IEnumerable<ShipmentQuantityUnits> VolumeUnits = new[]
        {
            ShipmentQuantityUnits.Litres,
            ShipmentQuantityUnits.CubicMetres,
        };

        public static IEnumerable<ShipmentQuantityUnits> WeightUnits = new[]
        {
            ShipmentQuantityUnits.Kilograms,
            ShipmentQuantityUnits.Tonnes
        };

        public static bool IsVolumeUnit(ShipmentQuantityUnits unit)
        {
            return VolumeUnits.Contains(unit);
        }

        public static bool IsWeightUnit(ShipmentQuantityUnits unit)
        {
            return WeightUnits.Contains(unit);
        }

        public static IEnumerable<ShipmentQuantityUnits> GetUnitsOfThisType(ShipmentQuantityUnits unit)
        {
            return (IsWeightUnit(unit))
                ? WeightUnits
                : VolumeUnits;
        }
    }
}

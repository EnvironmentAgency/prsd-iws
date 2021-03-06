﻿namespace EA.Iws.DocumentGeneration.Formatters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.WasteType;
    using Domain.NotificationApplication;
    using ViewModels;

    public class WasteCompositionFormatter
    {
        private static readonly Func<string, string> GetConstituentWithUnits =
            composition => (composition == null) ? string.Empty : composition + " wt/wt %";

        public string GetWasteName(WasteType wasteType)
        {
            if (wasteType == null)
            {
                return string.Empty;
            }

            if (wasteType.ChemicalCompositionType == ChemicalComposition.RDF)
            {
                return "Refuse Derived Fuel (RDF)";
            }
            if (wasteType.ChemicalCompositionType == ChemicalComposition.SRF)
            {
                return "Solid Recovered Fuel (SRF)";
            }
            if (wasteType.ChemicalCompositionType == ChemicalComposition.Wood)
            {
                return "Wood";
            }

            return wasteType.ChemicalCompositionName;
        }

        public IList<ChemicalCompositionPercentages> GetAdditionalInformationChemicalCompositionPercentages(
            IEnumerable<WasteAdditionalInformation> wasteAdditionalInformations)
        {
            if (wasteAdditionalInformations == null || !wasteAdditionalInformations.Any())
            {
                return new ChemicalCompositionPercentages[0];
            }

            return wasteAdditionalInformations
                .OrderBy(x => x.WasteInformationType)
                .Where(x => !(x.MaxConcentration == 0 && x.MinConcentration == 0))
                .Select(x => new ChemicalCompositionPercentages
            {
                Min = x.MinConcentration.ToString("N"),
                Max = x.MaxConcentration.ToString("N"),
                Name = GetChemicalConstituentName(x)
            }).ToArray();
        }

        public IList<ChemicalCompositionPercentages> GetWasteCompositionPercentages(WasteType wasteType)
        {
            if (wasteType == null || wasteType.WasteCompositions == null || !wasteType.WasteCompositions.Any())
            {
                return new ChemicalCompositionPercentages[0];
            }
            
            return wasteType.WasteCompositions
                .OrderBy(x => x.ChemicalCompositionType)
                .Where(x => !(x.MaxConcentration == 0 && x.MinConcentration == 0))
                .Select(x => new ChemicalCompositionPercentages
                {
                    Min = x.MinConcentration.ToString("N"),
                    Max = x.MaxConcentration.ToString("N"),
                    Name = GetConstituentWithUnits(x.Constituent)
                }).ToArray();
        }

        public string GetChemicalConstituentName(WasteAdditionalInformation wasteAdditionalInformation)
        {
            if (wasteAdditionalInformation.WasteInformationType == WasteInformationType.HeavyMetals)
            {
                return wasteAdditionalInformation.Constituent + " mg/kg";
            }

            if (wasteAdditionalInformation.WasteInformationType == WasteInformationType.NetCalorificValue)
            {
                return wasteAdditionalInformation.Constituent + " MJ/kg";
            }

            return GetConstituentWithUnits(wasteAdditionalInformation.Constituent);
        }

        public string GetEnergyEfficiencyString(WasteType wasteType)
        {
            if (wasteType == null || string.IsNullOrWhiteSpace(wasteType.EnergyInformation))
            {
                return string.Empty;
            }

            return Environment.NewLine + "Energy Efficiency = " + wasteType.EnergyInformation;
        }

        public string GetOptionalInformationTitle(WasteType wasteType)
        {
            if (wasteType != null && !string.IsNullOrWhiteSpace(wasteType.OptionalInformation))
            {
                return Environment.NewLine + "Additional Information" + Environment.NewLine +
                       wasteType.OptionalInformation + Environment.NewLine;
            }
            return string.Empty;
        }
    }
}

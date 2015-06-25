﻿namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteType
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.WasteType;
    using Web.ViewModels.Shared;

    public class ChemicalCompositionViewModel : IValidatableObject
    {
        public Guid Id { get; set; }

        public Guid NotificationId { get; set; }

        public RadioButtonStringCollectionViewModel ChemicalCompositionType { get; set; }

        public string OtherCompositionName { get; set; }

        public string Description { get; set; }

        public List<WasteCompositionData> WasteCompositions { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (ChemicalCompositionType.SelectedValue == Core.WasteType.ChemicalCompositionType.Wood.ToString())
            {
                if (String.IsNullOrWhiteSpace(Description))
                {
                    results.Add(new ValidationResult("The chemical composition of the waste is required."));
                }
            }
            else if (ChemicalCompositionType.SelectedValue == Core.WasteType.ChemicalCompositionType.Other.ToString())
            {
                if (String.IsNullOrWhiteSpace(OtherCompositionName))
                {
                    results.Add(new ValidationResult("The name of the waste is required."));
                }
                if (String.IsNullOrWhiteSpace(Description))
                {
                    results.Add(new ValidationResult("The chemical composition of the waste is required."));
                }
            }
            return results;
        }
    }
}
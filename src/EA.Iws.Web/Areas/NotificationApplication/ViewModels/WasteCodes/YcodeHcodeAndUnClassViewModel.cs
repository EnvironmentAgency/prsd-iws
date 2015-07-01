﻿namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.WasteCodes;

    public class YcodeHcodeAndUnClassViewModel : IValidatableObject
    {
        public YcodeHcodeAndUnClassViewModel()
        {
            SelectedHcodesList = new List<WasteCodeData>();
            SelectedYcodesList = new List<WasteCodeData>();
            SelectedUnClassesList = new List<WasteCodeData>();
        }

        public IEnumerable<WasteCodeData> Ycodes { get; set; }
        public IEnumerable<WasteCodeData> Hcodes { get; set; }
        public IEnumerable<WasteCodeData> UnClasses { get; set; }

        [Display(Name = "Y-code")]
        public string SelectedYcode { get; set; }

        [Display(Name = "H-code")]
        public string SelectedHcode { get; set; }

        [Display(Name = "UN class")]
        public string SelectedUnClass { get; set; }

        public Guid NotificationId { get; set; }

        public List<WasteCodeData> SelectedYcodesList { get; set; }
        public List<WasteCodeData> SelectedHcodesList { get; set; }
        public List<WasteCodeData> SelectedUnClassesList { get; set; }

        public string TypeOfCodeAdded { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(SelectedYcode) && (SelectedYcodesList == null || !SelectedYcodesList.Any()))
            {
                yield return new ValidationResult("Please enter a Y code or select not applicable", new[] { "SelectedYcode" });
            }

            if (string.IsNullOrEmpty(SelectedHcode) && (SelectedHcodesList == null || !SelectedHcodesList.Any()))
            {
                yield return new ValidationResult("Please enter a H code or select not applicable", new[] { "SelectedHcode" });
            }

            if (string.IsNullOrEmpty(SelectedUnClass) && (SelectedUnClassesList == null || !SelectedUnClassesList.Any()))
            {
                yield return new ValidationResult("Please enter a UN class or select not applicable", new[] { "SelectedUnClass" });
            }
        }
    }
}
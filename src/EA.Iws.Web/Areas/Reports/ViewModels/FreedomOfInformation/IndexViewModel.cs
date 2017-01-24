﻿namespace EA.Iws.Web.Areas.Reports.ViewModels.FreedomOfInformation
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.Reports;
    using Core.WasteType;
    using Infrastructure.Validation;
    using Prsd.Core.Helpers;
    using Web.ViewModels.Shared;

    public class IndexViewModel : IValidatableObject
    {
        public IndexViewModel()
        {
            FromDate = new OptionalDateInputViewModel(true);
            ToDate = new OptionalDateInputViewModel(true);

            var chemicalCompositions = EnumHelper.GetValues(typeof(ChemicalComposition));
            chemicalCompositions.Add(0, "View all");

            ChemicalCompositions = new SelectList(chemicalCompositions, "Key", "Value", null);
            DateSelectList = new SelectList(EnumHelper.GetValues(typeof(FoiReportDates)), "Key", "Value", null);
        }

        [Display(Name = "FromDate", ResourceType = typeof(IndexViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "FromDateRequired", ErrorMessageResourceType = typeof(IndexViewModelResources))]
        public OptionalDateInputViewModel FromDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(IndexViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "ToDateRequired", ErrorMessageResourceType = typeof(IndexViewModelResources))]
        public OptionalDateInputViewModel ToDate { get; set; }

        [Display(Name = "WasteType", ResourceType = typeof(IndexViewModelResources))]
        [Required(ErrorMessageResourceName = "WasteTypeRequired",
            ErrorMessageResourceType = typeof(IndexViewModelResources))]
        public ChemicalComposition? ChemicalComposition { get; set; }

        public SelectList ChemicalCompositions { get; set; }

        [Display(Name = "DateType", ResourceType = typeof(IndexViewModelResources))]
        [Required(ErrorMessageResourceName = "DateTypeRequired", ErrorMessageResourceType = typeof(IndexViewModelResources))]
        public FoiReportDates DateType { get; set; }

        public SelectList DateSelectList { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FromDate.AsDateTime() > ToDate.AsDateTime())
            {
                yield return new ValidationResult(IndexViewModelResources.FromDateBeforeToDate, new[] { "FromDate" });
            }
        }
    }
}
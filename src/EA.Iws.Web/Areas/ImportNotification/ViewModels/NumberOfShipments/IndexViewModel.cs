﻿namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.NumberOfShipments
{
    using System.ComponentModel.DataAnnotations;

    public class IndexViewModel
    {
        [Required(ErrorMessageResourceName = "NumberRequired", ErrorMessageResourceType = typeof(IndexViewModelResources))]
        [Display(Name = "Number", ResourceType = typeof(IndexViewModelResources))]
        [Range(1, int.MaxValue, ErrorMessage = null, ErrorMessageResourceName = "NumberIsInt", ErrorMessageResourceType = typeof(IndexViewModelResources))]
        public int? Number { get; set; }
    }
}
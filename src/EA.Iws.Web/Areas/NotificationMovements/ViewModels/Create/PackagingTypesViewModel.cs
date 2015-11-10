﻿namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Create
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Core.PackagingType;
    using Prsd.Core.Helpers;
    using Web.ViewModels.Shared;

    public class PackagingTypesViewModel
    {
        public CheckBoxCollectionViewModel PackagingTypes { get; set; }
        public IList<int> MovementNumbers { get; set; }

        public IList<PackagingType> SelectedValues
        {
            get
            {
                return PackagingTypes
                    .PossibleValues
                    .Where(x => x.Selected)
                    .Select(x => (PackagingType)System.Convert.ToInt32(x.Value))
                    .ToList();
            }
        }

        public PackagingTypesViewModel()
        {
        }

        public PackagingTypesViewModel(PackagingData availablePackagingTypes, IList<int> movementNumbers)
        {
            MovementNumbers = movementNumbers;

            var items = availablePackagingTypes.PackagingTypes
                .Where(x => x != PackagingType.Other)
                .Select(x => new SelectListItem
                {
                    Text = EnumHelper.GetDisplayName(x),
                    Value = ((int)x).ToString()
                })
                .ToList();

            if (availablePackagingTypes.PackagingTypes.Contains(PackagingType.Other))
            {
                items.Add(new SelectListItem
                {
                    Text = string.Format("{0} - {1}", EnumHelper.GetShortName(PackagingType.Other), 
                        availablePackagingTypes.OtherDescription),
                    Value = ((int)PackagingType.Other).ToString()
                });
            }

            PackagingTypes = new CheckBoxCollectionViewModel();
            PackagingTypes.ShowEnumValue = true;
            PackagingTypes.PossibleValues = items;
        }
    }
}
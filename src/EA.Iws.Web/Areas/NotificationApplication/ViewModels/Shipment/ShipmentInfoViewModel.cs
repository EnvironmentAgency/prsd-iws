﻿namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Shipment
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using Core.IntendedShipments;
    using Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Helpers;
    using Requests.IntendedShipments;
    using Services;

    public class ShipmentInfoViewModel : IValidatableObject
    {
        public ShipmentInfoViewModel()
        {
            UnitsSelectList = new SelectList(EnumHelper.GetValues(typeof(ShipmentQuantityUnits)), "Key", "Value");
        }

        public ShipmentInfoViewModel(IntendedShipmentData intendedShipmentData)
        {
            NotificationId = intendedShipmentData.NotificationId;
            UnitsSelectList = new SelectList(EnumHelper.GetValues(typeof(ShipmentQuantityUnits)), "Key", "Value");
            IsPreconsentedRecoveryFacility = intendedShipmentData.IsPreconsentedRecoveryFacility;

            if (intendedShipmentData.HasShipmentData)
            {
                EndDay = intendedShipmentData.LastDate.Day;
                EndMonth = intendedShipmentData.LastDate.Month;
                EndYear = intendedShipmentData.LastDate.Year;
                NumberOfShipments = intendedShipmentData.NumberOfShipments.ToString();
                Quantity = intendedShipmentData.Quantity.ToString();
                StartDay = intendedShipmentData.FirstDate.Day;
                StartMonth = intendedShipmentData.FirstDate.Month;
                StartYear = intendedShipmentData.FirstDate.Year;
                Units = intendedShipmentData.Units;
            }
        }

        public Guid NotificationId { get; set; }

        public bool IsPreconsentedRecoveryFacility { get; set; }

        [Required(ErrorMessage = "Please enter the total number of intended shipments")]
        [Display(Name = "Number of shipments")]
        public string NumberOfShipments { get; set; }

        [Required(ErrorMessage = "Please enter the total intended quantity")]
        public string Quantity { get; set; }

        [Required(ErrorMessage = "Please select the units.")]
        public ShipmentQuantityUnits? Units { get; set; }

        public IEnumerable<SelectListItem> UnitsSelectList { get; set; }

        [Required(ErrorMessage = "Please enter a valid number in the 'Day' field")]
        [Display(Name = "Day")]
        [Range(1, 31, ErrorMessage = "Please enter a valid number in the 'Day' field")]
        public int? StartDay { get; set; }

        [Required(ErrorMessage = "Please enter a valid number in the 'Month' field")]
        [Display(Name = "Month")]
        [Range(1, 12, ErrorMessage = "Please enter a valid number in the 'Month' field")]
        public int? StartMonth { get; set; }

        [Required(ErrorMessage = "Please enter a valid number in the 'Year' field")]
        [Display(Name = "Year")]
        [Range(2015, 3000, ErrorMessage = "Please enter a valid number in the 'Year' field")]
        public int? StartYear { get; set; }

        [Required(ErrorMessage = "Please enter a valid number in the 'Day' field")]
        [Display(Name = "Day")]
        [Range(1, 31, ErrorMessage = "Please enter a valid number in the 'Day' field")]
        public int? EndDay { get; set; }

        [Required(ErrorMessage = "Please enter a valid number in the 'Month' field")]
        [Display(Name = "Month")]
        [Range(1, 12, ErrorMessage = "Please enter a valid number in the 'Month' field")]
        public int? EndMonth { get; set; }

        [Required(ErrorMessage = "Please enter a valid number in the 'Year' field")]
        [Display(Name = "Year")]
        [Range(2015, 3000, ErrorMessage = "Please enter a valid number in the 'Year' field")]
        public int? EndYear { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IsNumberOfShipmentsValid())
            {
                yield return new ValidationResult("Please enter a valid number between 1 and 99999", new[] { "NumberOfShipments" });                
            }

            if (!IsQuantityValid())
            {
                yield return new ValidationResult("Please enter a valid number with a maximum of 4 decimal places", new[] { "Quantity" });
            }

            DateTime startDate;
            bool isValidStartDate = SystemTime.TryParse(StartYear.GetValueOrDefault(), StartMonth.GetValueOrDefault(), StartDay.GetValueOrDefault(), out startDate);
            if (!isValidStartDate)
            {
                yield return new ValidationResult("Please enter a valid first departure date", new[] { "StartDay" });
            }

            DateTime endDate;
            bool isValidEndDate = SystemTime.TryParse(EndYear.GetValueOrDefault(), EndMonth.GetValueOrDefault(), EndDay.GetValueOrDefault(), out endDate);
            if (!isValidEndDate)
            {
                yield return new ValidationResult("Please enter a valid last departure date", new[] { "EndDay" });
            }

            if (!(isValidStartDate && isValidEndDate))
            {
                // Stop further validation if either date is not a valid date
                yield break;
            }

            if (startDate < SystemTime.Now.Date)
            {
                yield return new ValidationResult("The first departure date cannot be in the past");
            }

            if (startDate > endDate)
            {
                yield return new ValidationResult("The first departure date must be before the last departure date", new[] { "StartYear" });
            }

            var monthPeriodLength = IsPreconsentedRecoveryFacility ? 36 : 12;
            if (endDate >= startDate.AddMonths(monthPeriodLength))
            {
                yield return
                    new ValidationResult(
                        string.Format("The first departure date and last departure date must be within a {0} month period",
                            monthPeriodLength), new[] { "EndYear" });
            }
        }

        private bool IsNumberOfShipmentsValid()
        {
            return ViewModelService.IsNumberOfShipmentsValid(NumberOfShipments);
        }

        private bool IsQuantityValid()
        {
            return ViewModelService.IsStringValidDecimalToFourDecimalPlaces(Quantity);
        }

        public SetIntendedShipmentInfoForNotification ToRequest()
        {
            DateTime startDate;
            SystemTime.TryParse(StartYear.GetValueOrDefault(), StartMonth.GetValueOrDefault(), StartDay.GetValueOrDefault(), out startDate);

            DateTime endDate;
            SystemTime.TryParse(EndYear.GetValueOrDefault(), EndMonth.GetValueOrDefault(), EndDay.GetValueOrDefault(), out endDate);

            return new SetIntendedShipmentInfoForNotification(
                NotificationId,
                Convert.ToInt32(NumberOfShipments),
                Convert.ToDecimal(Quantity),
                Units.GetValueOrDefault(),
                startDate,
                endDate);
        }
    }
}
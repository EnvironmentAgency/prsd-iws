﻿namespace EA.Iws.Web.Areas.Admin.ViewModels.ExportNotification
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Notification;
    using Core.Shared;

    public class EnterNumberViewModel : IValidatableObject
    {
        public UKCompetentAuthority CompetentAuthority { get; set; }

        public NotificationType NotificationType { get; set; }

        [Display(Name = "Notification number")]
        [Required(ErrorMessage = "Please enter the notification number")]
        public int? Number { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Number <= 0)
            {
                yield return new ValidationResult("The notification number must be greater than or equal to 1", new[] { "Number" });
            }

            if (CompetentAuthority == UKCompetentAuthority.England && Number >= 6000)
            {
                yield return new ValidationResult("The notification number must be less than 6000", new[] { "Number" });
            }
            else if (CompetentAuthority == UKCompetentAuthority.Scotland && Number >= 500)
            {
                yield return new ValidationResult("The notification number must be less than 500", new[] { "Number" });
            }
            else if (CompetentAuthority == UKCompetentAuthority.NorthernIreland && Number >= 1000)
            {
                yield return new ValidationResult("The notification number must be less than 1000", new[] { "Number" });
            }
            else if (CompetentAuthority == UKCompetentAuthority.Wales && Number >= 100)
            {
                yield return new ValidationResult("The notification number must be less than 100", new[] { "Number" });
            }
        }
    }
}
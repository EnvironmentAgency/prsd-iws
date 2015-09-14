﻿namespace EA.Iws.Web.Areas.Movement.ViewModels.ShipmentAcceptance
{
    using Requests.MovementReceipt;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.MovementReceipt;

    public class ShipmentAcceptanceViewModel : IValidatableObject
    {
        public Decision? Decision { get; set; }
         
        public string RejectReason { get; set; }

        public Guid MovementId { get; set; }

        public ShipmentAcceptanceViewModel()
        {
        }

        public ShipmentAcceptanceViewModel(MovementAcceptanceData acceptanceData)
        {
            Decision = acceptanceData.Decision;
            RejectReason = acceptanceData.RejectionReason;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Decision == null)
            {
                yield return new ValidationResult("Please answer this question");
            }
            else if (Decision == Core.MovementReceipt.Decision.Rejected && string.IsNullOrWhiteSpace(RejectReason))
            {
                yield return new ValidationResult("Please enter the reason the shipment was rejected", new[] { "RejectReason" });
            }
            else if (Decision == Core.MovementReceipt.Decision.Rejected && RejectReason.Count() > 200)
            {
                yield return new ValidationResult("Reason for rejection cannot be longer than 200 characters", new[] { "RejectReason" });
            }
        }
    }
}
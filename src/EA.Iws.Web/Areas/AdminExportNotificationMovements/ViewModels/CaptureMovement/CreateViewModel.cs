﻿namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.CaptureMovement
{
    using Core.Shared;
    using Prsd.Core;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.Movement;
    using Web.ViewModels.Shared;

    public class CreateViewModel : IValidatableObject
    {
        [Required(ErrorMessageResourceName = "NumberRequired", ErrorMessageResourceType = typeof(CreateViewModelResources))]
        [Display(Name = "Number", ResourceType = typeof(CreateViewModelResources))]
        [Range(1, int.MaxValue, ErrorMessage = null, ErrorMessageResourceName = "NumberIsInt", ErrorMessageResourceType = typeof(CreateViewModelResources))]
        public int? ShipmentNumber { get; set; }

        public ReceiptViewModel Receipt { get; set; }

        public RecoveryViewModel Recovery { get; set; }

        [Display(Name = "PrenotificationDateLabel", ResourceType = typeof(CreateViewModelResources))]
        public MaskedDateInputViewModel PrenotificationDate { get; set; }

        [Display(Name = "ActualDateLabel", ResourceType = typeof(CreateViewModelResources))]
        public MaskedDateInputViewModel ActualShipmentDate { get; set; }

        [Display(Name = "HasNoPrenotification", ResourceType = typeof(CreateViewModelResources))]
        public bool HasNoPrenotification { get; set; }

        public NotificationType NotificationType { get; set; }

        public bool IsReceived { get; set; }

        public bool IsOperationCompleted { get; set; }

        public bool IsRejected { get; set; }

        [Display(Name = "HasComments", ResourceType = typeof(CreateViewModelResources))]
        public bool HasComments { get; set; }

        [Display(Name = "Comments", ResourceType = typeof(CreateViewModelResources))]
        public string Comments { get; set; }

        [Display(Name = "StatsMarking", ResourceType = typeof(CreateViewModelResources))]
        public string StatsMarking { get; set; }

        public SelectList StatsMarkingSelectList
        {
            get
            {
                return new SelectList(new[]
                {
                    "Illegal Shipment (WSR Table 5)",
                    "Did not proceed as intended (Basel Table 9)",
                    "Accident occurred during transport (Basel Table 10)"
                });
            }
        }

        public CreateViewModel()
        {
            PrenotificationDate = new MaskedDateInputViewModel();
            ActualShipmentDate = new MaskedDateInputViewModel();
            Receipt = new ReceiptViewModel();
            Recovery = new RecoveryViewModel();
        }

        public CreateViewModel(MovementReceiptAndRecoveryData data)
        {
            ActualShipmentDate = new MaskedDateInputViewModel(data.ActualDate);
            if (data.PrenotificationDate.HasValue)
            {
                PrenotificationDate = new MaskedDateInputViewModel(data.PrenotificationDate.Value);
            }
            else
            {
                PrenotificationDate = new MaskedDateInputViewModel();
                HasNoPrenotification = true;
            }

            ShipmentNumber = data.Number;

            Comments = data.Comments;
            StatsMarking = data.StatsMarking;

            if (!string.IsNullOrWhiteSpace(data.Comments) || !string.IsNullOrWhiteSpace(data.StatsMarking))
            {
                HasComments = true;
            }

            NotificationType = data.NotificationType;
            IsReceived = data.IsReceived;
            IsOperationCompleted = data.IsOperationCompleted;
            IsRejected = data.IsRejected;

            Receipt = new ReceiptViewModel
            {
                ActualQuantity = data.ActualQuantity,
                ReceivedDate = new MaskedDateInputViewModel(data.ReceiptDate),
                Units = data.ReceiptUnits ?? data.NotificationUnits,
                WasShipmentAccepted = string.IsNullOrWhiteSpace(data.RejectionReason),
                RejectionReason = data.RejectionReason,
                PossibleUnits = data.PossibleUnits
            };

            Recovery = new RecoveryViewModel
            {
                NotificationType = data.NotificationType,
                RecoveryDate = new MaskedDateInputViewModel(data.OperationCompleteDate)
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ActualShipmentDate.IsCompleted)
            {
                yield return new ValidationResult(CreateViewModelResources.ActualDateRequired, new[] { "ActualShipmentDate" });
            }

            if (!HasNoPrenotification && !PrenotificationDate.IsCompleted)
            {
                yield return new ValidationResult(CreateViewModelResources.PrenotificationDateRequired, 
                    new[] { "PrenotificationDate" });
            }
          
            if ((!Receipt.ReceivedDate.IsCompleted && Receipt.ActualQuantity.HasValue) || (!Receipt.ReceivedDate.IsCompleted && !string.IsNullOrWhiteSpace(Receipt.RejectionReason)))
            {
                yield return new ValidationResult(CreateViewModelResources.ReceivedDateRequired, new[] { "Receipt.ReceivedDate" });
            }

            if (!Receipt.ActualQuantity.HasValue && Receipt.ReceivedDate.IsCompleted && Receipt.WasShipmentAccepted)
            {
                yield return new ValidationResult(CreateViewModelResources.QuantityRequired, new[] { "Receipt.ActualQuantity" });
            }

            if (!Receipt.WasShipmentAccepted && string.IsNullOrWhiteSpace(Receipt.RejectionReason))
            {
                yield return new ValidationResult(CreateViewModelResources.RejectReasonRequired, new[] { "Receipt.RejectionReason" });
            }

            if (Recovery.IsComplete() && !Receipt.IsComplete())
            {
                yield return new ValidationResult(string.Format(CreateViewModelResources.ReceiptMustBeCompletedFirst, NotificationType),
                    new[] { "Recovery.RecoveryDate" });
            }

            if (Receipt.IsComplete() && !Receipt.WasShipmentAccepted && Recovery.IsComplete())
            {
                yield return new ValidationResult(string.Format(CreateViewModelResources.RecoveryDateCannotBeEnteredForRejected, NotificationType),
                    new[] { "Recovery.RecoveryDate" });
            }

            if (PrenotificationDate.IsCompleted && PrenotificationDate.Date > SystemTime.UtcNow.Date)
            {
                yield return new ValidationResult(CreateViewModelResources.PrenotifictaionDateInfuture,
                   new[] { "PrenotificationDate" });
            }

            if (ActualShipmentDate.IsCompleted && PrenotificationDate.IsCompleted)
            {
                DateTime preNotificateDate = PrenotificationDate.Date.Value;

                if (ActualShipmentDate.Date < preNotificateDate)
                {
                    yield return new ValidationResult(CreateViewModelResources.ActualDateBeforePrenotification, new[] { "ActualShipmentDate" });
                }

                if (ActualShipmentDate.Date > preNotificateDate.AddDays(60))
                {
                    yield return new ValidationResult(CreateViewModelResources.ActualDateGreaterthanSixtyDays, new[] { "ActualShipmentDate" });
                }
            }

            if (Receipt.IsComplete())
            {
                if (Receipt.ReceivedDate.Date < ActualShipmentDate.Date)
                {
                    yield return new ValidationResult(CreateViewModelResources.ReceivedDateBeforeActualDate, new[] { "Receipt.ReceivedDate" });
                }
                if (Receipt.ReceivedDate.Date > SystemTime.UtcNow.Date)
                {
                    yield return new ValidationResult(CreateViewModelResources.ReceivedDateInfuture, new[] { "Receipt.ReceivedDate" });
                }
            }

            if (Recovery.IsComplete())
            {
                if (Recovery.RecoveryDate.Date < Receipt.ReceivedDate.Date)
                {
                    yield return new ValidationResult(string.Format(CreateViewModelResources.RecoveredDateBeforeReceivedDate, GetNotificationTypeVerb(Recovery.NotificationType)), new[] { "Recovery.RecoveryDate" });
                }
                if (Recovery.RecoveryDate.Date > SystemTime.UtcNow.Date)
                {
                    yield return new ValidationResult(string.Format(CreateViewModelResources.RecoveredDateInfuture, GetNotificationTypeVerb(Recovery.NotificationType)), new[] { "Recovery.RecoveryDate" });
                }
            }
        }

        private static string GetNotificationTypeVerb(NotificationType displayedType)
        {
            return displayedType == NotificationType.Recovery ? "recovered" : "disposed of";
        }
    }
}
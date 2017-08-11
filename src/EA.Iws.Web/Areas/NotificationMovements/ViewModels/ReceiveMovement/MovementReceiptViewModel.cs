﻿namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.ReceiveMovement
{
    using Core.Movement;
    using Core.MovementOperation;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class MovementReceiptViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public MovementReceiptViewModel()
        {
        }

        public IList<SelectShipmentViewModel> ReceiveShipments { get; set; }

        public IList<SelectShipmentViewModel> RecoveryShipments { get; set; }

        public MovementReceiptViewModel(Guid id, IEnumerable<MovementData> receiveModel, MovementOperationData recoveryModel)
        {
            ReceiveShipments = receiveModel
               .OrderBy(d => d.Number)
               .Select(d => new SelectShipmentViewModel
               {
                   DisplayName = "Shipment " + d.Number,
                   Id = d.Id,
                   IsSelected = false
               }).ToArray();
            NotificationId = id;

            var list = recoveryModel.MovementDatas;
            RecoveryShipments = list
               .OrderBy(d => d.Number)
               .Select(d => new SelectShipmentViewModel
               {
                   DisplayName = "Shipment " + d.Number,
                   Id = d.Id,
                   IsSelected = false
               }).ToArray();
            NotificationId = id;
        }

        public class SelectShipmentViewModel
        {
            public string DisplayName { get; set; }

            public Guid Id { get; set; }

            public bool IsSelected { get; set; }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ReceiveShipments.Any(s => s.IsSelected) && !RecoveryShipments.Any(s => s.IsSelected))
            {
                yield return new ValidationResult(MovementReceiptViewModelResources.ShipmentRequired);
            }
            if ((ReceiveShipments != null && ReceiveShipments.Any(s => s.IsSelected)) && (RecoveryShipments != null && RecoveryShipments.Any(s => s.IsSelected)))
            {
                yield return new ValidationResult(MovementReceiptViewModelResources.EitherReceiveOrRecovery);
            }
        }
    }
}
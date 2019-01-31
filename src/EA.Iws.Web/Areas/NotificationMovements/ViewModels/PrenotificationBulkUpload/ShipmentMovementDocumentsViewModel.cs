﻿namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.PrenotificationBulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using Views.PrenotificationBulkUpload;

    public class ShipmentMovementDocumentsViewModel : IValidatableObject
    {
        public ShipmentMovementDocumentsViewModel()
        {
            this.Shipments = new List<int?>();
        }

        public ShipmentMovementDocumentsViewModel(Guid notificationId, List<int?> shipments, string preNotificationFileName)
        {
            this.NotificationId = notificationId;
            this.Shipments = shipments;
            this.PreNotificationFileName = preNotificationFileName;
            this.FileSuccessMessage = PrenotificationBulkUploadResources.ShipmentMovementsSuccessText.Replace("{filename}", preNotificationFileName);
        }

        public Guid NotificationId { get; set; }

        public List<int?> Shipments { get; set; }

        public string PreNotificationFileName { get; set; }

        public string ShipmentMovementFileName
        {
            get
            {
                return File.FileName;
            }
        }

        public string FileSuccessMessage { get; set; }

        [Display(Name = "Upload the file containing your shipment movement documents")]
        public HttpPostedFileBase File { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (File == null || File.InputStream.Length == 0)
            {
                yield return new ValidationResult("Upload the file containing your shipment movement documents", new[] { "File" });
            }
        }

        public string GetShipments
        {
            get
            {
                string returnString = string.Empty;

                for (int i = 0; i < Shipments.Count; i++)
                {
                    returnString += Shipments[i].ToString() + ", ";
                    if (i == Shipments.Count - 2)
                    {
                        if (Shipments.Count.Equals(2))
                        {
                            returnString = returnString.Trim().TrimEnd(',');
                        }
                        returnString = string.Concat(returnString.Trim(), " and ");
                    }
                }

                return returnString.Trim().TrimEnd(',');
            }
        }
    }
}
﻿namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Carrier
{
    using System;
    using System.Collections.Generic;
    using Core.Carriers;

    public class CarrierListViewModel
    {
        public Guid NotificationId { get; set; }

        public IList<CarrierData> Carriers { get; set; }
    }
}
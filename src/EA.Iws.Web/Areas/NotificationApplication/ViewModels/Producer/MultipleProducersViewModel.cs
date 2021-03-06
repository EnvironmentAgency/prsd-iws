﻿namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Producer
{
    using System;
    using System.Collections.Generic;
    using Core.Producers;

    public class MultipleProducersViewModel
    {
        public MultipleProducersViewModel()
        {
            ProducerData = new List<ProducerData>();
        }

        public Guid NotificationId { get; set; }

        public IList<ProducerData> ProducerData { get; set; }
    }
}
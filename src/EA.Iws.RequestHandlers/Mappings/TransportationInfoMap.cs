﻿namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Carriers;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.Notification.Overview;

    internal class TransportationInfoMap : IMap<NotificationApplication, Transportation>
    {
        private readonly IMap<NotificationApplication, IList<CarrierData>> carrierMap;

        public TransportationInfoMap(
            IMap<NotificationApplication, IList<CarrierData>> carrierMap)
        {
            this.carrierMap = carrierMap;
        }

        public Transportation Map(NotificationApplication notification)
        {
            return new Transportation 
            {
                NotificationId = notification.Id,
                Carriers = carrierMap.Map(notification).ToList(),
                MeanOfTransport = notification.MeansOfTransport.ToList(),
                PackagingData = GetPackagingData(notification),
                SpecialHandlingDetails = GetSpecialHandling(notification)
            };
        }

        private static List<string> GetPackagingData(NotificationApplication notification)
        {
            var packagingData = new List<string>();
            foreach (var packagingInfo in notification.PackagingInfos)
            {
                packagingData.Add(packagingInfo.PackagingType != PackagingType.Other
                    ? packagingInfo.PackagingType.Value + " - " + packagingInfo.PackagingType.DisplayName
                    : packagingInfo.OtherDescription);
            }
            packagingData.Sort();

            return packagingData;
        }
        
        private static string GetSpecialHandling(NotificationApplication notification)
        {
            var specialHandlingAnswer = string.Empty;
            if (notification.HasSpecialHandlingRequirements.HasValue)
            {
                specialHandlingAnswer = notification.HasSpecialHandlingRequirements.Value
                    ? notification.SpecialHandlingDetails
                    : "No special handling required";
            }
            return specialHandlingAnswer;
        }
    }
}

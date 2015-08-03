﻿namespace EA.Iws.Core.Notification
{
    using System;
    using NotificationAssessment;

    public class SubmitSummaryData
    {
        public CompetentAuthority CompetentAuthority { get; set; }

        public string NotificationNumber { get; set; }

        public DateTime CreatedDate { get; set; }

        public NotificationStatus Status { get; set; }

        public int Charge { get; set; }
    }
}

﻿namespace EA.Iws.Core.NotificationAssessment
{
    using System.ComponentModel.DataAnnotations;

    public enum NotificationStatus
    {
        [Display(Name = "Not submitted")]
        NotSubmitted = 1,
        [Display(Name = "Submitted")]
        Submitted = 2,
        [Display(Name = "Notification received")]
        NotificationReceived = 3,
        [Display(Name = "In assessment")]
        InAssessment = 4
    }
}
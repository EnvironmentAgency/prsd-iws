﻿namespace EA.Iws.Web.ViewModels.ShareNotification
{
    using Core.Notification;
    using System;
    using System.Collections.Generic;

    public class SharedUserListViewModel
    {
        public Guid NotificationId { get; set; }

        public List<NotificationSharedUser> SharedUsers { get; set; }

        public SharedUserListViewModel()
        {
        }
    }
}
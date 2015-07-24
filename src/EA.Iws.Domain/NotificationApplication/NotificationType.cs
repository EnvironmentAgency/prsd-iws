﻿namespace EA.Iws.Domain.NotificationApplication
{
    using Prsd.Core.Domain;

    public class NotificationType : Enumeration
    {
        public static readonly NotificationType Recovery = new NotificationType(1, "Recovery");
        public static readonly NotificationType Disposal = new NotificationType(2, "Disposal");

        protected NotificationType()
        {
        }

        private NotificationType(int value, string displayName)
            : base(value, displayName)
        {
        }
    }
}
﻿namespace EA.Iws.Domain.Notification
{
    using System;

    public class WasteCode
    {
        protected WasteCode()
        {
        }

        public Guid Id { get; private set; }

        public string Description { get; private set; }

        public string Code { get; private set; }

        public CodeType CodeType { get; private set; }
    }
}
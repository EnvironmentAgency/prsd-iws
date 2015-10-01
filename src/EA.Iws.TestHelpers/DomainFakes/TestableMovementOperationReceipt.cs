﻿namespace EA.Iws.TestHelpers.DomainFakes
{
    using Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Domain.Movement;

    public class TestableMovementOperationReceipt : MovementOperationReceipt
    {
        public new DateTime Date
        {
            get { return base.Date; }
            set { ObjectInstantiator<MovementOperationReceipt>.SetProperty(x => x.Date, value, this); }
        }
    }
}

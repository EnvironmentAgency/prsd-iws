namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.FinancialGuarantee
{
    using System;
    using Core.FinancialGuarantee;
    using DataAccess;
    using Domain.FinancialGuarantee;
    using TestHelpers.Helpers;

    public class FinancialGuaranteeDecisionTests
    {
        protected static readonly DateTime FirstDate = new DateTime(2015, 1, 1);
        protected static readonly DateTime MiddleDate = new DateTime(2015, 1, 16);
        protected static readonly DateTime LastDate = new DateTime(2015, 2, 1);

        protected static readonly Guid ApplicationCompletedId = new Guid("9DA0EE37-EA13-4DF8-A4C9-0A8FDBC2207B");
        protected static readonly Guid FinancialGuaranteeId = new Guid("9837EB9C-5F52-4D8D-A347-3CF57B2ED4FE");

        protected IwsContext context;

        protected class TestFinancialGuaranteeCollection : FinancialGuaranteeCollection
        {
            public TestFinancialGuaranteeCollection(Guid notificationId)
                : base(notificationId)
            {
            }

            public void AddExistingFinancialGuarantee(FinancialGuarantee financialGuarantee)
            {
                FinancialGuaranteesCollection.Add(financialGuarantee);
            }
        }

        protected class TestFinancialGuarantee : FinancialGuarantee
        {
            public TestFinancialGuarantee(Guid financialGuaranteeId)
            {
                EntityHelper.SetEntityId(this, financialGuaranteeId);
            }

            public bool RejectThrows { get; set; }

            public bool RefuseCalled { get; set; }

            public bool RefuseThrows { get; set; }

            public bool ReleaseCalled { get; set; }

            public new DateTime? CompletedDate
            {
                get { return base.CompletedDate; }
                set { ObjectInstantiator<FinancialGuarantee>.SetProperty(x => x.CompletedDate, value, this); }
            }

            public override void Refuse(DateTime decisionDate, string refusalReason)
            {
                RefuseCalled = true;

                if (RejectThrows)
                {
                    throw new InvalidOperationException();
                }

                Status = FinancialGuaranteeStatus.Refused;
            }

            public override void Release(DateTime decisionDate)
            {
                ReleaseCalled = true;

                if (RefuseThrows)
                {
                    throw new InvalidOperationException();
                }

                Status = FinancialGuaranteeStatus.Released;
            }

            public void SetStatus(FinancialGuaranteeStatus status)
            {
                Status = status;
            }
        }
    }
}
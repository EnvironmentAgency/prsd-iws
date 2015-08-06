﻿namespace EA.Iws.Domain.Tests.Unit.FinancialGuarantee
{
    using System;
    using Core.FinancialGuarantee;
    using Domain.FinancialGuarantee;
    using Xunit;

    public class FinancialGuaranteeReleaseTests : FinancialGuaranteeTests
    {
        private readonly Action<FinancialGuarantee> releaseGuarantee =
            guarantee => guarantee.Release(AfterCompletionDate);
        
        [Fact]
        public void StateNotCompletedThrows()
        {
            Assert.Throws<InvalidOperationException>(() => releaseGuarantee(ReceivedFinancialGuarantee));
        }

        [Fact]
        public void DecisionDateBeforeCompletionDateThrows()
        {
            Assert.Throws<InvalidOperationException>(() => CompletedFinancialGuarantee.Release(BeforeCompletionDate));
        }

        [Fact]
        public void SetsDecisionDate()
        {
            CompletedFinancialGuarantee.Release(AfterCompletionDate);

            Assert.Equal(AfterCompletionDate, CompletedFinancialGuarantee.DecisionDate);
        }

        [Fact]
        public void SetsStatus()
        {
            releaseGuarantee(CompletedFinancialGuarantee);

            Assert.Equal(FinancialGuaranteeStatus.Released, CompletedFinancialGuarantee.Status);
        }

        [Fact]
        public void CanReleaseAnApprovedGuarantee()
        {
            ApprovedFinancialGuarantee.Release(AfterCompletionDate);

            Assert.Equal(FinancialGuaranteeStatus.Released, ApprovedFinancialGuarantee.Status);
        }

        [Fact]
        public void CanReleaseARefusedGuarantee()
        {
            RefusedFinancialGuarantee.Release(AfterCompletionDate);

            Assert.Equal(FinancialGuaranteeStatus.Released, RefusedFinancialGuarantee.Status);
        }
    }
}

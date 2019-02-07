﻿namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Domain;
    using Domain.Movement;
    using FakeItEasy;
    using RequestHandlers.NotificationMovements.BulkReceiptRecovery;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class ReceiptRecoveryRecoverOnlyRuleTests
    {
        private readonly ReceiptRecoveryRecoveryOnlyRule rule;
        private readonly Guid notificationId;
        private readonly IMovementRepository repo;

        public ReceiptRecoveryRecoverOnlyRuleTests()
        {
            repo = A.Fake<IMovementRepository>();
            rule = new ReceiptRecoveryRecoveryOnlyRule(repo);
            notificationId = Guid.NewGuid();
        }

        [Fact]
        public async Task RecoverOnly_Received_Success()
        {
            A.CallTo(() => repo.GetAllMovements(notificationId)).Returns(GetRepoMovements(true));
            var result = await rule.GetResult(GetTestData(false), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.RecoveredValidation, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Fact]
        public async Task RecoverOnly_NotReceived_Failure()
        {
            A.CallTo(() => repo.GetAllMovements(notificationId)).Returns(GetRepoMovements(false));
            var result = await rule.GetResult(GetTestData(true), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.RecoveredValidation, result.Rule);
            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }

        private List<ReceiptRecoveryMovement> GetTestData(bool received)
        {
            return new List<ReceiptRecoveryMovement>()
            {
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 1,
                    MissingReceivedDate = received
                },
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 2
                }
            };
        }

        private List<Movement> GetRepoMovements(bool received)
        {
            return new List<Movement>()
            {
             new TestableMovement()
                {
                    NotificationId = notificationId,
                    Status = received ? Core.Movement.MovementStatus.Received : Core.Movement.MovementStatus.Submitted,
                    Number = 1
                },

            new TestableMovement()
                {
                    NotificationId = notificationId,
                    Status = Core.Movement.MovementStatus.Submitted,
                    Number = 2
                }
            };
        }
    }
}

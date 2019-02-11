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

    public class ReceiptRecoveryShipmentMustBePrenotifiedRuleTests
    {
        private readonly ReceiptRecoveryShipmentMustBePrenotifiedRule rule;
        private readonly Guid notificationId;
        private readonly IMovementRepository repo;

        public ReceiptRecoveryShipmentMustBePrenotifiedRuleTests()
        {
            repo = A.Fake<IMovementRepository>();
            rule = new ReceiptRecoveryShipmentMustBePrenotifiedRule(repo);
            notificationId = Guid.NewGuid();
        }

        [Fact]
        public async Task ShipmentsArePrenotified_Success()
        {
            A.CallTo(() => repo.GetAllMovements(notificationId)).Returns(GetRepoMovements(true, DateTime.Now));
            var result = await rule.GetResult(GetTestData(), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.ReceivedRecoveredValidation, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Fact]
        public async Task ShipmentNotPrenotified_CapturedButDateInPast_Failure()
        {
            A.CallTo(() => repo.GetAllMovements(notificationId)).Returns(GetRepoMovements(false, DateTime.Now.AddDays(-1)));
            var result = await rule.GetResult(GetTestData(), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.ReceivedRecoveredValidation, result.Rule);
            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }

        [Fact]
        public async Task ShipmentNotPrenotified_CapturedButDateInFuture_Success()
        {
            A.CallTo(() => repo.GetAllMovements(notificationId)).Returns(GetRepoMovements(false, DateTime.Now));
            var result = await rule.GetResult(GetTestData(), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.ReceivedRecoveredValidation, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        private List<ReceiptRecoveryMovement> GetTestData()
        { 
            return new List<ReceiptRecoveryMovement>()
            {
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 1,
                    ReceivedDate = DateTime.Now,
                    RecoveredDisposedDate = DateTime.Now
                },
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 2,
                    ReceivedDate = DateTime.Now,
                    RecoveredDisposedDate = DateTime.Now
                }
            };
        }

        private List<Movement> GetRepoMovements(bool prenotified, DateTime shipmentDate)
        {
            return new List<Movement>()
            {
             new TestableMovement()
                {
                    NotificationId = notificationId,
                    Status = prenotified ? Core.Movement.MovementStatus.Submitted : Core.Movement.MovementStatus.Captured,
                    Date = shipmentDate,
                    Number = 1
                },

            new TestableMovement()
                {
                    NotificationId = notificationId,
                    Status = Core.Movement.MovementStatus.Submitted,
                    Date = shipmentDate,
                    Number = 2
                }
            };
        }
    }
}

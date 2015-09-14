﻿namespace EA.Iws.Domain.Tests.Unit.MovementReceipt
{
    using Core.MovementReceipt;
    using Domain.Movement;
    using Domain.MovementReceipt;
    using Domain.NotificationApplication;
    using TestHelpers.Helpers;
    using System;
    using Core.Shared;
    using Xunit;
    using NotificationType = Domain.NotificationApplication.NotificationType;

    public class MovementReceiptTests
    {
        private readonly Movement movement;
        private readonly MovementReceipt movementReceipt;

        private static readonly DateTime AnyDate = new DateTime(2015, 1, 1);
        private static readonly DateTime MovementDate = new DateTime(2015, 12, 1);
        private static readonly DateTime BeforeMovementDate = new DateTime(2015, 10, 1);
        private static readonly DateTime AfterMovementDate = new DateTime(2016, 1, 1);
        private static readonly string AnyString = "text";

        public MovementReceiptTests()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery, UKCompetentAuthority.England, 0);
            movement = new Movement(notification, 1);
            ObjectInstantiator<Movement>.SetProperty(x => x.Date, MovementDate, movement);
            movementReceipt = new MovementReceipt(AfterMovementDate);
        }

        [Fact]
        public void CanReceiveMovementWithDateProvided()
        {
            movement.Receive(AfterMovementDate);

            Assert.NotNull(movement.Receipt);
            Assert.Equal(AfterMovementDate, movement.Receipt.Date);
        }

        [Fact]
        public void MovementReceivedBeforeMovementDate_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => movement.Receive(BeforeMovementDate));
        }
        
        [Fact]
        public void MovementNotYetActive_Throws()
        {
            ObjectInstantiator<Movement>.SetProperty(x => x.Date, null, movement);

            Assert.Throws<InvalidOperationException>(() => movement.Receive(AnyDate));
        }

        [Fact]
        public void AcceptThrowsIfNoReceipt()
        {
            Assert.Throws<InvalidOperationException>(() => movement.Accept());
        }

        [Fact]
        public void RejectThrowsIfNoReceipt()
        {
            Assert.Throws<InvalidOperationException>(() => movement.Reject(AnyString));
        }

        [Fact]
        public void CanAcceptMovement()
        {
            movement.Receive(AfterMovementDate);

            movement.Accept();

            Assert.NotNull(movement.Receipt.Decision);
            Assert.Equal(Decision.Accepted, movement.Receipt.Decision);
        }

        [Fact]
        public void CanRejectMovement()
        {
            movement.Receive(AfterMovementDate);

            movement.Reject(AnyString);

            Assert.NotNull(movement.Receipt.Decision);
            Assert.Equal(Decision.Rejected, movement.Receipt.Decision);
        }

        [Fact]
        public void RejectedMovementMustHaveReason()
        {
            movement.Receive(AfterMovementDate);

            movement.Reject(AnyString);

            Assert.Equal(AnyString, movement.Receipt.RejectReason);
        }

        [Fact]
        public void SetQuantity_ReceiptNotAccepted_Throws()
        {
            Action action =
                () => movementReceipt.SetQuantity(10m, ShipmentQuantityUnits.Kilograms, ShipmentQuantityUnits.Kilograms);

            Assert.Throws<InvalidOperationException>(action);
        }

        [Fact]
        public void SetQuantity_ReceiptAccepted_Sets()
        {
            ObjectInstantiator<MovementReceipt>.SetProperty(x => x.Decision, Decision.Accepted, movementReceipt);

            movementReceipt.SetQuantity(10m, ShipmentQuantityUnits.Kilograms, ShipmentQuantityUnits.Kilograms);

            Assert.Equal(10, movementReceipt.Quantity);
        }

        [Fact]
        public void SetQuantity_DisplayUnitsDifferentToNotificationUnits_SetsWithConversion()
        {
            ObjectInstantiator<MovementReceipt>.SetProperty(x => x.Decision, Decision.Accepted, movementReceipt);

            movementReceipt.SetQuantity(10m, ShipmentQuantityUnits.Tonnes, ShipmentQuantityUnits.Kilograms);

            Assert.Equal(10 * 1000, movementReceipt.Quantity);
        }

        [Fact]
        public void ReceiveMoreThanOnceUpdatesReceiptDate()
        {
            movement.Receive(AfterMovementDate);

            var newDate = AfterMovementDate.AddDays(1);

            movement.Receive(newDate);

            Assert.Equal(newDate, movement.Receipt.Date);
        }

        [Fact]
        public void ReceiveMoreThanOnceDoesNotCreateNewReceipt()
        {
            movement.Receive(AfterMovementDate);

            var newDate = AfterMovementDate.AddDays(1);

            movement.Accept();

            movement.Receive(newDate);

            Assert.NotNull(movement.Receipt.Decision);
        }
    }
}

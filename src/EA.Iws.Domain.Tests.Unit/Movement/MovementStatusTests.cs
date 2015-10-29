﻿namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Core.Movement;
    using Core.Shared;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using TestHelpers.Helpers;
    using Xunit;

    public class MovementStatusTests
    {
        private readonly Movement movement;
        private static readonly Guid AnyGuid = new Guid("B68806E3-524E-476E-A505-40B717B3191E");
        private static readonly DateTime AnyDate = new DateTime(2015, 1, 1);
        private const string AnyString = "test";

        public MovementStatusTests()
        {
            movement = CreateCompleteMovement();
        }

        private void SetMovementStatus(MovementStatus status, Movement movement)
        {
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, status, movement);
        }

        [Fact]
        public void DefaultStatusIsNew()
        {
            Assert.Equal(MovementStatus.New, movement.Status);
        }

        [Fact]
        public void SubmitChangesStatusToSubmitted()
        {
            movement.Submit(AnyGuid);

            Assert.Equal(MovementStatus.Submitted, movement.Status);
        }

        [Fact]
        public void CantSubmitIncompelteMovement()
        {
            var newMovement = new Movement(2, AnyGuid);

            Assert.Throws<InvalidOperationException>(() => newMovement.Submit(AnyGuid));
        }

        [Fact]
        public void CantSubmitTwice()
        {
            movement.Submit(AnyGuid);
            Action submitAgain = () => movement.Submit(AnyGuid);

            Assert.Throws<InvalidOperationException>(submitAgain);
        }

        [Fact]
        public void SubmitRaisesStatusChangedEvent()
        {
            movement.Submit(AnyGuid);

            Assert.Equal(movement,
                movement.Events.OfType<MovementStatusChangeEvent>()
                    .SingleOrDefault()
                    .Movement);
        }

        [Fact]
        public void CanCancelSubmittedMovement()
        {
            SetMovementStatus(MovementStatus.Submitted, movement);

            movement.Cancel();

            Assert.Equal(MovementStatus.Cancelled, movement.Status);
        }

        [Fact]
        public void CompleteChangesStatusToCompleted()
        {
            SetMovementStatus(MovementStatus.Received, movement);

            movement.Complete(AnyDate, AnyGuid);

            Assert.Equal(MovementStatus.Completed, movement.Status);
        }

        [Fact]
        public void CompleteCreatesMovementCompletedReceipt()
        {
            SetMovementStatus(MovementStatus.Received, movement);

            movement.Complete(AnyDate, AnyGuid);

            Assert.NotNull(movement.CompletedReceipt);
        }

        [Theory]
        [InlineData(MovementStatus.New)]
        [InlineData(MovementStatus.Rejected)]
        [InlineData(MovementStatus.Cancelled)]
        [InlineData(MovementStatus.Submitted)]
        public void CannotCompleteNonReceivedMovement(MovementStatus status)
        {
            SetMovementStatus(status, movement);

            Assert.Throws<InvalidOperationException>(() => movement.Complete(AnyDate, AnyGuid));
        }

        private Movement CreateCompleteMovement()
        {
            var notificationId = new Guid("EAD34BEE-E962-4D4D-9D53-ADCD7240C333");
            var movement = new Movement(1, notificationId);

            ObjectInstantiator<Movement>.SetProperty(x => x.Date, AnyDate, movement);

            movement.SetQuantity(new ShipmentQuantity(5m, ShipmentQuantityUnits.Tonnes));

            typeof(Movement).GetProperty("PackagingInfosCollection", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(movement, new List<PackagingInfo>());
            movement.SetPackagingInfos(new[] { PackagingInfo.CreatePackagingInfo(PackagingType.Box) });
            movement.SetNumberOfPackages(50);

            var business = Business.CreateBusiness(
                AnyString,
                Domain.BusinessType.LimitedCompany,
                AnyString,
                AnyString);

            var address = new Address(
                AnyString,
                AnyString,
                AnyString,
                AnyString,
                AnyString,
                AnyString);

            var contact = new Contact(
                AnyString,
                AnyString,
                AnyString,
                AnyString);

            typeof(Movement).GetProperty("MovementCarriersCollection", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(movement, new List<MovementCarrier>());
            movement.SetMovementCarriers(new[]
            {
                new MovementCarrier(
                    1,
                    new Carrier(
                        business,
                        address,
                        contact))
            });

            return movement;
        }
    }
}
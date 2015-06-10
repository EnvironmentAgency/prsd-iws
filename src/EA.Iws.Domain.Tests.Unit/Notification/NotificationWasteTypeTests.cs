﻿namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Notification;
    using Xunit;

    public class NotificationWasteTypeTests
    {
        [Fact]
        public void CanAddWasteTypeForWood()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddWasteType(WasteType.CreateWoodWasteType("This waste type is of wood type. I am writing some description here."));

            Assert.True(notification.HasWasteType);
        }

        [Fact]
        public void CanAddWasteTypeForOther()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddWasteType(WasteType.CreateOtherWasteType("some other name",
                "This waste type is of any other type. I am writing some description here."));

            Assert.True(notification.HasWasteType);
        }

        [Fact]
        public void CanAddWasteTypeForRdf()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            List<WasteComposition> wasteCompositions = new List<WasteComposition>();
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("First Constituent", 1, 100));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Second Constituent", 2, 100));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Third Constituent", 3, 100));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Fourth Constituent", 4, 100));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Fifth Constituent", 5, 100));

            notification.AddWasteType(WasteType.CreateRdfWasteType(wasteCompositions));

            Assert.True(notification.HasWasteType);
            Assert.True(5 == notification.WasteType.WasteCompositions.Count());
        }

        [Fact]
        public void CanAddWasteTypeForSrf()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Disposal,
                UKCompetentAuthority.England, 0);

            List<WasteComposition> wasteCompositions = new List<WasteComposition>();
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("First Constituent", 1, 100));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Second Constituent", 2, 100));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Third Constituent", 3, 100));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Fourth Constituent", 4, 100));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Fifth Constituent", 5, 100));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Sixth Constituent", 6, 100));

            notification.AddWasteType(WasteType.CreateSrfWasteType(wasteCompositions));

            Assert.True(notification.HasWasteType);
            Assert.True(6 == notification.WasteType.WasteCompositions.Count());
        }

        [Fact]
        public void CantAddMultipleWasteTypes()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddWasteType(WasteType.CreateWoodWasteType("some description"));

            Action addSecondWasteType = () => notification.AddWasteType(WasteType.CreateOtherWasteType("name", "description"));

            Assert.Throws<InvalidOperationException>(addSecondWasteType);
        }

        [Fact]
        public void CantAddWasteTypeForWoodWithoutDescription()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Action addWoodWasteTypeWithoutDescription = () => notification.AddWasteType(WasteType.CreateWoodWasteType(null));
            Assert.Throws<ArgumentNullException>(addWoodWasteTypeWithoutDescription);
        }

        [Fact]
        public void CantAddWasteTypeForOtherWithoutOtherName()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Action addOtherWasteTypeWithoutName = () => notification.AddWasteType(WasteType.CreateOtherWasteType(null, "description"));
            Assert.Throws<ArgumentNullException>(addOtherWasteTypeWithoutName);
            var a = notification.WasteType == null;
        }

        [Fact]
        public void CantAddWasteTypeForOtherWithoutDescription()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Action addOtherWasteTypeWithoutDescription = () => notification.AddWasteType(WasteType.CreateOtherWasteType("name", null));
            Assert.Throws<ArgumentNullException>(addOtherWasteTypeWithoutDescription);
        }

        [Fact]
        public void CantAddWasteTypeForRdfWithoutComposition()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Action addOtherWasteTypeWithoutDescription = () => notification.AddWasteType(WasteType.CreateRdfWasteType(null));
            Assert.Throws<ArgumentException>(addOtherWasteTypeWithoutDescription);
        }

        [Fact]
        public void AddPhysicalCharacteristics_WithValidData_PhysicalCharacteristicsAdded()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddWasteType(WasteType.CreateOtherWasteType("Name", "Description"));

            notification.AddPhysicalCharacteristic(PhysicalCharacteristicType.Powdery);

            Assert.Equal(1, notification.WasteType.PhysicalCharacteristics.Count());
        }

        [Fact]
        public void AddPhysicalCharacteristics_WithValidDescription_PhysicalCharacteristicsAdded()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddWasteType(WasteType.CreateOtherWasteType("Name", "Description"));

            notification.AddPhysicalCharacteristic(PhysicalCharacteristicType.Other, "Other description");

            Assert.Equal(1, notification.WasteType.PhysicalCharacteristics.Count());
        }

        [Fact]
        public void AddPhysicalCharacteristics_WithoutWasteType_ThrowsException()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Action addPhysicalInfo = () => notification.AddPhysicalCharacteristic(PhysicalCharacteristicType.Powdery);

            Assert.Throws<InvalidOperationException>(addPhysicalInfo);
        }

        [Fact]
        public void AddPhysicalCharacteristics_WithDescriptionWhenNotOther_ThrowsException()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Action addPhysicalInfo = () => notification.AddPhysicalCharacteristic(PhysicalCharacteristicType.Powdery, "other description");

            Assert.Throws<InvalidOperationException>(addPhysicalInfo);
        }

        [Fact]
        public void AddWasteGenerationProcess_WithoutWasteType_ThrowsException()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Action addWasteGenerationProcess = () => notification.AddWasteGenerationProcess("process", true);

            Assert.Throws<InvalidOperationException>(addWasteGenerationProcess);
        }

        [Fact]
        public void AddWasteGenerationProcess_WithValidData_WasteGenerationDescriptionAdded()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddWasteType(WasteType.CreateOtherWasteType("Name", "Description"));

            notification.AddWasteGenerationProcess("Process description", true);

            Assert.Equal("Process description", notification.WasteType.WasteGenerationProcess);
        }
    }
}

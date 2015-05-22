﻿namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using System.Linq;
    using Domain.Notification;
    using Helpers;
    using Xunit;

    public class NotificationApplicationTests
    {
        private const string England = "England";
        private const string Scotland = "Scotland";
        private const string NorthernIreland = "Northern Ireland";
        private const string Wales = "Wales";

        [Theory]
        [InlineData("GB 0001 123456", England, 123456)]
        [InlineData("GB 0002 123456", Scotland, 123456)]
        [InlineData("GB 0003 123456", NorthernIreland, 123456)]
        [InlineData("GB 0004 123456", Wales, 123456)]
        [InlineData("GB 0001 005000", England, 5000)]
        [InlineData("GB 0002 000100", Scotland, 100)]
        public void NotificationNumberFormat(string expected, string country, int notificationNumber)
        {
            var userId = new Guid("{FCCC2E8A-2464-4C10-8521-09F16F2C550C}");
            var notification = new NotificationApplication(userId, NotificationType.Disposal,
                GetCompetentAuthority(country),
                notificationNumber);
            Assert.Equal(expected, notification.NotificationNumber);
        }

        private UKCompetentAuthority GetCompetentAuthority(string country)
        {
            if (country == England)
            {
                return UKCompetentAuthority.England;
            }
            if (country == Scotland)
            {
                return UKCompetentAuthority.Scotland;
            }
            if (country == NorthernIreland)
            {
                return UKCompetentAuthority.NorthernIreland;
            }
            if (country == Wales)
            {
                return UKCompetentAuthority.Wales;
            }
            throw new ArgumentException("Unknown competent authority", "country");
        }

        private Producer CreateEmptyProducer()
        {
            var address = new Address(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                "United Kingdom");

            var business = new Business(string.Empty, String.Empty, String.Empty, string.Empty);

            var contact = new Contact(string.Empty, String.Empty, String.Empty, String.Empty);

            return new Producer(business, address, contact);
        }

        [Fact]
        public void ProducersCanOnlyHaveOneSiteOfExport()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var producer1 = CreateEmptyProducer();
            var producer2 = CreateEmptyProducer();

            EntityHelper.SetEntityIds(producer1, producer2);

            notification.AddProducer(producer1);
            notification.AddProducer(producer2);

            notification.SetAsSiteOfExport(producer1.Id);

            var siteOfExportCount = notification.Producers.Count(p => p.IsSiteOfExport);
            Assert.Equal(1, siteOfExportCount);
        }

        [Fact]
        public void CantSetNonExistentProducerAsSiteOfExport()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);
            var producer = CreateEmptyProducer();
            EntityHelper.SetEntityId(producer, new Guid("{D65D91BA-FA77-47F6-ACF5-B1A405DEE187}"));

            notification.AddProducer(producer);

            var badId = new Guid("{5DF206F6-4116-4EEC-949A-0FC71FE609C1}");

            Action setAsSiteOfExport = () => notification.SetAsSiteOfExport(badId);

            Assert.Throws<InvalidOperationException>(setAsSiteOfExport);
        }

        [Fact]
        public void CantRemoveNonExistentProducer()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);
            var producer = CreateEmptyProducer();

            Action removeProducer = () => notification.RemoveProducer(producer);

            Assert.Throws<InvalidOperationException>(removeProducer);
        }

        [Fact]
        public void UpdateProducerModifiesCollection()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);
            var producer = CreateEmptyProducer();
            var producerId = new Guid("{D65D91BA-FA77-47F6-ACF5-B1A405DEE187}");
            EntityHelper.SetEntityId(producer, producerId);

            notification.AddProducer(producer);

            var updateProducer = notification.Producers.Single(p => p.Id == producerId);
            var newAddress = new Address("new building", string.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty,
                "United Kingdom");

            // TODO - no way to update Producer yet so using reflection to change values
            typeof(Producer).GetProperty("Address").SetValue(updateProducer, newAddress);

            Assert.Equal("new building", notification.Producers.Single(p => p.Id == producerId).Address.Building);
        }
    }
}
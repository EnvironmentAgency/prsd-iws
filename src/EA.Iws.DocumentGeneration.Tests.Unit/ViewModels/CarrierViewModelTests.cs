﻿namespace EA.Iws.DocumentGeneration.Tests.Unit.ViewModels
{
    using System;
    using DocumentGeneration.Formatters;
    using DocumentGeneration.ViewModels;
    using Domain.NotificationApplication;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class CarrierViewModelTests
    {
        private const string AnyString = "test";

        private readonly MeansOfTransportFormatter formatter = new MeansOfTransportFormatter();
        private readonly MeansOfTransport meansOfTransport;
        private readonly TestableCarrierCollection carrierCollection;
        private readonly TestableCarrier firstCarrier;
        private readonly TestableCarrier secondCarrier;

        public CarrierViewModelTests()
        {
            firstCarrier = new TestableCarrier
            {
                Address = TestableAddress.AddlestoneAddress,
                Business = TestableBusiness.WasteSolutions,
                Contact = TestableContact.BillyKnuckles
            };

            secondCarrier = new TestableCarrier
            {
                Address = TestableAddress.WitneyAddress,
                Business = TestableBusiness.CSharpGarbageCollector,
                Contact = TestableContact.MikeMerry
            };

            var notificationId = new Guid("1EB00552-0589-4AB2-804E-16CF9B8286BA");

            carrierCollection = new TestableCarrierCollection(notificationId);
            meansOfTransport = new MeansOfTransport(notificationId);
        }

        [Fact]
        public void NotificationIsNull_CarrierCollectionIsNull_ReturnsEmptyList()
        {
            var result = CarrierViewModel.CreateCarrierViewModelsForNotification(null, null, formatter);

            Assert.Empty(result);
        }

        [Fact]
        public void NotificationHasNoCarriers_ReturnsEmptyList()
        {
            var result = CarrierViewModel.CreateCarrierViewModelsForNotification(meansOfTransport, carrierCollection, formatter);

            Assert.Empty(result);
        }

        [Fact]
        public void NotificationHasOneCarrier_ReturnsListWithOneItem()
        {
            carrierCollection.AddCarrier(firstCarrier.Business, firstCarrier.Address, firstCarrier.Contact);

            var result = CarrierViewModel.CreateCarrierViewModelsForNotification(meansOfTransport, carrierCollection, formatter);

            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void CarrierIsNull_ReturnsModelWithEmptyFields()
        {
            var result = new CarrierViewModel(null, string.Empty);

            Assert.Equal(string.Empty, result.ContactPerson);
            Assert.Equal(string.Empty, result.Email);
            Assert.Equal(string.Empty, result.Fax);
            Assert.Equal(string.Empty, result.RegistrationNumber);
            Assert.Equal(string.Empty, result.Telephone);
            Assert.Equal(string.Empty, result.Name);
            Assert.Equal(string.Empty, result.AnnexMessage);
            Assert.Equal(string.Empty, result.Address);
        }

        [Fact]
        public void MeansOfTransportIsNull_ReturnsMeansOfTransportAsEmptyString()
        {
            var result = new CarrierViewModel(firstCarrier, null);

            Assert.Equal(string.Empty, result.MeansOfTransport);
        }

        [Fact]
        public void SetsMeansOfTransport()
        {
            var meansOfTransport = "Road Sea Road";

            var result = new CarrierViewModel(firstCarrier, meansOfTransport);

            Assert.Equal(meansOfTransport, result.MeansOfTransport);
        }

        [Fact]
        public void SetsContactName()
        {
            var result = new CarrierViewModel(firstCarrier, string.Empty);

            Assert.Equal(firstCarrier.Contact.FullName, result.ContactPerson);
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData("", "")]
        [InlineData(" ", "")]
        [InlineData("44-901 353 7450", "+44 901 353 7450")]
        [InlineData("44-1234567890", "+44 1234567890")]
        public void SetsFaxToFormattedFaxNumber(string inputFax, string expectedFax)
        {
            firstCarrier.Contact = new TestableContact
            {
                Email = AnyString,
                Fax = inputFax,
                FullName = AnyString,
                Telephone = AnyString
            };

            var result = new CarrierViewModel(firstCarrier, string.Empty);

            Assert.Equal(expectedFax, result.Fax);
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData("", "")]
        [InlineData(" ", "")]
        [InlineData("44-901 353 7450", "+44 901 353 7450")]
        [InlineData("44-1234567890", "+44 1234567890")]
        public void SetsTelephoneToFormattedPhoneNumber(string inputPhone, string expectedPhone)
        {
            firstCarrier.Contact = new TestableContact
            {
                Email = AnyString,
                Telephone = inputPhone,
                FullName = AnyString,
                Fax = AnyString
            };

            var result = new CarrierViewModel(firstCarrier, string.Empty);

            Assert.Equal(expectedPhone, result.Telephone);
        }

        [Fact]
        public void SetsEmail()
        {
            var result = new CarrierViewModel(firstCarrier, string.Empty);

            Assert.Equal(firstCarrier.Contact.Email,
                result.Email);
        }

        [Fact]
        public void SetsName()
        {
            var result = new CarrierViewModel(firstCarrier, string.Empty);

            Assert.Equal(firstCarrier.Business.Name,
                result.Name);
        }

        [Fact]
        public void SetsRegistrationNumber()
        {
            var result = new CarrierViewModel(firstCarrier, string.Empty);

            Assert.Equal(firstCarrier.Business.RegistrationNumber,
                result.RegistrationNumber);
        }
    }
}
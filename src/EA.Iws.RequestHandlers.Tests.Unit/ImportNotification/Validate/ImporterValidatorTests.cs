﻿namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using Core.ImportNotification.Draft;
    using Core.Shared;
    using FakeItEasy;
    using FluentValidation.TestHelper;
    using RequestHandlers.ImportNotification.Validate;
    using Xunit;

    public class ImporterValidatorTests
    {
        private readonly ImporterValidator validator;

        public ImporterValidatorTests()
        {
            var countryRepository = A.Fake<Domain.ICountryRepository>();
            var addressValidator = new AddressValidator(countryRepository);
            var contactValidator = new ContactValidator();
            validator = new ImporterValidator(addressValidator, contactValidator);
        }

        [Fact]
        public void ImporterValid_ReturnsSuccess()
        {
            var importer = GetValidImporter();

            var result = validator.Validate(importer);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void ImporterAddressIsValidated()
        {
            validator.ShouldHaveChildValidator(x => x.Address, typeof(AddressValidator));
        }

        [Fact]
        public void ImporterContactIsValidated()
        {
            validator.ShouldHaveChildValidator(x => x.Contact, typeof(ContactValidator));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void BusinessNameMissing_ReturnsFailure(string businessName)
        {
            validator.ShouldHaveValidationErrorFor(x => x.BusinessName, businessName);
        }

        [Fact]
        public void BusinessTypeMissing_ReturnsFailure()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Type, null as BusinessType?);
        }

        private Importer GetValidImporter()
        {
            return new Importer
            {
                Address = new Address
                {
                    AddressLine1 = "Eliot House",
                    AddressLine2 = "Eliot Lane",
                    CountryId = new System.Guid("0A323947-78A0-40FE-BDDD-9A58803559BC"),
                    PostalCode = "EL10TJ",
                    TownOrCity = "Eliotsville"
                },
                Contact = new Contact
                {
                    ContactName = "Mike Merry",
                    Email = "mike@merry.com",
                    Telephone = "01234 567890"
                },
                BusinessName = "Mike and Eliot Bros.",
                RegistrationNumber = "12987457",
                Type = BusinessType.Other
            };
        }
    }
}
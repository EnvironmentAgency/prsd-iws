﻿namespace EA.Iws.DataAccess.Tests.Integration
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Shared;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using Xunit;

    [Trait("Category", "Integration")]
    public class PricingStructureIntegration
    {
        private readonly IwsContext context;

        public PricingStructureIntegration()
        {
            var userContext = A.Fake<IUserContext>();

            A.CallTo(() => userContext.UserId).Returns(Guid.NewGuid());

            context = new IwsContext(userContext);
        }

        [Theory]
        [InlineData(1, TradeDirection.Export, 1, false, 1, 1450)]
        [InlineData(1, TradeDirection.Export, 1, false, 2, 1450)]
        [InlineData(1, TradeDirection.Export, 1, false, 3, 1450)]
        [InlineData(1, TradeDirection.Export, 1, false, 5, 1450)]
        [InlineData(1, TradeDirection.Export, 1, false, 6, 2700)]
        [InlineData(1, TradeDirection.Export, 1, false, 19, 2700)]
        [InlineData(1, TradeDirection.Export, 1, false, 20, 2700)]
        [InlineData(1, TradeDirection.Export, 1, false, 21, 4070)]
        [InlineData(1, TradeDirection.Export, 1, false, 30, 4070)]
        [InlineData(1, TradeDirection.Export, 1, false, 100, 4070)]
        [InlineData(1, TradeDirection.Export, 1, false, 101, 7920)]
        [InlineData(1, TradeDirection.Export, 1, false, 110, 7920)]
        [InlineData(1, TradeDirection.Export, 1, false, 500, 7920)]
        [InlineData(1, TradeDirection.Export, 1, false, 501, 14380)]
        [InlineData(1, TradeDirection.Export, 1, true, 1, 1450)]
        [InlineData(1, TradeDirection.Export, 1, true, 2, 1450)]
        [InlineData(1, TradeDirection.Export, 1, true, 3, 1450)]
        [InlineData(1, TradeDirection.Export, 1, true, 5, 1450)]
        [InlineData(1, TradeDirection.Export, 1, true, 6, 2700)]
        [InlineData(1, TradeDirection.Export, 1, true, 19, 2700)]
        [InlineData(1, TradeDirection.Export, 1, true, 20, 2700)]
        [InlineData(1, TradeDirection.Export, 1, true, 21, 4070)]
        [InlineData(1, TradeDirection.Export, 1, true, 30, 4070)]
        [InlineData(1, TradeDirection.Export, 1, true, 100, 4070)]
        [InlineData(1, TradeDirection.Export, 1, true, 101, 7920)]
        [InlineData(1, TradeDirection.Export, 1, true, 110, 7920)]
        [InlineData(1, TradeDirection.Export, 1, true, 500, 7920)]
        [InlineData(1, TradeDirection.Export, 1, true, 501, 14380)]
        [InlineData(1, TradeDirection.Export, 2, false, 1, 1540)]
        [InlineData(1, TradeDirection.Export, 2, false, 2, 1540)]
        [InlineData(1, TradeDirection.Export, 2, false, 3, 1540)]
        [InlineData(1, TradeDirection.Export, 2, false, 5, 1540)]
        [InlineData(1, TradeDirection.Export, 2, false, 6, 3330)]
        [InlineData(1, TradeDirection.Export, 2, false, 19, 3330)]
        [InlineData(1, TradeDirection.Export, 2, false, 20, 3330)]
        [InlineData(1, TradeDirection.Export, 2, false, 21, 5500)]
        [InlineData(1, TradeDirection.Export, 2, false, 30, 5500)]
        [InlineData(1, TradeDirection.Export, 2, false, 100, 5500)]
        [InlineData(1, TradeDirection.Export, 2, false, 101, 10600)]
        [InlineData(1, TradeDirection.Export, 2, false, 110, 10600)]
        [InlineData(1, TradeDirection.Export, 2, false, 500, 10600)]
        [InlineData(1, TradeDirection.Export, 2, false, 501, 19500)]
        [InlineData(1, TradeDirection.Export, 2, true, 1, 1700)]
        [InlineData(1, TradeDirection.Export, 2, true, 2, 1700)]
        [InlineData(1, TradeDirection.Export, 2, true, 3, 1700)]
        [InlineData(1, TradeDirection.Export, 2, true, 5, 1700)]
        [InlineData(1, TradeDirection.Export, 2, true, 6, 3330)]
        [InlineData(1, TradeDirection.Export, 2, true, 19, 3330)]
        [InlineData(1, TradeDirection.Export, 2, true, 20, 3330)]
        [InlineData(1, TradeDirection.Export, 2, true, 21, 6000)]
        [InlineData(1, TradeDirection.Export, 2, true, 30, 6000)]
        [InlineData(1, TradeDirection.Export, 2, true, 100, 6000)]
        [InlineData(1, TradeDirection.Export, 2, true, 101, 12900)]
        [InlineData(1, TradeDirection.Export, 2, true, 110, 12900)]
        [InlineData(1, TradeDirection.Export, 2, true, 500, 12900)]
        [InlineData(1, TradeDirection.Export, 2, true, 501, 24000)]
        [InlineData(1, TradeDirection.Import, 1, false, 1, 1250)]
        [InlineData(1, TradeDirection.Import, 1, false, 2, 1250)]
        [InlineData(1, TradeDirection.Import, 1, false, 3, 1250)]
        [InlineData(1, TradeDirection.Import, 1, false, 5, 1250)]
        [InlineData(1, TradeDirection.Import, 1, false, 6, 2700)]
        [InlineData(1, TradeDirection.Import, 1, false, 19, 2700)]
        [InlineData(1, TradeDirection.Import, 1, false, 20, 2700)]
        [InlineData(1, TradeDirection.Import, 1, false, 21, 4900)]
        [InlineData(1, TradeDirection.Import, 1, false, 30, 4900)]
        [InlineData(1, TradeDirection.Import, 1, false, 100, 4900)]
        [InlineData(1, TradeDirection.Import, 1, false, 101, 10600)]
        [InlineData(1, TradeDirection.Import, 1, false, 110, 10600)]
        [InlineData(1, TradeDirection.Import, 1, false, 500, 10600)]
        [InlineData(1, TradeDirection.Import, 1, false, 501, 19500)]
        [InlineData(1, TradeDirection.Import, 1, true, 1, 1450)]
        [InlineData(1, TradeDirection.Import, 1, true, 2, 1450)]
        [InlineData(1, TradeDirection.Import, 1, true, 3, 1450)]
        [InlineData(1, TradeDirection.Import, 1, true, 5, 1450)]
        [InlineData(1, TradeDirection.Import, 1, true, 6, 2830)]
        [InlineData(1, TradeDirection.Import, 1, true, 19, 2830)]
        [InlineData(1, TradeDirection.Import, 1, true, 20, 2830)]
        [InlineData(1, TradeDirection.Import, 1, true, 21, 5500)]
        [InlineData(1, TradeDirection.Import, 1, true, 30, 5500)]
        [InlineData(1, TradeDirection.Import, 1, true, 100, 5500)]
        [InlineData(1, TradeDirection.Import, 1, true, 101, 12900)]
        [InlineData(1, TradeDirection.Import, 1, true, 110, 12900)]
        [InlineData(1, TradeDirection.Import, 1, true, 500, 12900)]
        [InlineData(1, TradeDirection.Import, 1, true, 501, 24000)]
        [InlineData(1, TradeDirection.Import, 2, false, 1, 1540)]
        [InlineData(1, TradeDirection.Import, 2, false, 2, 1540)]
        [InlineData(1, TradeDirection.Import, 2, false, 3, 1540)]
        [InlineData(1, TradeDirection.Import, 2, false, 5, 1540)]
        [InlineData(1, TradeDirection.Import, 2, false, 6, 3330)]
        [InlineData(1, TradeDirection.Import, 2, false, 19, 3330)]
        [InlineData(1, TradeDirection.Import, 2, false, 20, 3330)]
        [InlineData(1, TradeDirection.Import, 2, false, 21, 5500)]
        [InlineData(1, TradeDirection.Import, 2, false, 30, 5500)]
        [InlineData(1, TradeDirection.Import, 2, false, 100, 5500)]
        [InlineData(1, TradeDirection.Import, 2, false, 101, 10600)]
        [InlineData(1, TradeDirection.Import, 2, false, 110, 10600)]
        [InlineData(1, TradeDirection.Import, 2, false, 500, 10600)]
        [InlineData(1, TradeDirection.Import, 2, false, 501, 19500)]
        [InlineData(1, TradeDirection.Import, 2, true, 1, 1700)]
        [InlineData(1, TradeDirection.Import, 2, true, 2, 1700)]
        [InlineData(1, TradeDirection.Import, 2, true, 3, 1700)]
        [InlineData(1, TradeDirection.Import, 2, true, 5, 1700)]
        [InlineData(1, TradeDirection.Import, 2, true, 6, 3330)]
        [InlineData(1, TradeDirection.Import, 2, true, 19, 3330)]
        [InlineData(1, TradeDirection.Import, 2, true, 20, 3330)]
        [InlineData(1, TradeDirection.Import, 2, true, 21, 6000)]
        [InlineData(1, TradeDirection.Import, 2, true, 30, 6000)]
        [InlineData(1, TradeDirection.Import, 2, true, 100, 6000)]
        [InlineData(1, TradeDirection.Import, 2, true, 101, 12900)]
        [InlineData(1, TradeDirection.Import, 2, true, 110, 12900)]
        [InlineData(1, TradeDirection.Import, 2, true, 500, 12900)]
        [InlineData(1, TradeDirection.Import, 2, true, 501, 24000)]
        [InlineData(2, TradeDirection.Export, 1, false, 1, 1450)]
        [InlineData(2, TradeDirection.Export, 1, false, 2, 1450)]
        [InlineData(2, TradeDirection.Export, 1, false, 3, 1450)]
        [InlineData(2, TradeDirection.Export, 1, false, 5, 1450)]
        [InlineData(2, TradeDirection.Export, 1, false, 6, 2700)]
        [InlineData(2, TradeDirection.Export, 1, false, 19, 2700)]
        [InlineData(2, TradeDirection.Export, 1, false, 20, 2700)]
        [InlineData(2, TradeDirection.Export, 1, false, 21, 4070)]
        [InlineData(2, TradeDirection.Export, 1, false, 30, 4070)]
        [InlineData(2, TradeDirection.Export, 1, false, 100, 4070)]
        [InlineData(2, TradeDirection.Export, 1, false, 101, 7920)]
        [InlineData(2, TradeDirection.Export, 1, false, 110, 7920)]
        [InlineData(2, TradeDirection.Export, 1, false, 500, 7920)]
        [InlineData(2, TradeDirection.Export, 1, false, 501, 14380)]
        [InlineData(2, TradeDirection.Export, 1, true, 1, 1450)]
        [InlineData(2, TradeDirection.Export, 1, true, 2, 1450)]
        [InlineData(2, TradeDirection.Export, 1, true, 3, 1450)]
        [InlineData(2, TradeDirection.Export, 1, true, 5, 1450)]
        [InlineData(2, TradeDirection.Export, 1, true, 6, 2700)]
        [InlineData(2, TradeDirection.Export, 1, true, 19, 2700)]
        [InlineData(2, TradeDirection.Export, 1, true, 20, 2700)]
        [InlineData(2, TradeDirection.Export, 1, true, 21, 4070)]
        [InlineData(2, TradeDirection.Export, 1, true, 30, 4070)]
        [InlineData(2, TradeDirection.Export, 1, true, 100, 4070)]
        [InlineData(2, TradeDirection.Export, 1, true, 101, 7920)]
        [InlineData(2, TradeDirection.Export, 1, true, 110, 7920)]
        [InlineData(2, TradeDirection.Export, 1, true, 500, 7920)]
        [InlineData(2, TradeDirection.Export, 1, true, 501, 14380)]
        [InlineData(2, TradeDirection.Export, 2, false, 1, 1540)]
        [InlineData(2, TradeDirection.Export, 2, false, 2, 1540)]
        [InlineData(2, TradeDirection.Export, 2, false, 3, 1540)]
        [InlineData(2, TradeDirection.Export, 2, false, 5, 1540)]
        [InlineData(2, TradeDirection.Export, 2, false, 6, 3330)]
        [InlineData(2, TradeDirection.Export, 2, false, 19, 3330)]
        [InlineData(2, TradeDirection.Export, 2, false, 20, 3330)]
        [InlineData(2, TradeDirection.Export, 2, false, 21, 5500)]
        [InlineData(2, TradeDirection.Export, 2, false, 30, 5500)]
        [InlineData(2, TradeDirection.Export, 2, false, 100, 5500)]
        [InlineData(2, TradeDirection.Export, 2, false, 101, 10600)]
        [InlineData(2, TradeDirection.Export, 2, false, 110, 10600)]
        [InlineData(2, TradeDirection.Export, 2, false, 500, 10600)]
        [InlineData(2, TradeDirection.Export, 2, false, 501, 19500)]
        [InlineData(2, TradeDirection.Export, 2, true, 1, 1700)]
        [InlineData(2, TradeDirection.Export, 2, true, 2, 1700)]
        [InlineData(2, TradeDirection.Export, 2, true, 3, 1700)]
        [InlineData(2, TradeDirection.Export, 2, true, 5, 1700)]
        [InlineData(2, TradeDirection.Export, 2, true, 6, 3330)]
        [InlineData(2, TradeDirection.Export, 2, true, 19, 3330)]
        [InlineData(2, TradeDirection.Export, 2, true, 20, 3330)]
        [InlineData(2, TradeDirection.Export, 2, true, 21, 6000)]
        [InlineData(2, TradeDirection.Export, 2, true, 30, 6000)]
        [InlineData(2, TradeDirection.Export, 2, true, 100, 6000)]
        [InlineData(2, TradeDirection.Export, 2, true, 101, 12900)]
        [InlineData(2, TradeDirection.Export, 2, true, 110, 12900)]
        [InlineData(2, TradeDirection.Export, 2, true, 500, 12900)]
        [InlineData(2, TradeDirection.Export, 2, true, 501, 24000)]
        [InlineData(2, TradeDirection.Import, 1, false, 1, 1250)]
        [InlineData(2, TradeDirection.Import, 1, false, 2, 1250)]
        [InlineData(2, TradeDirection.Import, 1, false, 3, 1250)]
        [InlineData(2, TradeDirection.Import, 1, false, 5, 1250)]
        [InlineData(2, TradeDirection.Import, 1, false, 6, 2700)]
        [InlineData(2, TradeDirection.Import, 1, false, 19, 2700)]
        [InlineData(2, TradeDirection.Import, 1, false, 20, 2700)]
        [InlineData(2, TradeDirection.Import, 1, false, 21, 4900)]
        [InlineData(2, TradeDirection.Import, 1, false, 30, 4900)]
        [InlineData(2, TradeDirection.Import, 1, false, 100, 4900)]
        [InlineData(2, TradeDirection.Import, 1, false, 101, 10600)]
        [InlineData(2, TradeDirection.Import, 1, false, 110, 10600)]
        [InlineData(2, TradeDirection.Import, 1, false, 500, 10600)]
        [InlineData(2, TradeDirection.Import, 1, false, 501, 19500)]
        [InlineData(2, TradeDirection.Import, 1, true, 1, 1450)]
        [InlineData(2, TradeDirection.Import, 1, true, 2, 1450)]
        [InlineData(2, TradeDirection.Import, 1, true, 3, 1450)]
        [InlineData(2, TradeDirection.Import, 1, true, 5, 1450)]
        [InlineData(2, TradeDirection.Import, 1, true, 6, 2830)]
        [InlineData(2, TradeDirection.Import, 1, true, 19, 2830)]
        [InlineData(2, TradeDirection.Import, 1, true, 20, 2830)]
        [InlineData(2, TradeDirection.Import, 1, true, 21, 5500)]
        [InlineData(2, TradeDirection.Import, 1, true, 30, 5500)]
        [InlineData(2, TradeDirection.Import, 1, true, 100, 5500)]
        [InlineData(2, TradeDirection.Import, 1, true, 101, 12900)]
        [InlineData(2, TradeDirection.Import, 1, true, 110, 12900)]
        [InlineData(2, TradeDirection.Import, 1, true, 500, 12900)]
        [InlineData(2, TradeDirection.Import, 1, true, 501, 24000)]
        [InlineData(2, TradeDirection.Import, 2, false, 1, 1540)]
        [InlineData(2, TradeDirection.Import, 2, false, 2, 1540)]
        [InlineData(2, TradeDirection.Import, 2, false, 3, 1540)]
        [InlineData(2, TradeDirection.Import, 2, false, 5, 1540)]
        [InlineData(2, TradeDirection.Import, 2, false, 6, 3330)]
        [InlineData(2, TradeDirection.Import, 2, false, 19, 3330)]
        [InlineData(2, TradeDirection.Import, 2, false, 20, 3330)]
        [InlineData(2, TradeDirection.Import, 2, false, 21, 5500)]
        [InlineData(2, TradeDirection.Import, 2, false, 30, 5500)]
        [InlineData(2, TradeDirection.Import, 2, false, 100, 5500)]
        [InlineData(2, TradeDirection.Import, 2, false, 101, 10600)]
        [InlineData(2, TradeDirection.Import, 2, false, 110, 10600)]
        [InlineData(2, TradeDirection.Import, 2, false, 500, 10600)]
        [InlineData(2, TradeDirection.Import, 2, false, 501, 19500)]
        [InlineData(2, TradeDirection.Import, 2, true, 1, 1700)]
        [InlineData(2, TradeDirection.Import, 2, true, 2, 1700)]
        [InlineData(2, TradeDirection.Import, 2, true, 3, 1700)]
        [InlineData(2, TradeDirection.Import, 2, true, 5, 1700)]
        [InlineData(2, TradeDirection.Import, 2, true, 6, 3330)]
        [InlineData(2, TradeDirection.Import, 2, true, 19, 3330)]
        [InlineData(2, TradeDirection.Import, 2, true, 20, 3330)]
        [InlineData(2, TradeDirection.Import, 2, true, 21, 6000)]
        [InlineData(2, TradeDirection.Import, 2, true, 30, 6000)]
        [InlineData(2, TradeDirection.Import, 2, true, 100, 6000)]
        [InlineData(2, TradeDirection.Import, 2, true, 101, 12900)]
        [InlineData(2, TradeDirection.Import, 2, true, 110, 12900)]
        [InlineData(2, TradeDirection.Import, 2, true, 500, 12900)]
        [InlineData(2, TradeDirection.Import, 2, true, 501, 24000)]
        [InlineData(3, TradeDirection.Export, 1, false, 1, 1090)]
        [InlineData(3, TradeDirection.Export, 1, false, 2, 1090)]
        [InlineData(3, TradeDirection.Export, 1, false, 3, 1090)]
        [InlineData(3, TradeDirection.Export, 1, false, 5, 1090)]
        [InlineData(3, TradeDirection.Export, 1, false, 6, 2025)]
        [InlineData(3, TradeDirection.Export, 1, false, 19, 2025)]
        [InlineData(3, TradeDirection.Export, 1, false, 20, 2025)]
        [InlineData(3, TradeDirection.Export, 1, false, 21, 3050)]
        [InlineData(3, TradeDirection.Export, 1, false, 30, 3050)]
        [InlineData(3, TradeDirection.Export, 1, false, 100, 3050)]
        [InlineData(3, TradeDirection.Export, 1, false, 101, 5940)]
        [InlineData(3, TradeDirection.Export, 1, false, 110, 5940)]
        [InlineData(3, TradeDirection.Export, 1, false, 500, 5940)]
        [InlineData(3, TradeDirection.Export, 1, false, 501, 10785)]
        [InlineData(3, TradeDirection.Export, 1, true, 1, 1090)]
        [InlineData(3, TradeDirection.Export, 1, true, 2, 1090)]
        [InlineData(3, TradeDirection.Export, 1, true, 3, 1090)]
        [InlineData(3, TradeDirection.Export, 1, true, 5, 1090)]
        [InlineData(3, TradeDirection.Export, 1, true, 6, 2025)]
        [InlineData(3, TradeDirection.Export, 1, true, 19, 2025)]
        [InlineData(3, TradeDirection.Export, 1, true, 20, 2025)]
        [InlineData(3, TradeDirection.Export, 1, true, 21, 3050)]
        [InlineData(3, TradeDirection.Export, 1, true, 30, 3050)]
        [InlineData(3, TradeDirection.Export, 1, true, 100, 3050)]
        [InlineData(3, TradeDirection.Export, 1, true, 101, 5940)]
        [InlineData(3, TradeDirection.Export, 1, true, 110, 5940)]
        [InlineData(3, TradeDirection.Export, 1, true, 500, 5940)]
        [InlineData(3, TradeDirection.Export, 1, true, 501, 10785)]
        [InlineData(3, TradeDirection.Export, 2, false, 1, 1090)]
        [InlineData(3, TradeDirection.Export, 2, false, 2, 1090)]
        [InlineData(3, TradeDirection.Export, 2, false, 3, 1090)]
        [InlineData(3, TradeDirection.Export, 2, false, 5, 1090)]
        [InlineData(3, TradeDirection.Export, 2, false, 6, 2025)]
        [InlineData(3, TradeDirection.Export, 2, false, 19, 2025)]
        [InlineData(3, TradeDirection.Export, 2, false, 20, 2025)]
        [InlineData(3, TradeDirection.Export, 2, false, 21, 3050)]
        [InlineData(3, TradeDirection.Export, 2, false, 30, 3050)]
        [InlineData(3, TradeDirection.Export, 2, false, 100, 3050)]
        [InlineData(3, TradeDirection.Export, 2, false, 101, 5940)]
        [InlineData(3, TradeDirection.Export, 2, false, 110, 5940)]
        [InlineData(3, TradeDirection.Export, 2, false, 500, 5940)]
        [InlineData(3, TradeDirection.Export, 2, false, 501, 10785)]
        [InlineData(3, TradeDirection.Export, 2, true, 1, 1090)]
        [InlineData(3, TradeDirection.Export, 2, true, 2, 1090)]
        [InlineData(3, TradeDirection.Export, 2, true, 3, 1090)]
        [InlineData(3, TradeDirection.Export, 2, true, 5, 1090)]
        [InlineData(3, TradeDirection.Export, 2, true, 6, 2025)]
        [InlineData(3, TradeDirection.Export, 2, true, 19, 2025)]
        [InlineData(3, TradeDirection.Export, 2, true, 20, 2025)]
        [InlineData(3, TradeDirection.Export, 2, true, 21, 3050)]
        [InlineData(3, TradeDirection.Export, 2, true, 30, 3050)]
        [InlineData(3, TradeDirection.Export, 2, true, 100, 3050)]
        [InlineData(3, TradeDirection.Export, 2, true, 101, 5940)]
        [InlineData(3, TradeDirection.Export, 2, true, 110, 5940)]
        [InlineData(3, TradeDirection.Export, 2, true, 500, 5940)]
        [InlineData(3, TradeDirection.Export, 2, true, 501, 10785)]
        [InlineData(3, TradeDirection.Import, 1, false, 1, 940)]
        [InlineData(3, TradeDirection.Import, 1, false, 2, 940)]
        [InlineData(3, TradeDirection.Import, 1, false, 3, 940)]
        [InlineData(3, TradeDirection.Import, 1, false, 5, 940)]
        [InlineData(3, TradeDirection.Import, 1, false, 6, 2025)]
        [InlineData(3, TradeDirection.Import, 1, false, 19, 2025)]
        [InlineData(3, TradeDirection.Import, 1, false, 20, 2025)]
        [InlineData(3, TradeDirection.Import, 1, false, 21, 3675)]
        [InlineData(3, TradeDirection.Import, 1, false, 30, 3675)]
        [InlineData(3, TradeDirection.Import, 1, false, 100, 3675)]
        [InlineData(3, TradeDirection.Import, 1, false, 101, 7950)]
        [InlineData(3, TradeDirection.Import, 1, false, 110, 7950)]
        [InlineData(3, TradeDirection.Import, 1, false, 500, 7950)]
        [InlineData(3, TradeDirection.Import, 1, false, 501, 14625)]
        [InlineData(3, TradeDirection.Import, 1, true, 1, 940)]
        [InlineData(3, TradeDirection.Import, 1, true, 2, 940)]
        [InlineData(3, TradeDirection.Import, 1, true, 3, 940)]
        [InlineData(3, TradeDirection.Import, 1, true, 5, 940)]
        [InlineData(3, TradeDirection.Import, 1, true, 6, 2025)]
        [InlineData(3, TradeDirection.Import, 1, true, 19, 2025)]
        [InlineData(3, TradeDirection.Import, 1, true, 20, 2025)]
        [InlineData(3, TradeDirection.Import, 1, true, 21, 3675)]
        [InlineData(3, TradeDirection.Import, 1, true, 30, 3675)]
        [InlineData(3, TradeDirection.Import, 1, true, 100, 3675)]
        [InlineData(3, TradeDirection.Import, 1, true, 101, 7950)]
        [InlineData(3, TradeDirection.Import, 1, true, 110, 7950)]
        [InlineData(3, TradeDirection.Import, 1, true, 500, 7950)]
        [InlineData(3, TradeDirection.Import, 1, true, 501, 14625)]
        [InlineData(3, TradeDirection.Import, 2, false, 1, 940)]
        [InlineData(3, TradeDirection.Import, 2, false, 2, 940)]
        [InlineData(3, TradeDirection.Import, 2, false, 3, 940)]
        [InlineData(3, TradeDirection.Import, 2, false, 5, 940)]
        [InlineData(3, TradeDirection.Import, 2, false, 6, 2025)]
        [InlineData(3, TradeDirection.Import, 2, false, 19, 2025)]
        [InlineData(3, TradeDirection.Import, 2, false, 20, 2025)]
        [InlineData(3, TradeDirection.Import, 2, false, 21, 3675)]
        [InlineData(3, TradeDirection.Import, 2, false, 30, 3675)]
        [InlineData(3, TradeDirection.Import, 2, false, 100, 3675)]
        [InlineData(3, TradeDirection.Import, 2, false, 101, 7950)]
        [InlineData(3, TradeDirection.Import, 2, false, 110, 7950)]
        [InlineData(3, TradeDirection.Import, 2, false, 500, 7950)]
        [InlineData(3, TradeDirection.Import, 2, false, 501, 14625)]
        [InlineData(3, TradeDirection.Import, 2, true, 1, 940)]
        [InlineData(3, TradeDirection.Import, 2, true, 2, 940)]
        [InlineData(3, TradeDirection.Import, 2, true, 3, 940)]
        [InlineData(3, TradeDirection.Import, 2, true, 5, 940)]
        [InlineData(3, TradeDirection.Import, 2, true, 6, 2025)]
        [InlineData(3, TradeDirection.Import, 2, true, 19, 2025)]
        [InlineData(3, TradeDirection.Import, 2, true, 20, 2025)]
        [InlineData(3, TradeDirection.Import, 2, true, 21, 3675)]
        [InlineData(3, TradeDirection.Import, 2, true, 30, 3675)]
        [InlineData(3, TradeDirection.Import, 2, true, 100, 3675)]
        [InlineData(3, TradeDirection.Import, 2, true, 101, 7950)]
        [InlineData(3, TradeDirection.Import, 2, true, 110, 7950)]
        [InlineData(3, TradeDirection.Import, 2, true, 500, 7950)]
        [InlineData(3, TradeDirection.Import, 2, true, 501, 14625)]
        [InlineData(4, TradeDirection.Export, 1, false, 1, 1450)]
        [InlineData(4, TradeDirection.Export, 1, false, 2, 1450)]
        [InlineData(4, TradeDirection.Export, 1, false, 3, 1450)]
        [InlineData(4, TradeDirection.Export, 1, false, 5, 1450)]
        [InlineData(4, TradeDirection.Export, 1, false, 6, 2700)]
        [InlineData(4, TradeDirection.Export, 1, false, 19, 2700)]
        [InlineData(4, TradeDirection.Export, 1, false, 20, 2700)]
        [InlineData(4, TradeDirection.Export, 1, false, 21, 4070)]
        [InlineData(4, TradeDirection.Export, 1, false, 30, 4070)]
        [InlineData(4, TradeDirection.Export, 1, false, 100, 4070)]
        [InlineData(4, TradeDirection.Export, 1, false, 101, 7920)]
        [InlineData(4, TradeDirection.Export, 1, false, 110, 7920)]
        [InlineData(4, TradeDirection.Export, 1, false, 500, 7920)]
        [InlineData(4, TradeDirection.Export, 1, false, 501, 14380)]
        [InlineData(4, TradeDirection.Export, 1, true, 1, 1450)]
        [InlineData(4, TradeDirection.Export, 1, true, 2, 1450)]
        [InlineData(4, TradeDirection.Export, 1, true, 3, 1450)]
        [InlineData(4, TradeDirection.Export, 1, true, 5, 1450)]
        [InlineData(4, TradeDirection.Export, 1, true, 6, 2700)]
        [InlineData(4, TradeDirection.Export, 1, true, 19, 2700)]
        [InlineData(4, TradeDirection.Export, 1, true, 20, 2700)]
        [InlineData(4, TradeDirection.Export, 1, true, 21, 4070)]
        [InlineData(4, TradeDirection.Export, 1, true, 30, 4070)]
        [InlineData(4, TradeDirection.Export, 1, true, 100, 4070)]
        [InlineData(4, TradeDirection.Export, 1, true, 101, 7920)]
        [InlineData(4, TradeDirection.Export, 1, true, 110, 7920)]
        [InlineData(4, TradeDirection.Export, 1, true, 500, 7920)]
        [InlineData(4, TradeDirection.Export, 1, true, 501, 14380)]
        [InlineData(4, TradeDirection.Export, 2, false, 1, 1540)]
        [InlineData(4, TradeDirection.Export, 2, false, 2, 1540)]
        [InlineData(4, TradeDirection.Export, 2, false, 3, 1540)]
        [InlineData(4, TradeDirection.Export, 2, false, 5, 1540)]
        [InlineData(4, TradeDirection.Export, 2, false, 6, 3330)]
        [InlineData(4, TradeDirection.Export, 2, false, 19, 3330)]
        [InlineData(4, TradeDirection.Export, 2, false, 20, 3330)]
        [InlineData(4, TradeDirection.Export, 2, false, 21, 5500)]
        [InlineData(4, TradeDirection.Export, 2, false, 30, 5500)]
        [InlineData(4, TradeDirection.Export, 2, false, 100, 5500)]
        [InlineData(4, TradeDirection.Export, 2, false, 101, 10600)]
        [InlineData(4, TradeDirection.Export, 2, false, 110, 10600)]
        [InlineData(4, TradeDirection.Export, 2, false, 500, 10600)]
        [InlineData(4, TradeDirection.Export, 2, false, 501, 19500)]
        [InlineData(4, TradeDirection.Export, 2, true, 1, 1700)]
        [InlineData(4, TradeDirection.Export, 2, true, 2, 1700)]
        [InlineData(4, TradeDirection.Export, 2, true, 3, 1700)]
        [InlineData(4, TradeDirection.Export, 2, true, 5, 1700)]
        [InlineData(4, TradeDirection.Export, 2, true, 6, 3330)]
        [InlineData(4, TradeDirection.Export, 2, true, 19, 3330)]
        [InlineData(4, TradeDirection.Export, 2, true, 20, 3330)]
        [InlineData(4, TradeDirection.Export, 2, true, 21, 6000)]
        [InlineData(4, TradeDirection.Export, 2, true, 30, 6000)]
        [InlineData(4, TradeDirection.Export, 2, true, 100, 6000)]
        [InlineData(4, TradeDirection.Export, 2, true, 101, 12900)]
        [InlineData(4, TradeDirection.Export, 2, true, 110, 12900)]
        [InlineData(4, TradeDirection.Export, 2, true, 500, 12900)]
        [InlineData(4, TradeDirection.Export, 2, true, 501, 24000)]
        [InlineData(4, TradeDirection.Import, 1, false, 1, 1250)]
        [InlineData(4, TradeDirection.Import, 1, false, 2, 1250)]
        [InlineData(4, TradeDirection.Import, 1, false, 3, 1250)]
        [InlineData(4, TradeDirection.Import, 1, false, 5, 1250)]
        [InlineData(4, TradeDirection.Import, 1, false, 6, 2700)]
        [InlineData(4, TradeDirection.Import, 1, false, 19, 2700)]
        [InlineData(4, TradeDirection.Import, 1, false, 20, 2700)]
        [InlineData(4, TradeDirection.Import, 1, false, 21, 4900)]
        [InlineData(4, TradeDirection.Import, 1, false, 30, 4900)]
        [InlineData(4, TradeDirection.Import, 1, false, 100, 4900)]
        [InlineData(4, TradeDirection.Import, 1, false, 101, 10600)]
        [InlineData(4, TradeDirection.Import, 1, false, 110, 10600)]
        [InlineData(4, TradeDirection.Import, 1, false, 500, 10600)]
        [InlineData(4, TradeDirection.Import, 1, false, 501, 19500)]
        [InlineData(4, TradeDirection.Import, 1, true, 1, 1450)]
        [InlineData(4, TradeDirection.Import, 1, true, 2, 1450)]
        [InlineData(4, TradeDirection.Import, 1, true, 3, 1450)]
        [InlineData(4, TradeDirection.Import, 1, true, 5, 1450)]
        [InlineData(4, TradeDirection.Import, 1, true, 6, 2830)]
        [InlineData(4, TradeDirection.Import, 1, true, 19, 2830)]
        [InlineData(4, TradeDirection.Import, 1, true, 20, 2830)]
        [InlineData(4, TradeDirection.Import, 1, true, 21, 5500)]
        [InlineData(4, TradeDirection.Import, 1, true, 30, 5500)]
        [InlineData(4, TradeDirection.Import, 1, true, 100, 5500)]
        [InlineData(4, TradeDirection.Import, 1, true, 101, 12900)]
        [InlineData(4, TradeDirection.Import, 1, true, 110, 12900)]
        [InlineData(4, TradeDirection.Import, 1, true, 500, 12900)]
        [InlineData(4, TradeDirection.Import, 1, true, 501, 24000)]
        [InlineData(4, TradeDirection.Import, 2, false, 1, 1540)]
        [InlineData(4, TradeDirection.Import, 2, false, 2, 1540)]
        [InlineData(4, TradeDirection.Import, 2, false, 3, 1540)]
        [InlineData(4, TradeDirection.Import, 2, false, 5, 1540)]
        [InlineData(4, TradeDirection.Import, 2, false, 6, 3330)]
        [InlineData(4, TradeDirection.Import, 2, false, 19, 3330)]
        [InlineData(4, TradeDirection.Import, 2, false, 20, 3330)]
        [InlineData(4, TradeDirection.Import, 2, false, 21, 5500)]
        [InlineData(4, TradeDirection.Import, 2, false, 30, 5500)]
        [InlineData(4, TradeDirection.Import, 2, false, 100, 5500)]
        [InlineData(4, TradeDirection.Import, 2, false, 101, 10600)]
        [InlineData(4, TradeDirection.Import, 2, false, 110, 10600)]
        [InlineData(4, TradeDirection.Import, 2, false, 500, 10600)]
        [InlineData(4, TradeDirection.Import, 2, false, 501, 19500)]
        [InlineData(4, TradeDirection.Import, 2, true, 1, 1700)]
        [InlineData(4, TradeDirection.Import, 2, true, 2, 1700)]
        [InlineData(4, TradeDirection.Import, 2, true, 3, 1700)]
        [InlineData(4, TradeDirection.Import, 2, true, 5, 1700)]
        [InlineData(4, TradeDirection.Import, 2, true, 6, 3330)]
        [InlineData(4, TradeDirection.Import, 2, true, 19, 3330)]
        [InlineData(4, TradeDirection.Import, 2, true, 20, 3330)]
        [InlineData(4, TradeDirection.Import, 2, true, 21, 6000)]
        [InlineData(4, TradeDirection.Import, 2, true, 30, 6000)]
        [InlineData(4, TradeDirection.Import, 2, true, 100, 6000)]
        [InlineData(4, TradeDirection.Import, 2, true, 101, 12900)]
        [InlineData(4, TradeDirection.Import, 2, true, 110, 12900)]
        [InlineData(4, TradeDirection.Import, 2, true, 500, 12900)]
        [InlineData(4, TradeDirection.Import, 2, true, 501, 24000)]
        public async Task PricingStructureCorrectValue(int ca, TradeDirection td, int nt, bool isInterim,
            int numberOfShipments, int expectedPrice)
        {
            var result = (await context.PricingStructures.SingleAsync(
                p => p.CompetentAuthority.Value == ca &&
                     p.Activity.TradeDirection == td &&
                     p.Activity.NotificationType.Value == nt &&
                     p.Activity.IsInterim == isInterim &&
                     (p.ShipmentQuantityRange.RangeFrom <= numberOfShipments &&
                      (p.ShipmentQuantityRange.RangeTo == null || p.ShipmentQuantityRange.RangeTo >= numberOfShipments))))
                .Price;

            Assert.Equal(expectedPrice, result);
        }
    }
}
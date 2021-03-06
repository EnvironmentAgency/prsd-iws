﻿namespace EA.Iws.DataAccess.Tests.Integration
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.Shared;
    using Domain;
    using Domain.TransportRoute;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using Xunit;
    using NotificationApplicationFactory = TestHelpers.Helpers.NotificationApplicationFactory;

    [Trait("Category", "Integration")]
    public class TransportRouteIntegration
    {
        private readonly IwsContext context;
        private readonly ITransportRouteValidator validator;

        public TransportRouteIntegration()
        {
            var userContext = A.Fake<IUserContext>();

            A.CallTo(() => userContext.UserId).Returns(Guid.NewGuid());

            context = new IwsContext(userContext, A.Fake<IEventDispatcher>());

            this.validator = A.Fake<ITransportRouteValidator>();
        }

        [Fact]
        public async Task CanAddStateOfExport()
        {
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Disposal, UKCompetentAuthority.England, 450);

            context.NotificationApplications.Add(notification);

            await context.SaveChangesAsync();

            var transport = new TransportRoute(notification.Id);

            context.TransportRoutes.Add(transport);

            await context.SaveChangesAsync();

            var exitPoint = await context.EntryOrExitPoints.FirstAsync();

            var country = exitPoint.Country;

            var competentAuthority = await context.CompetentAuthorities.FirstAsync(ca => ca.Country.Id == country.Id);

            var stateOfExport = new StateOfExport(country, competentAuthority, exitPoint);

            IntraCountryExportAllowed[] intraCountryExportAlloweds = new IntraCountryExportAllowed[0];

            A.CallTo(() => this.validator.IsImportAndExportStatesCombinationValid(null, stateOfExport)).Returns(true);

            transport.SetStateOfExportForNotification(stateOfExport, this.validator);

            await context.SaveChangesAsync();

            Assert.Equal(country.Id, transport.StateOfExport.Country.Id);
            Assert.Equal(competentAuthority.Id, transport.StateOfExport.CompetentAuthority.Id);
            Assert.Equal(exitPoint.Id, transport.StateOfExport.ExitPoint.Id);

            DatabaseDataDeleter.DeleteDataForNotification(notification.Id, context);
        }

        [Fact]
        public async Task CanUpdateStateOfExport()
        {
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Disposal, UKCompetentAuthority.England, 450);

            context.NotificationApplications.Add(notification);

            await context.SaveChangesAsync();

            var transport = new TransportRoute(notification.Id);

            context.TransportRoutes.Add(transport);

            await context.SaveChangesAsync();

            var exitPoint = context.EntryOrExitPoints.First();

            var country = exitPoint.Country;

            var competentAuthority = context.CompetentAuthorities.First(ca => ca.Country.Id == country.Id);

            var stateOfExport = new StateOfExport(country, competentAuthority, exitPoint);

            IntraCountryExportAllowed[] intraCountryExportAlloweds = new IntraCountryExportAllowed[0];
            A.CallTo(() => this.validator.IsImportAndExportStatesCombinationValid(null, stateOfExport)).Returns(true);

            transport.SetStateOfExportForNotification(stateOfExport, this.validator);

            await context.SaveChangesAsync();

            var nextExitPoint = context.EntryOrExitPoints.First(ep => ep.Id != exitPoint.Id);

            if (nextExitPoint.Country.Id != country.Id)
            {
                country = nextExitPoint.Country;
                competentAuthority = context.CompetentAuthorities.First(ca => ca.Country.Id == country.Id);
            }

            var newStateOfExport = new StateOfExport(country, competentAuthority, nextExitPoint);
            A.CallTo(() => this.validator.IsImportAndExportStatesCombinationValid(null, newStateOfExport)).Returns(true);

            transport.SetStateOfExportForNotification(newStateOfExport, this.validator);

            await context.SaveChangesAsync();

            Assert.Equal(nextExitPoint.Id, transport.StateOfExport.ExitPoint.Id);

            DatabaseDataDeleter.DeleteDataForNotification(notification.Id, context);
        }
    }
}

﻿namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using RequestHandlers.Admin.NotificationAssessment;
    using Requests.Admin.NotificationAssessment;
    using TestHelpers.Helpers;
    using Xunit;

    public class SetDatesHandlerTests
    {
        private readonly IwsContext context;
        private readonly SetDatesHandler handler;
        private readonly NotificationDates notificationDates;
        private readonly Guid notificationId = new Guid("5243D3E5-CA81-4A3E-B589-4D22D6676B28");
        private readonly Guid assessmentId = new Guid("6F3C6D5A-9914-4C2E-B432-E6BB2AA1BD2C");

        public SetDatesHandlerTests()
        {
            context = new TestIwsContext();
            handler = new SetDatesHandler(context);
            notificationDates = ObjectInstantiator<NotificationDates>.CreateNew();
            EntityHelper.SetEntityId(notificationDates, notificationId);

            var notification = new NotificationApplication(Guid.Empty, NotificationType.Recovery, UKCompetentAuthority.England, 0);
            EntityHelper.SetEntityId(notification, notificationId);

            context.NotificationApplications.Add(notification);

            var assessment = new NotificationAssessment(notificationId);
            EntityHelper.SetEntityId(assessment, assessmentId);
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.Dates, notificationDates, assessment);
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.NotificationApplicationId, notificationId, assessment);

            context.NotificationAssessments.Add(assessment);
        }

        [Fact]
        public async Task SetDates()
        {
            var acknowledgedDate = new DateTime(2015, 1, 1);
            var decisionDate = new DateTime(2015, 1, 3);

            var request = new SetDates
            {
                NotificationApplicationId = notificationId,
                AcknowledgedDate = acknowledgedDate,
                DecisionDate = decisionDate
            };

            await handler.HandleAsync(request);

            Assert.True(notificationDates.AcknowledgedDate == acknowledgedDate
                        && notificationDates.DecisionDate == decisionDate);
        }
    }
}

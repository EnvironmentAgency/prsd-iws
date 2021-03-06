﻿namespace EA.Iws.RequestHandlers.Tests.Unit.Notification
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.NotificationAssessment;
    using Core.Shared;
    using DataAccess;
    using Domain.NotificationApplication;
    using RequestHandlers.Notification;
    using TestHelpers.Helpers;
    using Xunit;
    using NotificationApplicationFactory = TestHelpers.Helpers.NotificationApplicationFactory;

    public class NotificationCreatedEventHandlerTests
    {
        private readonly IwsContext context;
        private readonly NotificationCreatedEventHandler handler;
        private readonly NotificationCreatedEvent message;
        private readonly Guid notificationId = new Guid("00452356-08C3-4D20-A9E4-552B0FE00864");

        public NotificationCreatedEventHandlerTests()
        {
            context = new TestIwsContext();

            var notification = NotificationApplicationFactory.Create(Guid.Empty, NotificationType.Recovery,
                UKCompetentAuthority.England, 0);
            EntityHelper.SetEntityId(notification, notificationId);

            message = new NotificationCreatedEvent(notification);
            handler = new NotificationCreatedEventHandler(context);
        }

        [Fact]
        public async Task NotificationAssessmentIsCreated()
        {
            await handler.HandleAsync(message);

            Assert.Equal(1, context.NotificationAssessments.Count(p => p.NotificationApplicationId == notificationId));
        }

        [Fact]
        public async Task NotificationAssessmentIsNotSubmitted()
        {
            await handler.HandleAsync(message);

            Assert.Equal(NotificationStatus.NotSubmitted,
                context.NotificationAssessments.Single(p => p.NotificationApplicationId == notificationId).Status);
        }

        [Fact]
        public async Task FinancialGuaranteeIsCreated()
        {
            await handler.HandleAsync(message);

            Assert.Single(context.FinancialGuarantees, fg => fg.NotificationId == notificationId);
        }
    }
}
﻿namespace EA.Iws.Web.Tests.Unit.Controllers.Movement
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.Movement.Controllers;
    using Areas.Movement.ViewModels;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using TestHelpers.Factories;
    using Xunit;

    public class OperationCompleteControllerTests
    {
        private static readonly Guid MovementId = new Guid("3509A56B-FA19-4A50-9C44-FB43E490EC68");
        private static readonly Guid AnyGuid = new Guid("29C4BF73-017B-4C2C-BC7C-0E2EEBF5CAF3");

        private readonly IMediator mediator;
        private readonly OperationCompleteController controller;

        public OperationCompleteControllerTests()
        {
            mediator = A.Fake<IMediator>();

            controller = new OperationCompleteController(mediator);    
        }

        [Fact]
        public async Task GetCallsCorrectRequest()
        {
            await controller.Index(MovementId);

            A.CallTo(() => 
                mediator.SendAsync(
                    A<GetNotificationIdByMovementId>.That.Matches(r => 
                        r.MovementId == MovementId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task PostRedirectsToCorrectScreen()
        {
            var result = await controller.Index(AnyGuid, new OperationCompleteViewModel
            {
                NotificationId = AnyGuid,
                File = FakeHttpPostedFileFactory.CreateTestFile()
            });

            Assert.IsType<RedirectToRouteResult>(result);
            var routeResult = result as RedirectToRouteResult;

            RouteAssert.RoutesTo(routeResult.RouteValues, "ApprovedNotification", "Applicant");
        }
    }
}

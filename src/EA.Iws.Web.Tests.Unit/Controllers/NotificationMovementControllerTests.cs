﻿namespace EA.Iws.Web.Tests.Unit.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Movement;
    using Core.MovementOperation;
    using Core.Shared;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Web.Controllers;
    using Web.ViewModels.Movement;
    using Web.ViewModels.Shared;
    using Xunit;

    public class NotificationMovementControllerTests
    {
        private static readonly Guid AnyGuid = new Guid("850D92A7-4143-4146-90BB-6D5EF943C790");
        private readonly NotificationMovementController controller;
        private readonly IMediator mediator;

        public NotificationMovementControllerTests()
        {
            mediator = A.Fake<IMediator>();
            controller = new NotificationMovementController(mediator);
            A.CallTo(
                () =>
                    mediator.SendAsync(A<GetActiveMovementsWithoutReceiptCertificateByNotificationId>.Ignored))
                .Returns(new List<MovementData>());
            A.CallTo(
                () =>
                    mediator.SendAsync(A<GetActiveMovementsWithReceiptCertificateByNotificationId>.Ignored))
                .Returns(new MovementOperationData
                {
                    MovementDatas = new List<MovementData>(),
                    NotificationType = NotificationType.Recovery
                });
        }

        [Fact]
        public async Task Get_Receipt_SendsCorrectRequest()
        {
            await controller.Receipt(AnyGuid);

            A.CallTo(
                () =>
                    mediator.SendAsync(
                        A<GetActiveMovementsWithoutReceiptCertificateByNotificationId>.That.Matches(r => r.Id == AnyGuid)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Get_Receipt_ReturnsCorrectViewModel()
        {
            var result = await controller.Receipt(AnyGuid) as ViewResult;

            Assert.IsType<MovementReceiptViewModel>(result.Model);
            Assert.Equal(AnyGuid, (result.Model as MovementReceiptViewModel).NotificationId);
        }

        [Fact]
        public async Task Get_Index_ListsMovementsForNotification()
        {
            var movements = new Dictionary<int, Guid>();
            movements.Add(1, AnyGuid);

            A.CallTo(() => mediator.SendAsync(A<GetMovementsForNotificationById>.Ignored)).Returns(movements);

            var result = await controller.Index(AnyGuid);

            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;

            Assert.IsType<MovementsViewModel>(viewResult.Model);
            var model = viewResult.Model as MovementsViewModel;

            Assert.NotEmpty(model.Movements);
        }

        [Fact]
        public async Task Create_SendsRequest()
        {
            await controller.Create(Guid.Empty);

            A.CallTo(() => mediator.SendAsync(A<CreateMovementForNotificationById>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Create_RedirectsToActualDate()
        {
            var result = await controller.Create(Guid.Empty) as RedirectToRouteResult;

            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("ShipmentDate", result.RouteValues["controller"]);
        }

        [Fact]
        public async Task Create_SendsRequestWithCorrectId()
        {
            await controller.Create(Guid.Empty);

            A.CallTo(
                () => mediator.SendAsync(A<CreateMovementForNotificationById>.That.Matches(r => r.Id == Guid.Empty)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void Post_ReceiptWithModelErrors_ReturnsView()
        {
            controller.ModelState.AddModelError("a", "b");

            var result =
                controller.Receipt(AnyGuid, new MovementReceiptViewModel(AnyGuid, new List<MovementData>())) as
                    ViewResult;

            Assert.IsType<MovementReceiptViewModel>(result.Model);
            Assert.Equal(AnyGuid, (result.Model as MovementReceiptViewModel).NotificationId);
        }

        [Fact]
        public void Post_Receipt_RedirectsToAction()
        {
            var movementId = new Guid("E3CB71D2-7D9A-4C1D-A30B-0EAACFCE8545");

            var result = controller.Receipt(AnyGuid, new MovementReceiptViewModel
            {
                NotificationId = AnyGuid,
                RadioButtons = new StringGuidRadioButtons
                {
                    SelectedValue = movementId
                }
            }) as RedirectToRouteResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("DateReceived", result.RouteValues["controller"]);
            Assert.Equal("Movement", result.RouteValues["area"]);
            Assert.Equal(movementId, result.RouteValues["id"]);
        }

        [Fact]
        public async Task Get_Operation_ReturnsCorrectViewModel()
        {
            var result = await controller.Operation(AnyGuid) as ViewResult;

            Assert.IsType<MovementOperationViewModel>(result.Model);
            Assert.Equal(AnyGuid, (result.Model as MovementOperationViewModel).NotificationId);
        }

        [Fact]
        public void Post_Operation_RedirectsToAction()
        {
            var movementId = new Guid("E3CB71D2-7D9A-4C1D-A30B-0EAACFCE8545");

            var result = controller.Operation(AnyGuid, new MovementOperationViewModel
            {
                NotificationId = AnyGuid,
                RadioButtons = new StringGuidRadioButtons
                {
                    SelectedValue = movementId
                }
            }) as RedirectToRouteResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("DateComplete", result.RouteValues["controller"]);
            Assert.Equal("Movement", result.RouteValues["area"]);
            Assert.Equal(movementId, result.RouteValues["id"]);
        }
    }
}
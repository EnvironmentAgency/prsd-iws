﻿namespace EA.Iws.Web.Tests.Unit.Controllers.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.Movement.Controllers;
    using Areas.Movement.ViewModels.Carrier;
    using Core.Carriers;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Xunit;

    public class CarrierControllerTests
    {
        private readonly IMediator mediator;
        private readonly CarrierController controller;
        private static readonly Guid AnyGuid = new Guid("7B9974BC-A709-4916-A040-0DDC3FB9BE02");
        private static readonly Guid MovementId = new Guid("5E07EC6F-9314-49E7-AA4D-FEB136BBC802");
        private static readonly Guid CarrierId = new Guid("FE3F0D01-FED3-41B0-B935-3D5CC3D5581F");

        public CarrierControllerTests()
        {
            mediator = A.Fake<IMediator>();

            controller = new CarrierController(mediator);
        }

        [Fact]
        public async Task NumberOfCarriers_Get_SendsCorrectRequest()
        {
            await controller.NumberOfCarriers(AnyGuid);

            A.CallTo(() => mediator.SendAsync(A<GetNumberOfCarriersByMovementId>.That.Matches(r => r.MovementId == AnyGuid)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task NumberOfCarriers_Get_ReturnsNull_ReturnsNullInViewModel()
        {
            A.CallTo(() => mediator.SendAsync(A<GetNumberOfCarriersByMovementId>.Ignored))
                .Returns<int?>(null);

            var result = await controller.NumberOfCarriers(AnyGuid);

            Assert.IsType<ViewResult>(result);

            var viewResult = result as ViewResult;

            Assert.IsType<NumberOfCarriersViewModel>(viewResult.Model);

            var model = viewResult.Model as NumberOfCarriersViewModel;

            Assert.Equal(null, model.Amount);
        }

        [Fact]
        public async Task NumberOfCarriers_Get_ReturnsNumber_ReturnsNumberInViewModel()
        {
            A.CallTo(() => mediator.SendAsync(A<GetNumberOfCarriersByMovementId>.Ignored))
                .Returns<int?>(3);

            var result = await controller.NumberOfCarriers(AnyGuid);

            Assert.IsType<ViewResult>(result);

            var viewResult = result as ViewResult;

            Assert.IsType<NumberOfCarriersViewModel>(viewResult.Model);

            var model = viewResult.Model as NumberOfCarriersViewModel;

            Assert.Equal(3, model.Amount);
        }

        [Fact]
        public async Task NumberOfCarriers_Post_InvalidModel_ReturnsView()
        {
            controller.ModelState.AddModelError("Test", "Error");

            var result = await controller.NumberOfCarriers(AnyGuid, new NumberOfCarriersViewModel());

            Assert.IsType<ViewResult>(result);

            var viewResult = result as ViewResult;

            Assert.IsType<NumberOfCarriersViewModel>(viewResult.Model);
        }

        [Fact]
        public async Task NumberOfCarriers_Post_RedirectsToIndex_WithQuerystringValue()
        {
            var result = await controller.NumberOfCarriers(AnyGuid, new NumberOfCarriersViewModel { Amount = 3 });

            Assert.IsType<RedirectToRouteResult>(result);

            var routeResult = result as RedirectToRouteResult;

            RouteAssert.RoutesTo(routeResult.RouteValues, "Index", "Carrier");

            var numberOfCarriersKey = "numberOfCarriers";
            Assert.True(routeResult.RouteValues.ContainsKey(numberOfCarriersKey));
            Assert.Equal(3, routeResult.RouteValues[numberOfCarriersKey]);
        }

        [Fact]
        public async Task Index_Get_RedirectsToNumber_WhenNoCarriersSelected_AndNoNumberSupplied()
        {
            SetUpMovementCarrierData(new[] { new CarrierData { Id = CarrierId } });

            var result = await controller.Index(AnyGuid);

            Assert.IsType<RedirectToRouteResult>(result);

            var redirectResult = result as RedirectToRouteResult;

            RouteAssert.RoutesTo(redirectResult.RouteValues, "NumberOfCarriers", "Carrier");
        }

        [Theory]
        [InlineData(300)]
        [InlineData(0)]
        [InlineData(-5)]
        public async Task Index_Get_RedirectsToNumber_WhenNoCarriersSelected_AndInvalidNumberSupplied(int numberOfCarriers)
        {
            SetUpMovementCarrierData(new[] { new CarrierData { Id = CarrierId } });

            var result = await controller.Index(AnyGuid, numberOfCarriers);

            Assert.IsType<RedirectToRouteResult>(result);

            var redirectResult = result as RedirectToRouteResult;

            RouteAssert.RoutesTo(redirectResult.RouteValues, "NumberOfCarriers", "Carrier");
        }

        [Theory]
        [InlineData(300)]
        [InlineData(0)]
        [InlineData(-5)]
        public async Task Index_Get_RedirectsToNumber_WhenCarriersSelected_AndInvalidNumberSupplied(int numberOfCarriers)
        {
            SetUpMovementCarrierData(
                notificationCarriers: new[] { new CarrierData { Id = AnyGuid } },
                selectedCarriers: new Dictionary<int, CarrierData> { { 0, new CarrierData { Id = AnyGuid } } });

            var result = await controller.Index(AnyGuid, numberOfCarriers);

            Assert.IsType<RedirectToRouteResult>(result);

            var redirectResult = result as RedirectToRouteResult;

            RouteAssert.RoutesTo(redirectResult.RouteValues, "NumberOfCarriers", "Carrier");
        }

        [Fact]
        public async Task Index_Get_DoesNotRedirect_WhenCarriersSelected_AndNoNumberSupplied()
        {
            SetUpMovementCarrierData(
                notificationCarriers: new[] { new CarrierData { Id = AnyGuid } },
                selectedCarriers: new Dictionary<int, CarrierData> { { 0, new CarrierData { Id = AnyGuid } } });

            var result = await controller.Index(AnyGuid);

            Assert.IsType<ViewResult>(result);

            var viewResult = result as ViewResult;

            Assert.Equal(string.Empty, viewResult.ViewName);
        }

        [Fact]
        public async Task Index_Get_DoesNotRedirect_ValidNumberSupplied()
        {
            SetUpMovementCarrierData(
                notificationCarriers: new[] { new CarrierData { Id = AnyGuid } },
                selectedCarriers: new Dictionary<int, CarrierData> { { 0, new CarrierData { Id = AnyGuid } } });

            var result = await controller.Index(AnyGuid, 3);

            Assert.IsType<ViewResult>(result);

            var viewResult = result as ViewResult;

            Assert.Equal(string.Empty, viewResult.ViewName);
        }

        [Fact]
        public async Task Index_Get_SendsCorrectRequest()
        {
            SetUpMovementCarrierData(new[] { new CarrierData { Id = AnyGuid } });

            await controller.Index(AnyGuid, 3);

            A.CallTo(() => mediator.SendAsync(A<GetMovementCarrierDataByMovementId>.That.Matches(r => r.MovementId == AnyGuid)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Index_Get_ReturnsCarriersInViewModel()
        {
            SetUpMovementCarrierData(new[] { new CarrierData { Id = CarrierId } });

            var result = await controller.Index(AnyGuid, 3);

            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;

            Assert.IsType<CarrierViewModel>(viewResult.Model);
            var model = viewResult.Model as CarrierViewModel;

            Assert.Equal(CarrierId, model.NotificationCarriers.Select(c => c.Id).Single());
        }

        [Fact]
        public async Task Index_Post_ModelStateInvalid_ReturnsView()
        {
            controller.ModelState.AddModelError("Test", "Error");

            SetUpMovementCarrierData(new[] { new CarrierData { Id = CarrierId } });

            var result = await controller.Index(AnyGuid, new CarrierViewModel());

            Assert.IsType<ViewResult>(result);

            var viewResult = result as ViewResult;

            Assert.IsType<CarrierViewModel>(viewResult.Model);
        }

        [Fact]
        public async Task Index_Post_CallsCorrectRequest()
        {
            var selectedCarriers = new[] { CarrierId };

            await controller.Index(MovementId, new CarrierViewModel { SelectedItems = selectedCarriers.Select(sc => (Guid?)sc).ToList() });

            A.CallTo(
                () =>
                    mediator.SendAsync(A<SetActualMovementCarriers>
                            .That.Matches(
                                r => r.MovementId == MovementId
                                && r.SelectedCarriers.ContainsKey(0)
                                && r.SelectedCarriers.ContainsValue(CarrierId))))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task CarriersRedirectsToCorrectScreen()
        {
            var result = await controller.Index(AnyGuid, new CarrierViewModel());

            Assert.IsType<RedirectToRouteResult>(result);

            var redirectResult = result as RedirectToRouteResult;

            RouteAssert.RoutesTo(redirectResult.RouteValues, "Index", "NotificationMovement");
        }

        private void SetUpMovementCarrierData(CarrierData[] notificationCarriers, Dictionary<int, CarrierData> selectedCarriers = null)
        {
            A.CallTo(() => mediator.SendAsync(A<GetMovementCarrierDataByMovementId>.Ignored))
                .Returns(new MovementCarrierData
                {
                    NotificationCarriers = notificationCarriers,
                    SelectedCarriers = selectedCarriers ?? new Dictionary<int, CarrierData>()
                });
        }
    }
}

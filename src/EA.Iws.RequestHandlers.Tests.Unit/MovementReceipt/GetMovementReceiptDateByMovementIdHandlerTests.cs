﻿namespace EA.Iws.RequestHandlers.Tests.Unit.MovementReceipt
{
    using EA.Iws.RequestHandlers.MovementReceipt;
using EA.Iws.Requests.MovementReceipt;
using EA.Iws.TestHelpers.DomainFakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

    public class GetMovementReceiptDateByMovementIdHandlerTests
    {
        private readonly GetMovementReceiptDateByMovementIdHandler handler;
        private readonly TestableMovement movement;
        private readonly TestIwsContext context;

        private static readonly Guid MovementId = new Guid("09CF4780-D5CB-43FC-98BC-74DD9273896E");
        private static readonly DateTime MovementDate = new DateTime(2015, 6, 1);
        private static readonly DateTime DateReceived = new DateTime(2015, 9, 1);

        public GetMovementReceiptDateByMovementIdHandlerTests()
        {
            context = new TestIwsContext();
            movement = new TestableMovement 
            { 
                Id = MovementId,
                Date = MovementDate
            };
            context.Movements.Add(movement);
            handler = new GetMovementReceiptDateByMovementIdHandler(context);
        }

        [Fact]
        public async Task MovementDoesNotExistThrows()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(new GetMovementReceiptDateByMovementId(Guid.Empty)));
        }

        [Fact]
        public async Task MovementNotRecievedReturnsNull()
        {
            var result = await handler.HandleAsync(new GetMovementReceiptDateByMovementId(MovementId));

            Assert.Null(result);
        }

        [Fact]
        public async Task ReturnsDateWhenSet()
        {
            movement.Receive(DateReceived);

            var result = await handler.HandleAsync(new GetMovementReceiptDateByMovementId(MovementId));

            Assert.Equal(DateReceived, result);
        }
    }
}

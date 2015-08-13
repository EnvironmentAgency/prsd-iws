﻿namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.UserAdministration
{
    using System;
    using System.Linq;
    using System.Security;
    using System.Threading.Tasks;
    using Core.Admin;
    using DataAccess;
    using Mappings;
    using RequestHandlers.Admin.UserAdministration;
    using Requests.Admin.UserAdministration;
    using Xunit;

    public class GetNewInternalUsersHandlerTests
    {
        private readonly GetNewInternalUsersHandler handler;
        private readonly IwsContext context;
        private readonly TestUserContext userContext;
        private readonly GetNewInternalUsers message = new GetNewInternalUsers();

        public GetNewInternalUsersHandlerTests()
        {
            this.context = new TestIwsContext();
            context.Users.AddRange(new InternalUserCollection().Users);

            this.userContext = new TestUserContext(Guid.Empty);

            handler = new GetNewInternalUsersHandler(context, new InternalUserMap(), userContext);
        }

        [Fact]
        public async Task GetAsNonAdminUser_Throws()
        {
            userContext.ReturnsId = InternalUserCollection.NonAdminUserId;

            await Assert.ThrowsAsync<SecurityException>(() => handler.HandleAsync(message));
        }

        [Fact]
        public async Task GetAsNonApprovedAdminUser_Throws()
        {
            userContext.ReturnsId = InternalUserCollection.ThisUserAdminPendingId;

            await Assert.ThrowsAsync<SecurityException>(() => handler.HandleAsync(message));
        }

        [Fact]
        public async Task GetAsApprovedAdmin_ReturnsResults()
        {
            userContext.ReturnsId = InternalUserCollection.ThisUserAdminApprovedId;

            var result = await handler.HandleAsync(message);

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task Get_ReturnsOnlyPendingUsers()
        {
            userContext.ReturnsId = InternalUserCollection.ThisUserAdminApprovedId;

            var result = await handler.HandleAsync(message);

            Assert.True(result.All(u => u.Status == InternalUserStatus.Pending));
        }

        [Fact]
        public async Task Get_ReturnsExpectedUsers()
        {
            userContext.ReturnsId = InternalUserCollection.ThisUserAdminApprovedId;

            var result = await handler.HandleAsync(message);

            Assert.Contains(InternalUserCollection.AdminPendingId.ToString(), result.Select(u => u.Id));
            Assert.DoesNotContain(InternalUserCollection.ThisUserAdminApprovedId.ToString(), result.Select(u => u.Id));
        }
    }
}

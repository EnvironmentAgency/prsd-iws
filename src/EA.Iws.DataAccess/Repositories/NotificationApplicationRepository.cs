﻿namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Domain.Security;

    internal class NotificationApplicationRepository : INotificationApplicationRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization notificationApplicationAuthorization;

        public NotificationApplicationRepository(IwsContext context,
            INotificationApplicationAuthorization notificationApplicationAuthorization)
        {
            this.context = context;
            this.notificationApplicationAuthorization = notificationApplicationAuthorization;
        }

        public async Task<NotificationApplication> GetById(Guid id)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(id);
            return await context.NotificationApplications.SingleAsync(n => n.Id == id);
        } 

        public async Task<NotificationApplication> GetByMovementId(Guid movementId)
        {
            var notificationId = await context.Movements
                .Where(m => m.Id == movementId)
                .Select(m => m.NotificationId)
                .SingleAsync();

            return await GetById(notificationId);
        }

        public async Task<string> GetNumber(Guid id)
        {
            return
                await
                    context.NotificationApplications.Where(n => n.Id == id)
                        .Select(n => n.NotificationNumber)
                        .SingleAsync();
        }

        public async Task<Guid?> GetIdOrDefault(string number)
        {
            return await context.NotificationApplications.Where(n => number == n.NotificationNumber)
                .Select(n => n.Id)
                .SingleOrDefaultAsync();
        }
    }
}
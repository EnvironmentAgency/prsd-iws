﻿namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.ImportNotificationAssessment;
    using EA.Iws.Core.Admin;

    internal class ImportNotificationCommentRepository : IImportNotificationCommentRepository
    {
        private readonly ImportNotificationContext context;
        public ImportNotificationCommentRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public async Task<bool> Add(ImportNotificationComment comment)
        {
            context.ImportNotificationComments.Add(comment);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid commentId)
        {
            var comment = await context.ImportNotificationComments.FirstOrDefaultAsync(p => p.Id == commentId);

            if (comment == null)
            {
                return false;
            }
            context.ImportNotificationComments.Remove(comment);

            await context.SaveChangesAsync();

            return true;
        }
        
        public async Task<List<ImportNotificationComment>> GetComments(Guid notificationId, NotificationShipmentsCommentsType type, DateTime startDate, DateTime endDate, int shipmentNumber, string user)
        {
            var allCommentsForType = await this.GetCommentsByType(notificationId, type);

            DateTime endDateForQuery = endDate == DateTime.MaxValue ? endDate : endDate.AddDays(1);

            if (shipmentNumber != default(int))
            {
                return allCommentsForType.Where(p => p.DateAdded >= startDate && p.DateAdded < endDateForQuery && p.ShipmentNumber == shipmentNumber).ToList();
            }

            if (user != null)
            {
                return allCommentsForType.Where(p => p.DateAdded >= startDate && p.DateAdded < endDateForQuery && p.UserId == user).ToList();
            }

            return allCommentsForType.Where(p => p.DateAdded >= startDate && p.DateAdded < endDateForQuery).ToList();
        }

        public async Task<List<ImportNotificationComment>> GetPagedComments(Guid notificationId, NotificationShipmentsCommentsType type, int pageNumber, int pageSize, DateTime startDate, DateTime endDate, int shipmentNumber, string user)
        {
            var allCommentsForType = await this.GetCommentsByType(notificationId, type);

            DateTime endDateForQuery = endDate == DateTime.MaxValue ? endDate : endDate.AddDays(1);

            var returnComments = allCommentsForType.Where(p => p.DateAdded >= startDate && p.DateAdded < endDateForQuery);
            if (shipmentNumber != default(int))
            {
                returnComments = returnComments.Where(p => p.ShipmentNumber == shipmentNumber);
            }

            if (user != null)
            {
                returnComments = returnComments.Where(p => p.UserId == user);
            }

            return returnComments
                   .OrderByDescending(x => x.ShipmentNumber)
                    .ThenByDescending(x => x.DateAdded)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
        }

        public async Task<int> GetTotalNumberOfComments(Guid notificationId, NotificationShipmentsCommentsType type)
        {
            var allCommentsForType = await this.GetCommentsByType(notificationId, type);

            return allCommentsForType.Count();
        }

        public async Task<List<string>> GetUsers(Guid notificationId, NotificationShipmentsCommentsType type)
        {
            var allComments = await this.GetCommentsByType(notificationId, type);

            return allComments.Select(p => p.UserId).Distinct().ToList();
        }

        private async Task<IEnumerable<ImportNotificationComment>> GetCommentsByType(Guid notificationId, NotificationShipmentsCommentsType type)
        {
            if (type == NotificationShipmentsCommentsType.Notification)
            {
                return await context.ImportNotificationComments.Where(p => p.NotificationId == notificationId && p.ShipmentNumber == 0).ToListAsync();
            }
            return await context.ImportNotificationComments.Where(p => p.NotificationId == notificationId && p.ShipmentNumber != 0).ToListAsync();
        }

        public async Task<int> GetCommentsCountForImportNotification(Guid notificationId)
        {
            return await context.ImportNotificationComments.CountAsync(p => p.NotificationId == notificationId);
        }
    }
}

﻿namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;

    internal class NotificationApplicationOverviewRepository : INotificationApplicationOverviewRepository
    {
        private readonly INotificationProgressService progressService;
        private readonly NotificationChargeCalculator chargeCalculator;
        private readonly IwsContext db;

        public NotificationApplicationOverviewRepository(
            IwsContext db,
            NotificationChargeCalculator chargeCalculator,
            INotificationProgressService progressService)
        {
            this.db = db;
            this.chargeCalculator = chargeCalculator;
            this.progressService = progressService;
        }

        public async Task<NotificationApplicationOverview> GetById(Guid notificationId)
        {
            var query =
                from notification in db.NotificationApplications
                where notification.Id == notificationId
                from wasteRecovery
                //left join waste recovery, if it exists
                in db.WasteRecoveries
                    .Where(wr => wr.NotificationId == notification.Id)
                    .DefaultIfEmpty()
                from assessment
                //left join assessment, if it exists
                in db.NotificationAssessments
                    .Where(na => na.NotificationApplicationId == notification.Id)
                    .DefaultIfEmpty()
                from wasteDiposal
                in db.WasteDisposals
                    .Where(wd => wd.NotificationId == notification.Id)
                    .DefaultIfEmpty()
                from shipmentInfo
                in db.ShipmentInfos
                    .Where(si => si.NotificationId == notification.Id)
                    .DefaultIfEmpty()
                select new
                {
                    Notification = notification,
                    WasteRecovery = wasteRecovery,
                    WasteDisposal = wasteDiposal,
                    NotificationAssessment = assessment,
                    ShipmentInfo = shipmentInfo
                };

            var data = await query.SingleAsync();

            var pricingStructures = await db.PricingStructures.ToArrayAsync();

            return NotificationApplicationOverview.Load(
                data.Notification,
                data.NotificationAssessment,
                data.WasteRecovery,
                data.WasteDisposal,
                decimal.ToInt32(
                    chargeCalculator.GetValue(pricingStructures, data.Notification, data.ShipmentInfo)),
                progressService.GetNotificationProgressInfo(notificationId));
        }
    }
}

﻿namespace EA.Iws.RequestHandlers.ImportNotificationMovements
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.ImportMovement;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationMovements;
    using MovementsSummary = Core.ImportNotificationMovements.MovementsSummary;

    internal class GetImportMovementsSummaryTableHandler : IRequestHandler<GetImportMovementsSummaryTable, MovementsSummary>
    {
        private readonly IImportNotificationRepository notificationRepository;
        private readonly IImportMovementTableDataRepository tableDataRepository;
        private readonly IMap<IEnumerable<MovementTableData>, IEnumerable<Core.ImportNotificationMovements.MovementTableData>> mapper;

        public GetImportMovementsSummaryTableHandler(IImportNotificationRepository notificationRepository,
            IImportMovementTableDataRepository tableDataRepository,
            IMap<IEnumerable<MovementTableData>, IEnumerable<Core.ImportNotificationMovements.MovementTableData>> mapper)
        {
            this.notificationRepository = notificationRepository;
            this.tableDataRepository = tableDataRepository;
            this.mapper = mapper;
        }

        public async Task<MovementsSummary> HandleAsync(GetImportMovementsSummaryTable message)
        {
            var type = await notificationRepository.GetTypeById(message.ImportNotificationId);
            var tableData = await tableDataRepository.GetById(message.ImportNotificationId);

            if (message.Status.HasValue)
            {
                tableData = tableData.Where(u => u.Status == message.Status);
            }

            var movementsSummary = new MovementsSummary
            {
                ImportNotificationId = message.ImportNotificationId,
                NotificationType = type,
                TableData = mapper.Map(tableData).ToList()
            };

            return movementsSummary;
        }
    }
}

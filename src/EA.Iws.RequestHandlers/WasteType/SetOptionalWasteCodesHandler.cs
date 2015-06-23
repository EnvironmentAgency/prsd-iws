﻿namespace EA.Iws.RequestHandlers.WasteType
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteType;
    using CodeType = Domain.Notification.CodeType;
    internal class SetOptionalWasteCodesHandler : IRequestHandler<SetOptionalWasteCodes, Guid>
    {
        private readonly IwsContext db;
        private readonly IMap<Requests.WasteType.CodeType, CodeType> mapper;
        public SetOptionalWasteCodesHandler(IwsContext db, IMap<Requests.WasteType.CodeType, CodeType> mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }
        public async Task<Guid> HandleAsync(SetOptionalWasteCodes command)
        {
            var notification = await db.NotificationApplications.Include(n => n.ShipmentInfo).SingleAsync(n => n.Id == command.NotificationId);
            foreach (var optionalWasteCode in command.OptionalWasteCodes)
            {
                var code = mapper.Map(optionalWasteCode.CodeType);
                var wasteCode = await db.WasteCodes.SingleAsync(w => w.CodeType.Value == code.Value);

                notification.AddWasteCode(wasteCode, optionalWasteCode.OptionalCode, optionalWasteCode.OptionalDescription);
            }
            await db.SaveChangesAsync();
            return notification.WasteType.Id;
        }
    }
}
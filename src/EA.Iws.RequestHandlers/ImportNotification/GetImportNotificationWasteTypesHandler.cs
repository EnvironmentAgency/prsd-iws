﻿namespace EA.Iws.RequestHandlers.ImportNotification
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ImportNotification.Update;
    using Core.WasteCodes;
    using DataAccess;
    using Domain.ImportNotification;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;

    internal class GetImportNotificationWasteTypesHandler : IRequestHandler<GetImportNotificationWasteTypes, WasteTypes>
    {
        private readonly ImportNotificationContext context;
        private readonly IWasteTypeRepository wasteTypeRepository;
        private readonly IWasteCodeRepository wasteCodeRepository;
        private readonly IMapper mapper;
        
        public GetImportNotificationWasteTypesHandler(ImportNotificationContext context, 
            IWasteCodeRepository wasteCodeRepository, IWasteTypeRepository wasteTypeRepository, IMapper mapper)
        {
            this.context = context;
            this.wasteTypeRepository = wasteTypeRepository;
            this.wasteCodeRepository = wasteCodeRepository;
            this.mapper = mapper;
        }

        public async Task<WasteTypes> HandleAsync(GetImportNotificationWasteTypes message)
        {
            var wasteTypes = await wasteTypeRepository.GetByNotificationId(message.ImportNotificationId);
            var wasteCodeData =
                (await wasteCodeRepository.GetAllWasteCodes()).Select(wasteCode => mapper.Map<WasteCodeData>(wasteCode))
                    .ToList();

            return mapper.Map<WasteTypes>(wasteTypes, wasteCodeData);
        }
    }
}

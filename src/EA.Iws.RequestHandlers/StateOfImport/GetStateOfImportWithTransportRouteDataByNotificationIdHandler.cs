﻿namespace EA.Iws.RequestHandlers.StateOfImport
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using Core.StateOfExport;
    using Core.StateOfImport;
    using Core.TransitState;
    using DataAccess;
    using Domain;
    using Domain.TransportRoute;
    using EA.Iws.Domain.NotificationApplication;
    using EA.Iws.Requests.TransportRoute;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.StateOfImport;

    internal class GetStateOfImportWithTransportRouteDataByNotificationIdHandler : IRequestHandler<GetStateOfImportWithTransportRouteDataByNotificationId, StateOfImportWithTransportRouteData>
    {
        private readonly IwsContext context;
        private readonly IMapper mapper;
        private readonly ITransportRouteRepository transportRouteRepository;
        private readonly ICompetentAuthorityRepository competentAuthorityRepository;
        private readonly ICountryRepository countryRepository;
        private readonly IIntraCountryExportAllowedRepository intraCountryExportAllowedRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly IRequestHandler<GetCompetentAuthoritiesAndEntryPointsByCountryId, CompetentAuthorityAndEntryOrExitPointData> getCompetentAuthoritiesAndEntryPointsByCountryIdHandler;

        public GetStateOfImportWithTransportRouteDataByNotificationIdHandler(IwsContext context,
            IMapper mapper,
            ITransportRouteRepository transportRouteRepository,
            ICompetentAuthorityRepository competentAuthorityRepository,
            ICountryRepository countryRepository,
            IIntraCountryExportAllowedRepository intraCountryExportAllowedRepository,
            INotificationApplicationRepository notificationApplicationRepository,
            IRequestHandler<GetCompetentAuthoritiesAndEntryPointsByCountryId, CompetentAuthorityAndEntryOrExitPointData> getCompetentAuthoritiesAndEntryPointsByCountryIdHandler)
        {
            this.context = context;
            this.mapper = mapper;
            this.transportRouteRepository = transportRouteRepository;
            this.competentAuthorityRepository = competentAuthorityRepository;
            this.countryRepository = countryRepository;
            this.intraCountryExportAllowedRepository = intraCountryExportAllowedRepository;
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.getCompetentAuthoritiesAndEntryPointsByCountryIdHandler = getCompetentAuthoritiesAndEntryPointsByCountryIdHandler;
        }

        public async Task<StateOfImportWithTransportRouteData> HandleAsync(GetStateOfImportWithTransportRouteDataByNotificationId message)
        {
            var transportRoute = await transportRouteRepository.GetByNotificationId(message.Id);
            var countries = await countryRepository.GetAllHavingCompetentAuthorities();

            var data = new StateOfImportWithTransportRouteData();

            if (transportRoute != null)
            {
                data.StateOfImport = mapper.Map<StateOfImportData>(transportRoute.StateOfImport);
                data.StateOfExport = mapper.Map<StateOfExportData>(transportRoute.StateOfExport);
                data.TransitStates = transportRoute.TransitStates.Select(t => mapper.Map<TransitStateData>(t)).ToList();
                var notification = await notificationApplicationRepository.GetById(message.Id);

                if (transportRoute.StateOfImport != null)
                {
                    var selectedCountryModel = new GetCompetentAuthoritiesAndEntryPointsByCountryId(transportRoute.StateOfImport.Country.Id, notification.CompetentAuthority);
                    var dataForSelectedCountry = await this.getCompetentAuthoritiesAndEntryPointsByCountryIdHandler.HandleAsync(selectedCountryModel);
                    data.CompetentAuthorities = dataForSelectedCountry.CompetentAuthorities;
                    data.EntryPoints = dataForSelectedCountry.EntryOrExitPoints;
                }

                var allowed = await intraCountryExportAllowedRepository.GetImportCompetentAuthorities(notification.CompetentAuthority);
                data.IntraCountryExportAllowed = allowed.Select(a => mapper.Map<IntraCountryExportAllowedData>(a)).ToArray();
            }
            
            data.Countries = countries.Select(c => mapper.Map<CountryData>(c)).ToArray();

            return data;
        }
    }
}

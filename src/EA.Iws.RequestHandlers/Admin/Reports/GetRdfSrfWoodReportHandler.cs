﻿namespace EA.Iws.RequestHandlers.Admin.Reports
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Domain;
    using Domain.Reports;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Admin.Reports;

    internal class GetRdfSrfWoodReportHandler : IRequestHandler<GetRdfSrfWoodReport, RdfSrfWoodData[]>
    {
        private readonly IRdfSrfWoodRepository repository;
        private readonly IUserContext userContext;
        private readonly IInternalUserRepository internalUserRepository;

        public GetRdfSrfWoodReportHandler(IRdfSrfWoodRepository repository, IUserContext userContext, IInternalUserRepository internalUserRepository)
        {
            this.repository = repository;
            this.userContext = userContext;
            this.internalUserRepository = internalUserRepository;
        }

        public async Task<RdfSrfWoodData[]> HandleAsync(GetRdfSrfWoodReport message)
        {
            var user = await internalUserRepository.GetByUserId(userContext.UserId);
            return (await repository.Get(message.From, message.To, message.ChemicalComposition, user.CompetentAuthority)).ToArray();
        }
    }
}
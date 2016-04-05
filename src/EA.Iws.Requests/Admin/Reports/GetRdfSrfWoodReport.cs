﻿namespace EA.Iws.Requests.Admin.Reports
{
    using System;
    using Core.Admin.Reports;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.WasteType;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ReportingPermissions.CanViewRdfSrfWoodReport)]
    public class GetRdfSrfWoodReport : IRequest<RdfSrfWoodData[]>
    {
        public GetRdfSrfWoodReport(DateTime from, DateTime to, ChemicalComposition chemicalComposition)
        {
            From = from;
            To = to;
            ChemicalComposition = chemicalComposition;
        }

        public DateTime From { get; private set; }

        public DateTime To { get; private set; }

        public ChemicalComposition ChemicalComposition { get; private set; }
    }
}
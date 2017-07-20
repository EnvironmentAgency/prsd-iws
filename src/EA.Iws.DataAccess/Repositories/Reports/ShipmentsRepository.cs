﻿namespace EA.Iws.DataAccess.Repositories.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.Reports;
    using Domain.Reports;

    internal class ShipmentsRepository : IShipmentsRepository
    {
        private readonly IwsContext context;

        public ShipmentsRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Shipment>> Get(DateTime from, DateTime to, UKCompetentAuthority competentAuthority, ShipmentsReportDates dateType)
        {
            return await context.Database.SqlQuery<Shipment>(
                @"SELECT 
                    [NotificationNumber],
                    [ImportOrExport],
                    [Exporter],
                    [Importer],
                    [Facility],
                    [BaselOecdCode],
                    [ShipmentNumber],
                    [ActualDateOfShipment],
                    [ConsentFrom],
                    [ConsentTo],
                    [PrenotificationDate],
                    [ReceivedDate],
                    [CompletedDate],
                    [QuantityReceived],
                    [QuantityReceivedUnitId] AS [Units],
                    [ChemicalComposition],
                    [LocalArea],
                    [TotalQuantity],
                    [TotalQuantityUnitsId],
                    [EntryPort],
                    [DestinationCountry],
                    [ExitPort],
                    [OriginatingCountry],
                    [Status],
                    [EwcCodes],
                    [OperationCodes],
                    [YCode],
                    [HCode],
                    [UNClass]
                FROM [Reports].[ShipmentsCache]
                WHERE [CompetentAuthorityId] = @ca
                AND (@dateType = 'NotificationReceivedDate' and  [NotificationReceivedDate] BETWEEN @from AND @to
                     OR @dateType = 'ConsentFrom' and  [ConsentFrom] BETWEEN @from AND @to
                     OR @dateType = 'ReceivedDate' and  [ReceivedDate] BETWEEN @from AND @to
                     OR @dateType = 'CompletedDate' and  [CompletedDate] BETWEEN @from AND @to
                     OR @dateType = 'ActualDateOfShipment' and  [ActualDateOfShipment] BETWEEN @from AND @to)",
                new SqlParameter("@from", from),
                new SqlParameter("@to", to),
                new SqlParameter("@ca", (int)competentAuthority),
                new SqlParameter("@dateType", dateType.ToString())).ToArrayAsync();
        }
    }
}
﻿namespace EA.Iws.DataAccess.Repositories.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Core.Notification;
    using Core.Reports;
    using Core.WasteType;
    using Domain.Reports;

    internal class FreedomOfInformationRepository : IFreedomOfInformationRepository
    {
        private readonly IwsContext context;

        public FreedomOfInformationRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<FreedomOfInformationData>> Get(DateTime from, DateTime to,
            ChemicalComposition chemicalComposition, UKCompetentAuthority competentAuthority, FoiReportDates dateType)
        {
            return await context.Database.SqlQuery<FreedomOfInformationData>(
                @"SELECT 
                    [NotificationNumber],
                    [NotifierName],
                    [NotifierAddress],
                    [ProducerName],
                    [ProducerAddress],
                    [PointOfExport],
                    [PointOfEntry],
                    [ImportCountryName],
                    [NameOfWaste],
                    [EWC],
                    [YCode],
                    [OperationCodes],
                    [ImporterName],
                    [ImporterAddress],
                    [FacilityName],
                    [FacilityAddress],
                    COALESCE(                    
                        (SELECT	SUM(
                            CASE WHEN [QuantityReceivedUnitId] IN (1, 2) -- Tonnes / Cubic Metres
                                THEN COALESCE([QuantityReceived], 0)
                            ELSE 
                                COALESCE([QuantityReceived] / 1000, 0) -- Convert to Tonnes / Cubic Metres
                            END
                            ) 
                            FROM [Reports].[Movements]
                            WHERE Id = NotificationId
                                AND (@dateType = 'NotificationReceivedDate'
				                     OR @dateType = 'ConsentFrom'
				                     OR @dateType = 'ReceivedDate' and  [ReceivedDate] BETWEEN @from AND @to
				                     OR @dateType = 'CompletedDate' and  [CompletedDate] BETWEEN @from AND @to)
                        ), 0) AS [QuantityReceived],
                    CASE WHEN [IntendedQuantityUnitId] IN (1, 2) -- Due to conversion units will only be Tonnes / Cubic Metres
                        THEN [IntendedQuantityUnit] 
                    WHEN [IntendedQuantityUnitId] = 3 THEN 'Tonnes'
                    WHEN [IntendedQuantityUnitId] = 4 THEN 'Cubic Metres'
                    END AS [QuantityReceivedUnit],
                    [IntendedQuantity],
                    [IntendedQuantityUnit],
                    [ConsentFrom],
                    [ConsentTo],
                    [LocalArea]
                FROM 
                    [Reports].[FreedomOfInformation]
                WHERE 
                    [CompetentAuthorityId] = @competentAuthority
                    AND [ChemicalCompositionTypeId] = @chemicalComposition
                    AND (@dateType = 'NotificationReceivedDate' AND  [NotificationReceivedDate] BETWEEN @from AND @to
				         OR @dateType = 'ConsentFrom' AND  [ConsentFrom] BETWEEN @from AND @to
                         OR @dateType = 'ReceivedDate' AND  
                                            EXISTS (SELECT [MovementId] FROM [Reports].[Movements]
                                            WHERE Id = NotificationId AND [ReceivedDate] BETWEEN @from AND @to)
				         OR @dateType = 'CompletedDate' AND  
                                            EXISTS (SELECT [MovementId] FROM [Reports].[Movements]
                                            WHERE Id = NotificationId AND [CompletedDate] BETWEEN @from AND @to))",
                new SqlParameter("@from", from),
                new SqlParameter("@to", to),
                new SqlParameter("@chemicalComposition", (int)chemicalComposition),
                new SqlParameter("@competentAuthority", (int)competentAuthority),
                new SqlParameter("@dateType", dateType.ToString())).ToArrayAsync();
        }
    }
}
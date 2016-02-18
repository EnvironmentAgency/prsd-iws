﻿namespace EA.Iws.RequestHandlers.Notification
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationsByUserHandler :
        IRequestHandler<GetNotificationsByUser, IList<NotificationApplicationSummaryData>>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public GetNotificationsByUserHandler(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<IList<NotificationApplicationSummaryData>> HandleAsync(GetNotificationsByUser message)
        {
            return await context.Database.SqlQuery<NotificationApplicationSummaryData>(@"
                SELECT 
                    N.Id,
                    N.NotificationNumber,
                    NA.Status,
                    E.Name AS Exporter,
                    I.Name AS Importer,
                    P.Name AS Producer
                FROM 
                    [Notification].[Notification] N
                    INNER JOIN [Notification].[NotificationAssessment] NA ON N.Id = NA.NotificationApplicationId
                    LEFT JOIN [Notification].[Exporter] E ON N.Id = E.NotificationId
                    LEFT JOIN [Notification].[Importer] I ON N.Id = I.NotificationId
                    LEFT JOIN [Notification].[ProducerCollection] PC ON N.Id = PC.NotificationId
                    LEFT JOIN [Notification].[Producer] P ON PC.Id = P.ProducerCollectionId AND P.IsSiteOfExport = 1
                WHERE 
                    N.UserId = @Id
                ORDER BY
                    N.NotificationNumber ASC", 
                new SqlParameter("@Id", userContext.UserId)).ToListAsync();
        }
    }
}
﻿namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.ImportNotificationAssessment.Transactions;
    using Domain.Security;

    internal class ImportNotificationTransactionRepository : IImportNotificationTransactionRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public ImportNotificationTransactionRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<IEnumerable<ImportNotificationTransaction>> GetTransactions(Guid importNotificationId)
        {
            await authorization.EnsureAccessAsync(importNotificationId);
            return
                await
                    context.ImportNotificationTransactions.Where(t => t.NotificationId == importNotificationId)
                        .ToListAsync();
        }

        public void Add(ImportNotificationTransaction transaction)
        {
            context.ImportNotificationTransactions.Add(transaction);
        }

        public async Task DeleteById(Guid transactionId)
        {
            var transaction = await context.ImportNotificationTransactions.Where(t => t.Id == transactionId).SingleAsync();

            context.ImportNotificationTransactions.Remove(transaction);
        }
    }
}

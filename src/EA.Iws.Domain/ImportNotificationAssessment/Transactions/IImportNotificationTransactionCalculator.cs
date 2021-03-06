﻿namespace EA.Iws.Domain.ImportNotificationAssessment.Transactions
{
    using System;
    using System.Threading.Tasks;

    public interface IImportNotificationTransactionCalculator
    {
        Task<decimal> TotalPaid(Guid importNotificationId);

        Task<bool> PaymentIsNowFullyReceived(Guid importNotificationId, decimal credit);

        Task<decimal> Balance(Guid importNotificationId);

        Task<ImportNotificationTransaction> LatestPayment(Guid notificationId);

        Task<ImportNotificationTransaction> PaymentReceivedDate(Guid notificationId);
    }
}
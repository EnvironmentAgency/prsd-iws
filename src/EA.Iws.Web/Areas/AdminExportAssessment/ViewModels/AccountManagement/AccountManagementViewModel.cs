﻿namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.AccountManagement
{
    using System.Collections.Generic;
    using Core.NotificationAssessment;
    using Core.Shared;

    public class AccountManagementViewModel
    {
        public AccountManagementViewModel(AccountManagementData data)
        {
            TotalBillable = data.TotalBillable;
            Balance = data.Balance;
            TableData = data.PaymentHistory;
        }

        public decimal TotalBillable { get; set; }

        public decimal Balance { get; set; }

        public IList<TransactionRecordData> TableData { get; set; }
    }
}
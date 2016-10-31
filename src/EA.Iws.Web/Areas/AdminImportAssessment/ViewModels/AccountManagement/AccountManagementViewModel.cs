﻿namespace EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.AccountManagement
{
    using System.Collections.Generic;
    using Core.ImportNotificationAssessment.Transactions;
    using Core.Shared;
    using PaymentDetails;
    using RefundDetails;

    public class AccountManagementViewModel
    {
        public AccountManagementViewModel(AccountOverviewData data)
        {
            TotalCharge = data.TotalCharge;
            TotalPaid = data.TotalPaid;
            Transactions = data.Transactions;
        }

        public decimal TotalCharge { get; set; }

        public decimal TotalPaid { get; set; }

        public IList<TransactionRecordData> Transactions { get; set; }

        public decimal AmountRemaining
        {
            get { return TotalCharge - TotalPaid; }
        }

        public PaymentDetailsViewModel PaymentViewModel { get; set; }

        public bool ShowPaymentDetails { get; set; }

        public RefundDetailsViewModel RefundViewModel { get; set; }

        public bool ShowRefundDetails { get; set; }
    }
}
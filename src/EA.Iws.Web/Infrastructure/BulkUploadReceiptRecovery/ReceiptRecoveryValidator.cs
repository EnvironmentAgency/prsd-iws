﻿namespace EA.Iws.Web.Infrastructure.BulkUploadReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Documents;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    public class ReceiptRecoveryValidator : IReceiptRecoveryValidator
    {
        private readonly IMediator mediator;
        private readonly IEnumerable<IReceiptRecoveryFileRule> fileRules;

        public DataTable DataTable { get; set; }

        public ReceiptRecoveryValidator(IMediator mediator, IEnumerable<IReceiptRecoveryFileRule> fileRules)
        {
            this.mediator = mediator;
            this.fileRules = fileRules;
        }

        public async Task<ReceiptRecoveryRulesSummary> GetValidationSummary(HttpPostedFileBase file, Guid notificationId)
        {
            var resultFileRules = await GetFileRules(file, FileUploadType.ReceiptRecovery);

            var extension = Path.GetExtension(file.FileName);
            var isCsv = extension == ".csv" ? true : false;

            var bulkMovementRulesSummary = new ReceiptRecoveryRulesSummary(resultFileRules);
            if (bulkMovementRulesSummary.IsFileRulesSuccess)
            {
                bulkMovementRulesSummary =
                    await
                        mediator.SendAsync(new PerformReceiptRecoveryContentValidation(bulkMovementRulesSummary,
                            notificationId, DataTable, file.FileName, isCsv));
            }
            return bulkMovementRulesSummary;
        }

        public async Task<ReceiptRecoveryRulesSummary> GetShipmentMovementValidationSummary(HttpPostedFileBase file, Guid notificationId)
        {
            var resultFileRules = await GetFileRules(file, FileUploadType.ShipmentMovementDocuments);

            var extension = Path.GetExtension(file.FileName);
            var isCsv = extension == ".csv" ? true : false;

            var bulkMovementRulesSummary = new ReceiptRecoveryRulesSummary(resultFileRules);
            
            return bulkMovementRulesSummary;
        }

        private async Task<List<RuleResult<ReceiptRecoveryFileRules>>> GetFileRules(HttpPostedFileBase file, FileUploadType type)
        {
            var rules = new List<RuleResult<ReceiptRecoveryFileRules>>();

            foreach (var rule in fileRules.Where(p => p.UploadType.Contains(type)))
            {
                var result = await rule.GetResult(file);
                // Grab the DataTable from the File Parse rule.
                if (result.Rule == ReceiptRecoveryFileRules.FileParse)
                {
                    DataTable = rule.DataTable;
                }

                rules.Add(result);
            }

            if (DataTable != null && DataTable.Rows.Count == 0)
            {
                rules.Add(new RuleResult<ReceiptRecoveryFileRules>(ReceiptRecoveryFileRules.EmptyData, MessageLevel.Error));
            }
            
            return rules;
        }
    }
}
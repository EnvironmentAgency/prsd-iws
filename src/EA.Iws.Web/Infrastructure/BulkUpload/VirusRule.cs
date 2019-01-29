﻿namespace EA.Iws.Web.Infrastructure.BulkUpload
{
    using System.Data;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Movement.Bulk;
    using Core.Rules;
    using VirusScanning;

    public class VirusRule : IBulkMovementPrenotificationFileRule
    {
        private readonly IVirusScanner virusScanner;

        public VirusRule(IVirusScanner virusScanner)
        {
            this.virusScanner = virusScanner;
        }

        public DataTable DataTable { get; set; }

        public async Task<RuleResult<BulkMovementFileRules>> GetResult(HttpPostedFileBase file)
        {
            var result = MessageLevel.Success;
            byte[] fileBytes;

            using (var memoryStream = new MemoryStream())
            {
                await file.InputStream.CopyToAsync(memoryStream);

                fileBytes = memoryStream.ToArray();
            }

            if (await virusScanner.ScanFileAsync(fileBytes) == ScanResult.Virus)
            {
                result = MessageLevel.Error;
            }

            return new RuleResult<BulkMovementFileRules>(BulkMovementFileRules.Virus, result);
        }
    }
}
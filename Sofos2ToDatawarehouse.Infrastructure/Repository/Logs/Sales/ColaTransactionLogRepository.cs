using Newtonsoft.Json;
using Sofos2ToDatawarehouse.Domain.Entity.General;
using Sofos2ToDatawarehouse.Domain.Entity.Sales;
using Sofos2ToDatawarehouse.Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Infrastructure.Repository.Logs.Sales
{
    public class ColaTransactionLogRepository
    {
        public async Task ExportToJSONFile(List<ColaTransaction> colaTransactionHeader, string transType, string branchCode, string dropSitePathExtractedBase, string dropSitePathLog)
        {
            int firstIdLedger = colaTransactionHeader.Min(o => o.TransNum);
            int lastIdLedger = colaTransactionHeader.Max(o => o.TransNum);
            int transactionCount = colaTransactionHeader.Count();

            string filePath = dropSitePathExtractedBase;
            string filePathLog = dropSitePathLog;
            string baseFileName = string.Format("{0}_{1}", DateTime.Now.ToString("yyyyMMdd_HHmmss"), branchCode);
            string fileName = string.Format("{0}_Start_{1}_Last_{2}_Total{3}.json", baseFileName, firstIdLedger, lastIdLedger, transactionCount);
            string fileDestinationPath = Path.Combine(filePath, fileName);

            using (StreamWriter file = File.CreateText(fileDestinationPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, colaTransactionHeader);
            }

            await LogActivity(firstIdLedger, lastIdLedger, transactionCount, fileName, transType, filePathLog, branchCode);
            AppSettingHelper.SetSetting("lastTransnumLogColaTransaction", lastIdLedger.ToString());
        }

        private async Task LogActivity(int firstIdLedger, int lastIdLedger, int transactionCount, string fileName, string transType, string filePathLog, string branchCode)
        {
            LoggerModel log = new LoggerModel();
            log.Transactiontype = transType;
            log.FirstIdLedger = firstIdLedger;
            log.LastIdLedger = lastIdLedger;
            log.TransactionCount = transactionCount;
            log.FileName = fileName;
            log.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            log.FilePath = filePathLog;
            log.BranchCode = branchCode;
            await CreateLog(log);
        }

        public async Task CreateLog(LoggerModel log)
        {
            string baseFileName = string.Format("{0}_{1}", DateTime.Now.ToString("yyyyMMdd_HHmmss"), log.BranchCode);
            string fileName = string.Format("{0}_Start_{1}_Last_{2}_Total{3}.txt", baseFileName, log.FirstIdLedger, log.LastIdLedger, log.TransactionCount);
            string fileDestinationPath = Path.Combine(log.FilePath, fileName);
            using (StreamWriter file = File.CreateText(fileDestinationPath))
            {

                file.WriteLine($"TransactionType: {log.Transactiontype}");
                file.WriteLine($"FirstIdLedger: {log.FirstIdLedger}");
                file.WriteLine($"LastIdLedger: {log.LastIdLedger}");
                file.WriteLine($"TransactionCount: {log.TransactionCount}");
                file.WriteLine($"FileName: {log.FileName}");
                file.WriteLine($"DateCreated: {log.CreatedDate}");

            }
        }
    }
}

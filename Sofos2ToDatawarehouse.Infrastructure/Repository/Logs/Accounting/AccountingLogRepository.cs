using Newtonsoft.Json;
using Sofos2ToDatawarehouse.Domain.Entity.Accounting;
using Sofos2ToDatawarehouse.Domain.Entity.General;
using Sofos2ToDatawarehouse.Domain.Entity.Inventory;
using Sofos2ToDatawarehouse.Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Infrastructure.Repository.Logs.Accounting
{
    public class AccountingLogRepository
    {
        public void ExportToJSONFile(List<AccountDetails> accountingHeader, string transType, string branchCode, string dropSitePathExtractedBase, string dropSitePathLog)
        {
            int firstTransnum = accountingHeader.Min(o => o.Transnum);
            int lastTransnum = accountingHeader.Max(o => o.Transnum);
            int transactionCount = accountingHeader.Count();

            string filePath = dropSitePathExtractedBase;
            string filePathLog = dropSitePathLog;
            string baseFileName = string.Format("{0}_{1}", DateTime.Now.ToString("yyyyMMdd_HHmmss"), branchCode);
            //string fileName = string.Format("{0}_Start_{1}_Last_{2}_Total{3}.json", baseFileName);
            string fileName = string.Format("{0}_Start_{1}_Last_{2}_Total{3}.json", baseFileName, firstTransnum, lastTransnum, transactionCount);
            string fileDestinationPath = Path.Combine(filePath, fileName);

            using (StreamWriter file = File.CreateText(fileDestinationPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, accountingHeader);
            }

            LogActivity(fileName, filePathLog, branchCode);
            AppSettingHelper.SetSetting("lastTransnumLogChargeAmount", lastTransnum.ToString());
        }

        private void LogActivity(string fileName, string filePathLog, string branchCode)
        {
            LoggerModel log = new LoggerModel();
            log.FileName = fileName;
            log.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            log.FilePath = filePathLog;
            log.BranchCode = branchCode;
            CreateLog(log);
        }

        public void CreateLog(LoggerModel log)
        {
            string baseFileName = string.Format("{0}_{1}", DateTime.Now.ToString("yyyyMMdd_HHmmss"), log.BranchCode);
            //string fileName = string.Format("{0}_Start_{1}_Last_{2}_Total{3}.txt", baseFileName);
            //string fileDestinationPath = Path.Combine(log.FilePath, fileName);
            string fileName = string.Format("{0}_Start_{1}_Last_{2}_Total{3}.txt", baseFileName, log.FirstIdLedger, log.LastIdLedger, log.TransactionCount);
            string fileDestinationPath = Path.Combine(log.FilePath, fileName);
            using (StreamWriter file = File.CreateText(fileDestinationPath))
            {
                file.WriteLine($"FileName: {log.FileName}");
                file.WriteLine($"DateCreated: {log.CreatedDate}");

            }
        }
    }
}

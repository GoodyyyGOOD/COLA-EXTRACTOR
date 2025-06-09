using Newtonsoft.Json;
using Sofos2ToDatawarehouse.Domain.Entity.General;
using Sofos2ToDatawarehouse.Domain.Entity.Inventory;
using Sofos2ToDatawarehouse.Domain.Entity.Sales;
using Sofos2ToDatawarehouse.Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Infrastructure.Repository.Logs.Inventory
{
    public class InventoryLogRepository
    {
        public void ExportToJSONFile(List<Items> inventoryHeader, string transType, string branchCode, string dropSitePathExtractedBase, string dropSitePathLog)
        {

            string filePath = dropSitePathExtractedBase;
            string filePathLog = dropSitePathLog;
            string baseFileName = string.Format("{0}_{1}", DateTime.Now.ToString("yyyyMMdd_HHmmss"), branchCode);
            string fileName = string.Format("{0}_Item.json", baseFileName);
            string fileDestinationPath = Path.Combine(filePath, fileName);

            using (StreamWriter file = File.CreateText(fileDestinationPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, inventoryHeader);
            }

            LogActivity(fileName, filePathLog, branchCode);
        }

        private void LogActivity( string fileName, string filePathLog, string branchCode)
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
            string fileName = string.Format("{0}_Item.txt", baseFileName);
            string fileDestinationPath = Path.Combine(log.FilePath, fileName);
            using (StreamWriter file = File.CreateText(fileDestinationPath))
            {
                file.WriteLine($"FileName: {log.FileName}");
                file.WriteLine($"DateCreated: {log.CreatedDate}");

            }
        }
    }
}

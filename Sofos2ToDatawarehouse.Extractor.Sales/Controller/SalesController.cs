using Sofos2ToDatawarehouse.Domain.Entity.General;
using Sofos2ToDatawarehouse.Infrastructure.Helper;
using Sofos2ToDatawarehouse.Infrastructure.Repository.General;
using Sofos2ToDatawarehouse.Infrastructure.Repository.Logs.Sales;
using Sofos2ToDatawarehouse.Infrastructure.Repository.Sales;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Extractor.Sales.Controller
{
    public class ColaController
    {
        #region Private Declaration

        private string _module = "SALES";

        private string dropSitePathExtractedSalesBase = string.Empty;
        private string dropSitePathTransferredSalesBase = string.Empty;
        private string dropSitePathLogsSalesBase = string.Empty;

        private SalesRepository _salesRepository;
        private ColaTransactionLogRepository _colaTransactionLogRepository;
        private DropSiteModelRepository _dropSiteModelRepository;

        #endregion Private Declaration

        public ColaController()
        {
            InitilizeDropSiteAndSalesRepositories();
            InitializeFolders();
        }

        #region Public Methods

        public void ProcessExtraction()
        {
            try
            {
                var lastIdLedgerLog = AppSettingHelper.GetSetting("lastIdLedgerLogSales");
                int lastIdlegerFromLog = Int32.Parse(lastIdLedgerLog);

                var salesHeader = _salesRepository.GetSalesData(_dropSiteModelRepository.DropSiteModel.QueryMaxFetchLimit, lastIdlegerFromLog);

                if (salesHeader != null)
                {
                    _colaTransactionLogRepository.ExportToJSONFile(salesHeader, _module, _salesRepository._company.BranchCode, dropSitePathExtractedSalesBase, dropSitePathLogsSalesBase);
                    System.Console.WriteLine("All files have been extracted successfully.");
                }
                else
                {
                    System.Console.WriteLine("No sales data available for extraction. Please check the last IdLedger in the configuration file.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }

        }

        #endregion Public Methods
        #region Private Methods

        private string SetDBSource()
        {
            Global _global = new Global(
                            Properties.Settings.Default.HOST,
                            Properties.Settings.Default.DB_NAME,
                            Properties.Settings.Default.DB_USERNAME,
                            Properties.Settings.Default.DB_PASSWORD);
            return _global.GetSourceDatabase();
        }

        private void InitializeFolders()
        {
            string dropSitePathExtractedBase = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathExtracted);
            dropSitePathExtractedSalesBase = Path.Combine(dropSitePathExtractedBase, _dropSiteModelRepository.DropSiteModel.DropSitePathSales);
            if (!Directory.Exists(dropSitePathExtractedSalesBase))
                Directory.CreateDirectory(dropSitePathExtractedSalesBase);

            string dropSitePathTransferedBase = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathTransferred);
            dropSitePathTransferredSalesBase = Path.Combine(dropSitePathTransferedBase, _dropSiteModelRepository.DropSiteModel.DropSitePathSales);
            if (!Directory.Exists(dropSitePathTransferredSalesBase))
                Directory.CreateDirectory(dropSitePathTransferredSalesBase);

            string dropSitePathLogsBase = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathLog);
            dropSitePathLogsSalesBase = Path.Combine(dropSitePathLogsBase, _dropSiteModelRepository.DropSiteModel.DropSitePathSales);
            if (!Directory.Exists(dropSitePathLogsSalesBase))
                Directory.CreateDirectory(dropSitePathLogsSalesBase);
        }



        private void InitilizeDropSiteAndSalesRepositories()
        {
            _dropSiteModelRepository = new DropSiteModelRepository()
            {
                DropSiteModel = new DropSiteModel()
                {
                    DropSitePath = Properties.Settings.Default.DROPSITE_FOLDER,
                    QueryMaxFetchLimit = Convert.ToInt32(Properties.Settings.Default.MAX_FETCH_LIMIT)
                }
            };
            _salesRepository = new SalesRepository(SetDBSource());
            _colaTransactionLogRepository = new ColaTransactionLogRepository();
        }

        #endregion Private Methods
    }
}

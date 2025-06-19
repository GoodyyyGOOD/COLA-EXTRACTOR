using Sofos2ToDatawarehouse.Domain.Entity.General;
using Sofos2ToDatawarehouse.Infrastructure.Helper;
using Sofos2ToDatawarehouse.Infrastructure.Repository.CancelTransaction;
using Sofos2ToDatawarehouse.Infrastructure.Repository.General;
using Sofos2ToDatawarehouse.Infrastructure.Repository.Logs.CancelTransaction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Extractor.CancelTransaction.Controller
{
    public class CancelTransactionController
    {
        #region Private Declaration
        private string _module = "CANCELTRANSACTION";

        private string dropSitePathExtractedCancelTransactionBase = string.Empty;
        private string dropSitePathTransferredCancelTransactionBase = string.Empty;
        private string dropSitePathLogsCancelTransactionBase = string.Empty;

        private CancelTransactionRepository _cancelTransactionRepository;
        private CancelTransactionLogRepository _cancelTransactionLogRepository;
        private DropSiteModelRepository _dropSiteModelRepository;

        #endregion Private Declaration

        public CancelTransactionController()
        {
            InitilizeDropSiteAndCancelTransactionRepositories();
            InitializeFolders();
        }

        #region Public Methods

        public void ProcessExtraction()
        {
            try
            {
                var lastIdLedgerLog = AppSettingHelper.GetSetting("lastIdLedgerLogSales");
                int lastIdlegerFromLog = Int32.Parse(lastIdLedgerLog);

                var CancelTransactionTransaction = _cancelTransactionRepository.GetCancelTransactionData(_dropSiteModelRepository.DropSiteModel.QueryMaxFetchLimit, lastIdlegerFromLog);

                if (CancelTransactionTransaction != null)
                {
                    //_cancelTransactionLogRepository.ExportToJSONFile(CancelTransactionTransaction, _module, _cancelTransactionRepository._company.BranchCode, dropSitePathExtractedCancelTransactionBase, dropSitePathLogsCancelTransactionBase);
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
            dropSitePathExtractedCancelTransactionBase = Path.Combine(dropSitePathExtractedBase, _dropSiteModelRepository.DropSiteModel.DropSitePathCancelTransaction);
            if (!Directory.Exists(dropSitePathExtractedCancelTransactionBase))
                Directory.CreateDirectory(dropSitePathExtractedCancelTransactionBase);

            string dropSitePathTransferedBase = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathTransferred);
            dropSitePathTransferredCancelTransactionBase = Path.Combine(dropSitePathTransferedBase, _dropSiteModelRepository.DropSiteModel.DropSitePathCancelTransaction);
            if (!Directory.Exists(dropSitePathTransferredCancelTransactionBase))
                Directory.CreateDirectory(dropSitePathTransferredCancelTransactionBase);

            string dropSitePathLogsBase = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathLog);
            dropSitePathLogsCancelTransactionBase = Path.Combine(dropSitePathLogsBase, _dropSiteModelRepository.DropSiteModel.DropSitePathCancelTransaction);
            if (!Directory.Exists(dropSitePathLogsCancelTransactionBase))
                Directory.CreateDirectory(dropSitePathLogsCancelTransactionBase);
        }



        private void InitilizeDropSiteAndCancelTransactionRepositories()
        {
            _dropSiteModelRepository = new DropSiteModelRepository()
            {
                DropSiteModel = new DropSiteModel()
                {
                    DropSitePath = Properties.Settings.Default.DROPSITE_FOLDER,
                    QueryMaxFetchLimit = Convert.ToInt32(Properties.Settings.Default.MAX_FETCH_LIMIT)
                }
            };
            _cancelTransactionRepository = new CancelTransactionRepository(SetDBSource());
            _cancelTransactionLogRepository = new CancelTransactionLogRepository();
        }

        #endregion Private Methods
    }
}

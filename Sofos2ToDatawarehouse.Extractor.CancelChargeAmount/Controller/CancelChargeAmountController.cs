using Sofos2ToDatawarehouse.Domain.Entity.General;
using Sofos2ToDatawarehouse.Infrastructure.Repository.CancelChargeAmount;
using Sofos2ToDatawarehouse.Infrastructure.Repository.General;
using Sofos2ToDatawarehouse.Infrastructure.Repository.Logs.CancelChargeAmount;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sofos2ToDatawarehouse.Infrastructure.Helper;

namespace Sofos2ToDatawarehouse.Extractor.CancelChargeAmount.Controller
{
    public class CancelChargeAmountController
    {
        #region Private Declaration

        private string _module = "CANCELCHARGEAMOUNT";

        private string dropSitePathExtractedCancelChargeAmountBase = string.Empty;
        private string dropSitePathTransferredCancelChargeAmountBase = string.Empty;
        private string dropSitePathLogsCancelChargeAmountBase = string.Empty;

        private CancelChargeAmountRepository _cancelChargeAmountRepository;
        private CancelChargeAmountLogRepository _cancelChargeAmountLogRepository;
        private DropSiteModelRepository _dropSiteModelRepository;

        #endregion Private Declaration

        public CancelChargeAmountController()
        {
            InitilizeDropSiteAndCancelChargeAmountRepositories();
            InitializeFolders();
        }

        #region Public Methods

        public void ProcessExtraction()
        {
            try
            {
                var lastIdLedgerLog = AppSettingHelper.GetSetting("lastTransnumLogChargeAmount");
                int lastIdlegerFromLog = Int32.Parse(lastIdLedgerLog);
                var cancelChargeAmountHeader = _cancelChargeAmountRepository.GetCancelChargeAmountData(_dropSiteModelRepository.DropSiteModel.QueryMaxFetchLimit, lastIdlegerFromLog);

                if (cancelChargeAmountHeader != null)
                {
                    _cancelChargeAmountLogRepository.ExportToJSONFile(cancelChargeAmountHeader, _module, _cancelChargeAmountRepository._company.BranchCode, dropSitePathExtractedCancelChargeAmountBase, dropSitePathLogsCancelChargeAmountBase);
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
            dropSitePathExtractedCancelChargeAmountBase = Path.Combine(dropSitePathExtractedBase, _dropSiteModelRepository.DropSiteModel.DropSitePathCancelChargeAmount);
            if (!Directory.Exists(dropSitePathExtractedCancelChargeAmountBase))
                Directory.CreateDirectory(dropSitePathExtractedCancelChargeAmountBase);

            string dropSitePathTransferedBase = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathTransferred);
            dropSitePathTransferredCancelChargeAmountBase = Path.Combine(dropSitePathTransferedBase, _dropSiteModelRepository.DropSiteModel.DropSitePathCancelChargeAmount);
            if (!Directory.Exists(dropSitePathTransferredCancelChargeAmountBase))
                Directory.CreateDirectory(dropSitePathTransferredCancelChargeAmountBase);

            string dropSitePathLogsBase = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathLog);
            dropSitePathLogsCancelChargeAmountBase = Path.Combine(dropSitePathLogsBase, _dropSiteModelRepository.DropSiteModel.DropSitePathCancelChargeAmount);
            if (!Directory.Exists(dropSitePathLogsCancelChargeAmountBase))
                Directory.CreateDirectory(dropSitePathLogsCancelChargeAmountBase);
        }



        private void InitilizeDropSiteAndCancelChargeAmountRepositories()
        {
            _dropSiteModelRepository = new DropSiteModelRepository()
            {
                DropSiteModel = new DropSiteModel()
                {
                    DropSitePath = Properties.Settings.Default.DROPSITE_FOLDER,
                    QueryMaxFetchLimit = Convert.ToInt32(Properties.Settings.Default.MAX_FETCH_LIMIT)
                }
            };
            _cancelChargeAmountRepository = new CancelChargeAmountRepository(SetDBSource());
            _cancelChargeAmountLogRepository = new CancelChargeAmountLogRepository();
        }

        #endregion Private Methods
    }
}

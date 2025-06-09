using Sofos2ToDatawarehouse.Domain.Entity.General;
using Sofos2ToDatawarehouse.Infrastructure.Repository.Accounting;
using Sofos2ToDatawarehouse.Infrastructure.Repository.General;
using Sofos2ToDatawarehouse.Infrastructure.Repository.Logs.Accounting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sofos2ToDatawarehouse.Extrator.Accounting.Properties;
using Sofos2ToDatawarehouse.Infrastructure.Helper;

namespace Sofos2ToDatawarehouse.Extrator.Accounting.Controller
{
    public class ChargeAmountController
    {
        #region Private Declaration

        private string _module = "ACCOUNTING";

        private string dropSitePathExtractedAccountingBase = string.Empty;
        private string dropSitePathTransferredAccountingBase = string.Empty;
        private string dropSitePathLogsAccountingBase = string.Empty;

        private AccountingRepository _accountingRepository;
        private AccountingLogRepository _accountingLogRepository;
        private DropSiteModelRepository _dropSiteModelRepository;

        #endregion Private Declaration

        public ChargeAmountController()
        {
            InitilizeDropSiteAndAccountingRepositories();
            InitializeFolders();
        }

        #region Public Methods

        public void ProcessExtraction()
        {
            try
            {
                var lastIdLedgerLog = AppSettingHelper.GetSetting("lastTransnumLogChargeAmount");
                int lastIdlegerFromLog = Int32.Parse(lastIdLedgerLog);
                var accountingHeader = _accountingRepository.GetAccountingData(_dropSiteModelRepository.DropSiteModel.QueryMaxFetchLimit, lastIdlegerFromLog);

                if (accountingHeader != null)
                {
                    _accountingLogRepository.ExportToJSONFile(accountingHeader, _module, _accountingRepository._company.BranchCode, dropSitePathExtractedAccountingBase, dropSitePathLogsAccountingBase);
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
            dropSitePathExtractedAccountingBase = Path.Combine(dropSitePathExtractedBase, _dropSiteModelRepository.DropSiteModel.DropSitePathAccounting);
            if (!Directory.Exists(dropSitePathExtractedAccountingBase))
                Directory.CreateDirectory(dropSitePathExtractedAccountingBase);

            string dropSitePathTransferedBase = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathTransferred);
            dropSitePathTransferredAccountingBase = Path.Combine(dropSitePathTransferedBase, _dropSiteModelRepository.DropSiteModel.DropSitePathAccounting);
            if (!Directory.Exists(dropSitePathTransferredAccountingBase))
                Directory.CreateDirectory(dropSitePathTransferredAccountingBase);

            string dropSitePathLogsBase = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathLog);
            dropSitePathLogsAccountingBase = Path.Combine(dropSitePathLogsBase, _dropSiteModelRepository.DropSiteModel.DropSitePathAccounting);
            if (!Directory.Exists(dropSitePathLogsAccountingBase))
                Directory.CreateDirectory(dropSitePathLogsAccountingBase);
        }



        private void InitilizeDropSiteAndAccountingRepositories()
        {
            _dropSiteModelRepository = new DropSiteModelRepository()
            {
                DropSiteModel = new DropSiteModel()
                {
                    DropSitePath = Properties.Settings.Default.DROPSITE_FOLDER,
                    QueryMaxFetchLimit = Convert.ToInt32(Properties.Settings.Default.MAX_FETCH_LIMIT)
                }
            };
            _accountingRepository = new AccountingRepository(SetDBSource());
            _accountingLogRepository = new AccountingLogRepository();
        }

        #endregion Private Methods
    }
}

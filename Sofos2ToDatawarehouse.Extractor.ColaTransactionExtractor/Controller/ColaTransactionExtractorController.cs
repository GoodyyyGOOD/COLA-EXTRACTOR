using Sofos2ToDatawarehouse.Domain.Entity.General;
using Sofos2ToDatawarehouse.Infrastructure.Repository.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Sofos2ToDatawarehouse.Infrastructure.Helper;
using Sofos2ToDatawarehouse.Infrastructure.Repository.CancelChargeAmount;
using Sofos2ToDatawarehouse.Infrastructure.Repository.Logs.CancelChargeAmount;
using Sofos2ToDatawarehouse.Infrastructure.Repository.ColaStub;
using Sofos2ToDatawarehouse.Infrastructure.Repository.Logs.ColaStub;
using Sofos2ToDatawarehouse.Infrastructure.Repository.CancelTransaction;
using Sofos2ToDatawarehouse.Infrastructure.Repository.Logs.CancelTransaction;
using Sofos2ToDatawarehouse.Infrastructure.Repository.Logs.Sales;
using Sofos2ToDatawarehouse.Infrastructure.Repository.Sales;
using Sofos2ToDatawarehouse.Infrastructure.Repository.Accounting;
using Sofos2ToDatawarehouse.Infrastructure.Repository.Logs.Accounting;
using System.IO;
using System.Reflection;

namespace Sofos2ToDatawarehouse.Extractor.ColaTransactionExtractor.Controller
{
    public class ColaTransactionExtractorController
    {
        #region Private Declaration

        private string _chargeAmountmodule = "CA";
        private string _cancelChargeAmountmodule = "CCA";
        private string _colaStubmodule = "CS";
        private string _cancelTransactionmodule = "CCT";
        private string _colaTransactionmodule = "CT";

        private string dropSitePathExtractedAccountingBase = string.Empty;
        private string dropSitePathTransferredAccountingBase = string.Empty;
        private string dropSitePathLogsAccountingBase = string.Empty;

        private string dropSitePathExtractedCancelChargeAmountBase = string.Empty;
        private string dropSitePathTransferredCancelChargeAmountBase = string.Empty;
        private string dropSitePathLogsCancelChargeAmountBase = string.Empty;

        private string dropSitePathExtractedColaStubBase = string.Empty;
        private string dropSitePathTransferredColaStubBase = string.Empty;
        private string dropSitePathLogsColaStubBase = string.Empty;

        private string dropSitePathExtractedCancelTransactionBase = string.Empty;
        private string dropSitePathTransferredCancelTransactionBase = string.Empty;
        private string dropSitePathLogsCancelTransactionBase = string.Empty;

        private string dropSitePathExtractedSalesBase = string.Empty;
        private string dropSitePathTransferredSalesBase = string.Empty;
        private string dropSitePathLogsSalesBase = string.Empty;


        private DropSiteModelRepository _dropSiteModelRepository;

        private AccountingRepository _accountingRepository;
        private AccountingLogRepository _accountingLogRepository;

        private CancelChargeAmountRepository _cancelChargeAmountRepository;
        private CancelChargeAmountLogRepository _cancelChargeAmountLogRepository;

        private ColaStubRepository _colaStubRepository;
        private ColaStubLogRepository _colaStubLogRepository;

        private CancelTransactionRepository _cancelTransactionRepository;
        private CancelTransactionLogRepository _cancelTransactionLogRepository;

        private SalesRepository _salesRepository;
        private ColaTransactionLogRepository _colaTransactionLogRepository;

        #endregion Private Declaration

        public ColaTransactionExtractorController()
        {
            InitilizeDropSiteAndColaTransactionExtractorRepositories();
            InitializeFolders();
        }
        #region Public Methods
        #region Cancel Charge Amount Extractor
        public async Task CancelChargeAmountExtraction()
        {
            try
            {
                var lastIdLedgerLog = AppSettingHelper.GetSetting("lastTransnumLogCancelChargeAmount");
                int lastIdlegerFromLog = Int32.Parse(lastIdLedgerLog);
                var cancelChargeAmountHeader = await _cancelChargeAmountRepository.GetCancelChargeAmountData(_dropSiteModelRepository.DropSiteModel.QueryMaxFetchLimit, lastIdlegerFromLog);

                if (cancelChargeAmountHeader != null)
                {
                    await _cancelChargeAmountLogRepository.ExportToJSONFile(cancelChargeAmountHeader, _cancelChargeAmountmodule, _cancelChargeAmountRepository._company.BranchCode, dropSitePathExtractedCancelChargeAmountBase, dropSitePathLogsCancelChargeAmountBase);
                    System.Console.WriteLine("All cancel charge amount files have been extracted successfully.");
                }
                else
                {
                    System.Console.WriteLine("No cancel charge amount data available for extraction. Please check the last IdLedger in the configuration file.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }

        }
        #endregion Cancel Charge Amount Extractor
        #region Charge Amount Extractor

        public async Task ChargeAmountExtraction()
        {
            try
            {
                var lastIdLedgerLog = AppSettingHelper.GetSetting("lastTransnumLogChargeAmount");
                int lastIdlegerFromLog = Int32.Parse(lastIdLedgerLog);
                var accountingHeader = await _accountingRepository.GetAccountingData(_dropSiteModelRepository.DropSiteModel.QueryMaxFetchLimit, lastIdlegerFromLog);

                if (accountingHeader != null)
                {
                    await _accountingLogRepository.ExportToJSONFile(accountingHeader, _chargeAmountmodule, _accountingRepository._company.BranchCode, dropSitePathExtractedAccountingBase, dropSitePathLogsAccountingBase);
                    System.Console.WriteLine("All charge amount files have been extracted successfully.");
                }
                else
                {
                    System.Console.WriteLine("No charge amount data available for extraction. Please check the last IdLedger in the configuration file.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }

        }
        #endregion Charge Amount Extractor
        #region ColaStub
        public async Task ColaStubExtraction()
        {
            try
            {
                var lastIdLedgerLog = AppSettingHelper.GetSetting("lastTransnumLogColastub");
                int lastIdlegerFromLog = Int32.Parse(lastIdLedgerLog);

                var colaStubTransaction = await _colaStubRepository.GetColaStubData(_dropSiteModelRepository.DropSiteModel.QueryMaxFetchLimit, lastIdlegerFromLog);

                if (colaStubTransaction != null)
                {
                    await _colaStubLogRepository.ExportToJSONFile(colaStubTransaction, _colaStubmodule, _colaStubRepository._company.BranchCode, dropSitePathExtractedColaStubBase, dropSitePathLogsColaStubBase);
                    System.Console.WriteLine("All colastub files have been extracted successfully.");
                }
                else
                {
                    System.Console.WriteLine("No colastub data available for extraction. Please check the last IdLedger in the configuration file.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }

        }
        #endregion ColaStub
        #region Cancel Transaction
        public async Task CancelTransactionExtraction()
        {
            try
            {
                var lastIdLedgerLog = AppSettingHelper.GetSetting("lastTransnumLogCancelTransaction");
                int lastIdlegerFromLog = Int32.Parse(lastIdLedgerLog);

                var CancelTransactionTransaction = await _cancelTransactionRepository.GetCancelTransactionData(_dropSiteModelRepository.DropSiteModel.QueryMaxFetchLimit, lastIdlegerFromLog);

                if (CancelTransactionTransaction != null)
                {
                    await _cancelTransactionLogRepository.ExportToJSONFile(CancelTransactionTransaction, _cancelTransactionmodule, _cancelTransactionRepository._company.BranchCode, dropSitePathExtractedCancelTransactionBase, dropSitePathLogsCancelTransactionBase);
                    System.Console.WriteLine("All cancel cola transaction files have been extracted successfully.");
                }
                else
                {
                    System.Console.WriteLine("No cancel cola transaction data available for extraction. Please check the last IdLedger in the configuration file.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }

        }
        #endregion Cancel Transaction
        #region Cola Transaction
        public async Task ColaTransactionExtraction()
        {
            try
            {
                var lastIdLedgerLog = AppSettingHelper.GetSetting("lastTransnumLogColaTransaction");
                int lastIdlegerFromLog = Int32.Parse(lastIdLedgerLog);

                var salesHeader = await _salesRepository.GetColaData(_dropSiteModelRepository.DropSiteModel.QueryMaxFetchLimit, lastIdlegerFromLog);

                if (salesHeader != null)
                {
                    await _colaTransactionLogRepository.ExportToJSONFile(salesHeader, _colaTransactionmodule, _salesRepository._company.BranchCode, dropSitePathExtractedSalesBase, dropSitePathLogsSalesBase);
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
        #endregion Cola Transaction
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
            string dropSitePathExtractedBaseCA = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathExtracted);
            dropSitePathExtractedAccountingBase = Path.Combine(dropSitePathExtractedBaseCA, _dropSiteModelRepository.DropSiteModel.DropSitePathAccounting);
            if (!Directory.Exists(dropSitePathExtractedAccountingBase))
                Directory.CreateDirectory(dropSitePathExtractedAccountingBase);

            string dropSitePathTransferedBaseCA = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathTransferred);
            dropSitePathTransferredAccountingBase = Path.Combine(dropSitePathTransferedBaseCA, _dropSiteModelRepository.DropSiteModel.DropSitePathAccounting);
            if (!Directory.Exists(dropSitePathTransferredAccountingBase))
                Directory.CreateDirectory(dropSitePathTransferredAccountingBase);

            string dropSitePathLogsBaseCA = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathLog);
            dropSitePathLogsAccountingBase = Path.Combine(dropSitePathLogsBaseCA, _dropSiteModelRepository.DropSiteModel.DropSitePathAccounting);
            if (!Directory.Exists(dropSitePathLogsAccountingBase))
                Directory.CreateDirectory(dropSitePathLogsAccountingBase);

            string dropSitePathExtractedBaseCCA = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathExtracted);
            dropSitePathExtractedCancelChargeAmountBase = Path.Combine(dropSitePathExtractedBaseCCA, _dropSiteModelRepository.DropSiteModel.DropSitePathCancelChargeAmount);
            if (!Directory.Exists(dropSitePathExtractedCancelChargeAmountBase))
                Directory.CreateDirectory(dropSitePathExtractedCancelChargeAmountBase);

            string dropSitePathTransferedBaseCCA = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathTransferred);
            dropSitePathTransferredCancelChargeAmountBase = Path.Combine(dropSitePathTransferedBaseCCA, _dropSiteModelRepository.DropSiteModel.DropSitePathCancelChargeAmount);
            if (!Directory.Exists(dropSitePathTransferredCancelChargeAmountBase))
                Directory.CreateDirectory(dropSitePathTransferredCancelChargeAmountBase);

            string dropSitePathLogsBaseCCA = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathLog);
            dropSitePathLogsCancelChargeAmountBase = Path.Combine(dropSitePathLogsBaseCCA, _dropSiteModelRepository.DropSiteModel.DropSitePathCancelChargeAmount);
            if (!Directory.Exists(dropSitePathLogsCancelChargeAmountBase))
                Directory.CreateDirectory(dropSitePathLogsCancelChargeAmountBase);

            string dropSitePathExtractedBaseCS = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathExtracted);
            dropSitePathExtractedColaStubBase = Path.Combine(dropSitePathExtractedBaseCS, _dropSiteModelRepository.DropSiteModel.DropSitePathColaStub);
            if (!Directory.Exists(dropSitePathExtractedColaStubBase))
                Directory.CreateDirectory(dropSitePathExtractedColaStubBase);

            string dropSitePathTransferedBaseCS = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathTransferred);
            dropSitePathTransferredColaStubBase = Path.Combine(dropSitePathTransferedBaseCS, _dropSiteModelRepository.DropSiteModel.DropSitePathColaStub);
            if (!Directory.Exists(dropSitePathTransferredColaStubBase))
                Directory.CreateDirectory(dropSitePathTransferredColaStubBase);

            string dropSitePathLogsBaseCS = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathLog);
            dropSitePathLogsColaStubBase = Path.Combine(dropSitePathLogsBaseCS, _dropSiteModelRepository.DropSiteModel.DropSitePathColaStub);
            if (!Directory.Exists(dropSitePathLogsColaStubBase))
                Directory.CreateDirectory(dropSitePathLogsColaStubBase);

            string dropSitePathExtractedBaseCCT = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathExtracted);
            dropSitePathExtractedCancelTransactionBase = Path.Combine(dropSitePathExtractedBaseCCT, _dropSiteModelRepository.DropSiteModel.DropSitePathCancelTransaction);
            if (!Directory.Exists(dropSitePathExtractedCancelTransactionBase))
                Directory.CreateDirectory(dropSitePathExtractedCancelTransactionBase);

            string dropSitePathTransferedBaseCCT = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathTransferred);
            dropSitePathTransferredCancelTransactionBase = Path.Combine(dropSitePathTransferedBaseCCT, _dropSiteModelRepository.DropSiteModel.DropSitePathCancelTransaction);
            if (!Directory.Exists(dropSitePathTransferredCancelTransactionBase))
                Directory.CreateDirectory(dropSitePathTransferredCancelTransactionBase);

            string dropSitePathLogsBaseCCT = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathLog);
            dropSitePathLogsCancelTransactionBase = Path.Combine(dropSitePathLogsBaseCCT, _dropSiteModelRepository.DropSiteModel.DropSitePathCancelTransaction);
            if (!Directory.Exists(dropSitePathLogsCancelTransactionBase))
                Directory.CreateDirectory(dropSitePathLogsCancelTransactionBase);

            string dropSitePathExtractedBaseCT = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathExtracted);
            dropSitePathExtractedSalesBase = Path.Combine(dropSitePathExtractedBaseCT, _dropSiteModelRepository.DropSiteModel.DropSitePathSales);
            if (!Directory.Exists(dropSitePathExtractedSalesBase))
                Directory.CreateDirectory(dropSitePathExtractedSalesBase);

            string dropSitePathTransferedBaseCT = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathTransferred);
            dropSitePathTransferredSalesBase = Path.Combine(dropSitePathTransferedBaseCT, _dropSiteModelRepository.DropSiteModel.DropSitePathSales);
            if (!Directory.Exists(dropSitePathTransferredSalesBase))
                Directory.CreateDirectory(dropSitePathTransferredSalesBase);

            string dropSitePathLogsBaseCT = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathLog);
            dropSitePathLogsSalesBase = Path.Combine(dropSitePathLogsBaseCT, _dropSiteModelRepository.DropSiteModel.DropSitePathSales);
            if (!Directory.Exists(dropSitePathLogsSalesBase))
                Directory.CreateDirectory(dropSitePathLogsSalesBase);


        }



        private void InitilizeDropSiteAndColaTransactionExtractorRepositories()
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
            _cancelChargeAmountRepository = new CancelChargeAmountRepository(SetDBSource());
            _colaStubRepository = new ColaStubRepository(SetDBSource());
            _cancelTransactionRepository = new CancelTransactionRepository(SetDBSource());
            _salesRepository = new SalesRepository(SetDBSource());

            _accountingLogRepository = new AccountingLogRepository();
            _cancelChargeAmountLogRepository = new CancelChargeAmountLogRepository();
            _colaStubLogRepository = new ColaStubLogRepository();
            _cancelTransactionLogRepository = new CancelTransactionLogRepository();
            _colaTransactionLogRepository = new ColaTransactionLogRepository();
        }

        #endregion Private Methods
    }
}

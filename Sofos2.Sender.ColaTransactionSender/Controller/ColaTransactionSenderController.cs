using Sofos2ToDatawarehouse.Domain.DTOs;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Accounting.ChargeAmount.BulkUpSert;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.CancelTransaction.BulkUpSert;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaStub.BulkUpSert;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaTransaction.BulkUpSert;
using Sofos2ToDatawarehouse.Domain.Entity.General;
using Sofos2ToDatawarehouse.Infrastructure.Repository.General;
using Sofos2ToDatawarehouse.Infrastructure.Repository.Sales;
using Sofos2ToDatawarehouse.Infrastructure.Services.Accounting;
using Sofos2ToDatawarehouse.Infrastructure.Services.CancelChargeAmount;
using Sofos2ToDatawarehouse.Infrastructure.Services.CancelTransaction;
using Sofos2ToDatawarehouse.Infrastructure.Services.ColaStub;
using Sofos2ToDatawarehouse.Infrastructure.Services.LogsEntity;
using Sofos2ToDatawarehouse.Infrastructure.Services.Sales;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2.Sender.ColaTransactionSender.Controller
{
    public class ColaTransactionSenderController
    {
        private string _colaTransactionModule = "CO";
        private string _colaStubModule = "CS";
        private string _cancelTransactionModule = "CT";
        private string _CancelChargeAmountModule = "CC";
        private string _accountingModule = "CA";

        private string dropSitePathExtractedColaTransactionPOBase = string.Empty;
        private string dropSitePathTransferredColaTransactionBase = string.Empty;
        private string dropSitePathLogsColaTransactionBase = string.Empty;
        private string dropSitePathExtractedColaStubPOBase = string.Empty;
        private string dropSitePathTransferredColaStubBase = string.Empty;
        private string dropSitePathLogsColaStubBase = string.Empty;
        private string dropSitePathExtractedCancelTransactionPOBase = string.Empty;
        private string dropSitePathTransferredCancelTransactionBase = string.Empty;
        private string dropSitePathLogsCancelTransactionBase = string.Empty;
        private string dropSitePathExtractedCancelChargeAmountPOBase = string.Empty;
        private string dropSitePathTransferredCancelChargeAmountBase = string.Empty;
        private string dropSitePathLogsCancelChargeAmountBase = string.Empty;
        private string dropSitePathExtractedChargeAmountPOBase = string.Empty;
        private string dropSitePathTransferredChargeAmountBase = string.Empty;
        private string dropSitePathLogsChargeAmountBase = string.Empty;


        private ColaTransactionService _colaTransactionService;
        private ColaStubService _colaStubService;
        private CancelTransactionService _cancelTransactionService;
        private CancelChargeAmountService _cancelChargeAmountService;
        private AccountingService _accountingService;

        private SalesRepository _salesRepository;

        private SIDCAPILogsService _sidcAPILogsService;
        private ProcessLogsService _processLogsService;

        private DropSiteModelRepository _dropSiteModelRepository;
        private string _branchCode = Properties.Settings.Default.BRANCH_CODE;

        #region Cola Transaction
        public async Task SendingColaTransactionToAPIAsync()
        {
            try
            {

                DirectoryInfo directoryInfo = new DirectoryInfo(dropSitePathExtractedColaTransactionPOBase);
                FileInfo[] extractedFiles = directoryInfo.GetFiles("*.json");


                foreach (FileInfo extractedFile in extractedFiles)
                {
                    ColaTransactionBulkUpsertRequest colaTransactionBulkUpsertRequest = new ColaTransactionBulkUpsertRequest();

                    string jsonString = File.ReadAllText(extractedFile.FullName);

                    colaTransactionBulkUpsertRequest.CreateColaTransactionCommand = _colaTransactionService.DeserializeObjectToColaTransactionBulkUpSertRequest(jsonString);

                    //string tokenForStockRequestService = await _colaTransactionService.SendAuthenticationAsync();

                    colaTransactionBulkUpsertRequest.CreateColaTransactionCommand = colaTransactionBulkUpsertRequest.CreateColaTransactionCommand;
                    //await _salesRepository.MarkColaAsInserted(colaTransactionBulkUpsertRequest.CreateColaTransactionCommand);
                    var responseSendBulkUpsert = await _colaTransactionService.PostColaTransactionAsync(colaTransactionBulkUpsertRequest);

                    //await _salesRepository.MarkColaAsInserted(colaTransactionBulkUpsertRequest.CreateColaTransactionCommand);

                    try
                    {
                        if (responseSendBulkUpsert.Succeeded)
                        {
                            await _salesRepository.MarkColaAsInserted(colaTransactionBulkUpsertRequest.CreateColaTransactionCommand);
                            Console.WriteLine("Sending COLA transactions. . .");
                            await _colaTransactionService.MoveFileToTransferredAsync(extractedFile, Path.Combine(dropSitePathTransferredColaTransactionBase, extractedFile.Name));
                            //await _processLogsService.ColaTransactionLogsServiceRequestAsync(colaTransactionBulkUpsertRequest, _sidcAPILogsService, extractedFile.Name, _branchCode);

                            //await _salesRepository.MarkColaAsInserted(colaTransactionBulkUpsertRequest);
                            //_salesRepository = new SalesRepository();
                            await _salesRepository.MarkColaAsInserted(colaTransactionBulkUpsertRequest.CreateColaTransactionCommand);
                        }
                    }
                    catch (Exception ex)
                    {

                        if (ex is IOException || ex is UnauthorizedAccessException moveFileException)
                        {
                            Console.WriteLine($"Error in ColaTransactionToAPIAsync MoveFileToTransferredAsync");
                        }
                        else
                        {
                            Console.WriteLine("Error in ColaTransactionToAPIAsync LogsServiceRequestAsync");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: ColaTransactionToAPIAsync {ex.Message}");
            }
        }
        #endregion Cola Transaction
        #region Cola Stub
        public async Task SendingColaStubToAPIAsync()
        {
            try
            {

                DirectoryInfo directoryInfo = new DirectoryInfo(dropSitePathExtractedColaStubPOBase);
                FileInfo[] extractedFiles = directoryInfo.GetFiles("*.json");


                foreach (FileInfo extractedFile in extractedFiles)
                {
                    ColaStubBulkUpsertRequest colaStubBulkUpsertRequest = new ColaStubBulkUpsertRequest();

                    string jsonString = File.ReadAllText(extractedFile.FullName);

                    colaStubBulkUpsertRequest.CreateColaStubCommand = _colaStubService.DeserializeObjectToColaStubBulkUpSertRequest(jsonString);

                    //string tokenForStockRequestService = await _colaStubService.SendAuthenticationAsync();

                    colaStubBulkUpsertRequest.CreateColaStubCommand = colaStubBulkUpsertRequest.CreateColaStubCommand;
                    var responseSendBulkUpsert = await _colaStubService.PostColaStubAsync(colaStubBulkUpsertRequest);


                    try
                    {
                        if (responseSendBulkUpsert.Succeeded)
                        {
                            Console.WriteLine("Sending colastub transactions. . .");
                            await _colaStubService.MoveFileToTransferredAsync(extractedFile, Path.Combine(dropSitePathTransferredColaStubBase, extractedFile.Name));
                            //await _processLogsService.ColaStubLogsServiceRequestAsync(colaStubBulkUpsertRequest, _sidcAPILogsService, extractedFile.Name, _branchCode);
                        }
                    }
                    catch (Exception ex)
                    {

                        if (ex is IOException || ex is UnauthorizedAccessException moveFileException)
                        {
                            Console.WriteLine($"Error in ColaStubToAPIAsync MoveFileToTransferredAsync");
                        }
                        else
                        {
                            Console.WriteLine("Error in ColaStubToAPIAsync LogsServiceRequestAsync");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: ColaStubToAPIAsync {ex.Message}");
            }
        }
        #endregion Cola Stub

        #region Cancel Transaction
        public async Task SendingCancelTransactionToAPIAsync()
        {
            try
            {

                DirectoryInfo directoryInfo = new DirectoryInfo(dropSitePathExtractedCancelTransactionPOBase);
                FileInfo[] extractedFiles = directoryInfo.GetFiles("*.json");


                foreach (FileInfo extractedFile in extractedFiles)
                {
                    CancelTransactionBulkUpsertRequest cancelTransactionBulkUpsertRequest = new CancelTransactionBulkUpsertRequest();

                    string jsonString = File.ReadAllText(extractedFile.FullName);

                    cancelTransactionBulkUpsertRequest.CreateCancelTransactionCommand = _cancelTransactionService.DeserializeObjectToCancelTransactionBulkUpSertRequest(jsonString);

                    //string tokenForStockRequestService = await _cancelTransactionService.SendAuthenticationAsync();

                    cancelTransactionBulkUpsertRequest.CreateCancelTransactionCommand = cancelTransactionBulkUpsertRequest.CreateCancelTransactionCommand;
                    var responseSendBulkUpsert = await _cancelTransactionService.PostCancelTransactionAsync(cancelTransactionBulkUpsertRequest);


                    try
                    {
                        if (responseSendBulkUpsert.Succeeded)
                        {
                            Console.WriteLine("Sending CancelTransaction transactions. . .");
                            await _cancelTransactionService.MoveFileToTransferredAsync(extractedFile, Path.Combine(dropSitePathTransferredCancelTransactionBase, extractedFile.Name));
                            //await _processLogsService.CancelTransactionLogsServiceRequestAsync(cancelTransactionBulkUpsertRequest, _sidcAPILogsService, extractedFile.Name, _branchCode);
                        }
                    }
                    catch (Exception ex)
                    {

                        if (ex is IOException || ex is UnauthorizedAccessException moveFileException)
                        {
                            Console.WriteLine($"Error in CancelTransactionToAPIAsync MoveFileToTransferredAsync");
                        }
                        else
                        {
                            Console.WriteLine("Error in CancelTransactionToAPIAsync LogsServiceRequestAsync");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: CancelTransactionToAPIAsync {ex.Message}");
            }
        }
        #endregion Cancel Transaction

        #region Cancel Charge Amount
        public async Task SendingCancelChargeAmountToAPIAsync()
        {
            try
            {

                DirectoryInfo directoryInfo = new DirectoryInfo(dropSitePathExtractedCancelChargeAmountPOBase);
                FileInfo[] extractedFiles = directoryInfo.GetFiles("*.json");


                foreach (FileInfo extractedFile in extractedFiles)
                {
                    Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Accounting.CancelChargeAmount.BulkUpSert.CancelChargeAmountBulkUpsertRequest cancelChargeAmountBulkUpsertRequest = new Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Accounting.CancelChargeAmount.BulkUpSert.CancelChargeAmountBulkUpsertRequest();

                    string jsonString = File.ReadAllText(extractedFile.FullName);

                    cancelChargeAmountBulkUpsertRequest.CreateCancelChargeAmountCommand = _cancelChargeAmountService.DeserializeObjectToCancelChargeAmountBulkUpSertRequest(jsonString);

                    //string tokenForStockRequestService = await _CancelChargeAmountService.SendAuthenticationAsync();

                    cancelChargeAmountBulkUpsertRequest.CreateCancelChargeAmountCommand = cancelChargeAmountBulkUpsertRequest.CreateCancelChargeAmountCommand;
                    var responseSendBulkUpsert = await _cancelChargeAmountService.PostCancelChargeAmountAsync(cancelChargeAmountBulkUpsertRequest);


                    try
                    {
                        if (responseSendBulkUpsert.Succeeded)
                        {
                            Console.WriteLine("Sending Charge Amount transactions. . .");
                            await _cancelChargeAmountService.MoveFileToTransferredAsync(extractedFile, Path.Combine(dropSitePathTransferredCancelChargeAmountBase, extractedFile.Name));
                            //await _processLogsService.CancelChargeAmountLogsServiceRequestAsync(cancelChargeAmountBulkUpsertRequest, _sidcAPILogsService, extractedFile.Name, _branchCode);
                        }
                    }
                    catch (Exception ex)
                    {

                        if (ex is IOException || ex is UnauthorizedAccessException moveFileException)
                        {
                            Console.WriteLine($"Error in CancelChargeAmountToAPIAsync MoveFileToTransferredAsync");
                        }
                        else
                        {
                            Console.WriteLine("Error in CancelChargeAmountToAPIAsync LogsServiceRequestAsync");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: CancelChargeAmountToAPIAsync {ex.Message}");
            }
        }
        #endregion Cancel Charge Amount

        #region Charge Amount
        public async Task SendingChargeAmountToAPIAsync()
        {
            try
            {

                DirectoryInfo directoryInfo = new DirectoryInfo(dropSitePathExtractedChargeAmountPOBase);
                FileInfo[] extractedFiles = directoryInfo.GetFiles("*.json");


                foreach (FileInfo extractedFile in extractedFiles)
                {
                    ChargeAmountBulkUpsertRequest chargeAmountBulkUpsertRequest = new ChargeAmountBulkUpsertRequest();

                    string jsonString = File.ReadAllText(extractedFile.FullName);

                    chargeAmountBulkUpsertRequest.CreateChargeAmountCommand = _accountingService.DeserializeObjectToChargeAmountBulkUpSertRequest(jsonString);

                    //string tokenForStockRequestService = await _accountingService.SendAuthenticationAsync();

                    chargeAmountBulkUpsertRequest.CreateChargeAmountCommand = chargeAmountBulkUpsertRequest.CreateChargeAmountCommand;
                    var responseSendBulkUpsert = await _accountingService.PostChargeAmountAsync(chargeAmountBulkUpsertRequest);


                    try
                    {
                        if (responseSendBulkUpsert.Succeeded)
                        {
                            Console.WriteLine("Sending Charge Amount transactions. . .");
                            await _accountingService.MoveFileToTransferredAsync(extractedFile, Path.Combine(dropSitePathTransferredChargeAmountBase, extractedFile.Name));
                            //await _processLogsService.ChargeAmountLogsServiceRequestAsync(chargeAmountBulkUpsertRequest, _sidcAPILogsService, extractedFile.Name, _branchCode);
                        }
                    }
                    catch (Exception ex)
                    {

                        if (ex is IOException || ex is UnauthorizedAccessException moveFileException)
                        {
                            Console.WriteLine($"Error in ChargeAmountToAPIAsync MoveFileToTransferredAsync");
                        }
                        else
                        {
                            Console.WriteLine("Error in ChargeAmountToAPIAsync LogsServiceRequestAsync");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: ChargeAmountToAPIAsync {ex.Message}");
            }
        }
        #endregion Charge Amount

        private void InitializeFolders()
        {

            string dropSitePathExtractedBaseCT = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathExtracted);
            dropSitePathExtractedColaTransactionPOBase = Path.Combine(dropSitePathExtractedBaseCT, _dropSiteModelRepository.DropSiteModel.DropSitePathSales);
            if (!Directory.Exists(dropSitePathExtractedColaTransactionPOBase))
                Directory.CreateDirectory(dropSitePathExtractedColaTransactionPOBase);

            string dropSitePathTransferredBaseCT = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathTransferred);
            dropSitePathTransferredColaTransactionBase = Path.Combine(dropSitePathTransferredBaseCT, _dropSiteModelRepository.DropSiteModel.DropSitePathSales);
            if (!Directory.Exists(dropSitePathTransferredColaTransactionBase))
                Directory.CreateDirectory(dropSitePathTransferredColaTransactionBase);

            string dropSitePathLogsBaseCT = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathLog);
            dropSitePathLogsColaTransactionBase = Path.Combine(dropSitePathLogsBaseCT, _dropSiteModelRepository.DropSiteModel.DropSitePathSales);
            if (!Directory.Exists(dropSitePathLogsColaTransactionBase))
                Directory.CreateDirectory(dropSitePathLogsColaTransactionBase);

            string dropSitePathExtractedBaseCS = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathExtracted);
            dropSitePathExtractedColaStubPOBase = Path.Combine(dropSitePathExtractedBaseCS, _dropSiteModelRepository.DropSiteModel.DropSitePathColaStub);
            if (!Directory.Exists(dropSitePathExtractedColaStubPOBase))
                Directory.CreateDirectory(dropSitePathExtractedColaStubPOBase);

            string dropSitePathTransferredBaseCS = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathTransferred);
            dropSitePathTransferredColaStubBase = Path.Combine(dropSitePathTransferredBaseCS, _dropSiteModelRepository.DropSiteModel.DropSitePathColaStub);
            if (!Directory.Exists(dropSitePathTransferredColaStubBase))
                Directory.CreateDirectory(dropSitePathTransferredColaStubBase);

            string dropSitePathLogsBaseCS = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathLog);
            dropSitePathLogsColaStubBase = Path.Combine(dropSitePathLogsBaseCS, _dropSiteModelRepository.DropSiteModel.DropSitePathColaStub);
            if (!Directory.Exists(dropSitePathLogsColaStubBase))
                Directory.CreateDirectory(dropSitePathLogsColaStubBase);

            string dropSitePathExtractedBaseCCT = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathExtracted);
            dropSitePathExtractedCancelTransactionPOBase = Path.Combine(dropSitePathExtractedBaseCCT, _dropSiteModelRepository.DropSiteModel.DropSitePathCancelTransaction);
            if (!Directory.Exists(dropSitePathExtractedCancelTransactionPOBase))
                Directory.CreateDirectory(dropSitePathExtractedCancelTransactionPOBase);

            string dropSitePathTransferredBaseCCT = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathTransferred);
            dropSitePathTransferredCancelTransactionBase = Path.Combine(dropSitePathTransferredBaseCCT, _dropSiteModelRepository.DropSiteModel.DropSitePathCancelTransaction);
            if (!Directory.Exists(dropSitePathTransferredCancelTransactionBase))
                Directory.CreateDirectory(dropSitePathTransferredCancelTransactionBase);

            string dropSitePathLogsBaseCCT = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathLog);
            dropSitePathLogsCancelTransactionBase = Path.Combine(dropSitePathLogsBaseCCT, _dropSiteModelRepository.DropSiteModel.DropSitePathCancelTransaction);
            if (!Directory.Exists(dropSitePathLogsCancelTransactionBase))
                Directory.CreateDirectory(dropSitePathLogsCancelTransactionBase);

            string dropSitePathExtractedBaseCC = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathExtracted);
            dropSitePathExtractedCancelChargeAmountPOBase = Path.Combine(dropSitePathExtractedBaseCC, _dropSiteModelRepository.DropSiteModel.DropSitePathCancelChargeAmount);
            if (!Directory.Exists(dropSitePathExtractedCancelChargeAmountPOBase))
                Directory.CreateDirectory(dropSitePathExtractedCancelChargeAmountPOBase);

            string dropSitePathTransferredBaseCC = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathTransferred);
            dropSitePathTransferredCancelChargeAmountBase = Path.Combine(dropSitePathTransferredBaseCC, _dropSiteModelRepository.DropSiteModel.DropSitePathCancelChargeAmount);
            if (!Directory.Exists(dropSitePathTransferredCancelChargeAmountBase))
                Directory.CreateDirectory(dropSitePathTransferredCancelChargeAmountBase);

            string dropSitePathLogsBaseCC = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathLog);
            dropSitePathLogsCancelChargeAmountBase = Path.Combine(dropSitePathLogsBaseCC, _dropSiteModelRepository.DropSiteModel.DropSitePathCancelChargeAmount);
            if (!Directory.Exists(dropSitePathLogsCancelChargeAmountBase))
                Directory.CreateDirectory(dropSitePathLogsCancelChargeAmountBase);

            string dropSitePathExtractedBaseCA = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathExtracted);
            dropSitePathExtractedChargeAmountPOBase = Path.Combine(dropSitePathExtractedBaseCA, _dropSiteModelRepository.DropSiteModel.DropSitePathAccounting);
            if (!Directory.Exists(dropSitePathExtractedChargeAmountPOBase))
                Directory.CreateDirectory(dropSitePathExtractedChargeAmountPOBase);

            string dropSitePathTransferredBaseCA = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathTransferred);
            dropSitePathTransferredChargeAmountBase = Path.Combine(dropSitePathTransferredBaseCA, _dropSiteModelRepository.DropSiteModel.DropSitePathAccounting);
            if (!Directory.Exists(dropSitePathTransferredChargeAmountBase))
                Directory.CreateDirectory(dropSitePathTransferredChargeAmountBase);

            string dropSitePathLogsBaseCA = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathLog);
            dropSitePathLogsChargeAmountBase = Path.Combine(dropSitePathLogsBaseCA, _dropSiteModelRepository.DropSiteModel.DropSitePathAccounting);
            if (!Directory.Exists(dropSitePathLogsChargeAmountBase))
                Directory.CreateDirectory(dropSitePathLogsChargeAmountBase);
        }

        private SIDCLogsServiceApiSettings SetSIDCLogServiceApiSettings()
        {
            var _sidcLogsServiceApiSettings = new SIDCLogsServiceApiSettings();
            _sidcLogsServiceApiSettings.BaseUrl = Properties.Settings.Default.BASE_URL;


            return _sidcLogsServiceApiSettings;
        }

        private void InitilizeDropSiteAndRepositories()
        {
            _dropSiteModelRepository = new DropSiteModelRepository()
            {
                DropSiteModel = new DropSiteModel()
                {
                    DropSitePath = Properties.Settings.Default.DROPSITE_FOLDER,

                }
            };
            _salesRepository = new SalesRepository(SetDBSource());
            _colaTransactionService = new ColaTransactionService(SetSIDCAPIServiceSettings());
            _colaStubService = new ColaStubService(SetSIDCAPIServiceSettings());
            _cancelTransactionService = new CancelTransactionService(SetSIDCAPIServiceSettings());
            _cancelChargeAmountService = new CancelChargeAmountService(SetSIDCAPIServiceSettings());
            _accountingService = new AccountingService(SetSIDCAPIServiceSettings());
        }

        private SIDCAPIServiceSettings SetSIDCAPIServiceSettings()
        {
            var _sidcServiceApiSettings = new SIDCAPIServiceSettings();
            _sidcServiceApiSettings.BaseUrl = Properties.Settings.Default.BASE_URL;
            _sidcServiceApiSettings.APIToken = Properties.Settings.Default.API_TOKEN;
            _sidcServiceApiSettings.SalesBaseUrl = Properties.Settings.Default.API_COLA_URL;
            _sidcServiceApiSettings.ColaStubBaseUrl = Properties.Settings.Default.API_COLASTUB_URL;
            _sidcServiceApiSettings.CancelTransactionBaseUrl = Properties.Settings.Default.API_CANCELTRANSACTION_URL;
            _sidcServiceApiSettings.CancelChargeAmountBaseUrl = Properties.Settings.Default.API_CANCELCHARGEAMOUNT_URL;
            _sidcServiceApiSettings.AccountingBaseUrl = Properties.Settings.Default.API_ACCOUNTING_URL;

            return _sidcServiceApiSettings;
        }

        private string SetDBSource()
        {
            Global _global = new Global(
                            Properties.Settings.Default.HOST,
                            Properties.Settings.Default.DB_NAME,
                            Properties.Settings.Default.DB_USERNAME,
                            Properties.Settings.Default.DB_PASSWORD);
            return _global.GetSourceDatabase();
        }

        private string PasswordDecode(string passwordEncoded)
        {

            var passwordDecoded = Convert.FromBase64String(passwordEncoded);
            return Encoding.UTF8.GetString(passwordDecoded);
        }
    }
}

using Sofos2ToDatawarehouse.Domain.DTOs;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaTransaction.BulkUpSert;
using Sofos2ToDatawarehouse.Domain.Entity.General;
using Sofos2ToDatawarehouse.Infrastructure.Repository.General;
using Sofos2ToDatawarehouse.Infrastructure.Services.LogsEntity;
using Sofos2ToDatawarehouse.Infrastructure.Services.Sales;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOFOS2.Sender.Sales.Controller
{
    public class SalesController
    {
        private string _accountingModule = "CO";

        private string dropSitePathExtractedColaTransactionPOBase = string.Empty;
        private string dropSitePathTransferredColaTransactionBase = string.Empty;
        private string dropSitePathLogsColaTransactionBase = string.Empty;

        private ColaTransactionService _colaTransactionService;

        private SIDCAPILogsService _sidcAPILogsService;
        private ProcessLogsService _processLogsService;

        private DropSiteModelRepository _dropSiteModelRepository;
        private string _branchCode = Properties.Settings.Default.BRANCH_CODE;

        public SalesController()
        {
            InitilizeDropSiteAndRepositories();
            InitializeFolders();
        }


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
                    var responseSendBulkUpsert = await _colaTransactionService.PostColaTransactionAsync(colaTransactionBulkUpsertRequest);


                    try
                    {
                        if (responseSendBulkUpsert.Succeeded)
                        {
                            Console.WriteLine("Sending COLA transactions. . .");
                            await _colaTransactionService.MoveFileToTransferredAsync(extractedFile, Path.Combine(dropSitePathTransferredColaTransactionBase, extractedFile.Name));
                            await _processLogsService.ColaTransactionLogsServiceRequestAsync(colaTransactionBulkUpsertRequest, _sidcAPILogsService, extractedFile.Name, _branchCode);
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

        #region Private Methods

        private void InitializeFolders()
        {

            string dropSitePathExtractedBaseCA = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathExtracted);
            dropSitePathExtractedColaTransactionPOBase = Path.Combine(dropSitePathExtractedBaseCA, _dropSiteModelRepository.DropSiteModel.DropSitePathSales);
            if (!Directory.Exists(dropSitePathExtractedColaTransactionPOBase))
                Directory.CreateDirectory(dropSitePathExtractedColaTransactionPOBase);

            string dropSitePathTransferredBaseCA = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathTransferred);
            dropSitePathTransferredColaTransactionBase = Path.Combine(dropSitePathTransferredBaseCA, _dropSiteModelRepository.DropSiteModel.DropSitePathSales);
            if (!Directory.Exists(dropSitePathTransferredColaTransactionBase))
                Directory.CreateDirectory(dropSitePathTransferredColaTransactionBase);

            string dropSitePathLogsBaseCA = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathLog);
            dropSitePathLogsColaTransactionBase = Path.Combine(dropSitePathLogsBaseCA, _dropSiteModelRepository.DropSiteModel.DropSitePathSales);
            if (!Directory.Exists(dropSitePathLogsColaTransactionBase))
                Directory.CreateDirectory(dropSitePathLogsColaTransactionBase);
        }

        private SIDCLogsServiceApiSettings SetSIDCLogServiceApiSettings()
        {
            var _sidcLogsServiceApiSettings = new SIDCLogsServiceApiSettings();
            _sidcLogsServiceApiSettings.BaseUrl = Properties.Settings.Default.BASE_URL;
            _sidcLogsServiceApiSettings.BaseUrl = Properties.Settings.Default.BASE_URL;
            //_sidcLogsServiceApiSettings.IdentityUrl = Properties.Settings.Default.API_IDENTITY_BASE_URL;
            //_sidcLogsServiceApiSettings.BaseUrl = Properties.Settings.Default.API_LOGS_BASE_URL;
            //_sidcLogsServiceApiSettings.AuthTokenUrl = Properties.Settings.Default.API_AUTH_TOKEN_URL;
            //_sidcLogsServiceApiSettings.AuthUser = Properties.Settings.Default.API_AUTH_USERNAME;
            //_sidcLogsServiceApiSettings.AuthPassword = PasswordDecode(Properties.Settings.Default.API_AUTH_PASSWORD);

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
            _colaTransactionService = new ColaTransactionService(SetSIDCAPIServiceSettings());
            //_sidcAPILogsService = new SIDCAPILogsService(SetSIDCLogServiceApiSettings());
            //_processLogsService = new ProcessLogsService();
        }

        private SIDCAPIServiceSettings SetSIDCAPIServiceSettings()
        {
            var _sidcServiceApiSettings = new SIDCAPIServiceSettings();
            _sidcServiceApiSettings.BaseUrl = Properties.Settings.Default.BASE_URL;
            _sidcServiceApiSettings.SalesBaseUrl = Properties.Settings.Default.API_COLA_URL;
            _sidcServiceApiSettings.APIToken = Properties.Settings.Default.API_TOKEN;
            //_sidcServiceApiSettings.IdentityUrl = Properties.Settings.Default.API_IDENTITY_BASE_URL;
            //_sidcServiceApiSettings.AccountingBaseUrl = Properties.Settings.Default.API_ACCOUNTING_BASE_URL;
            //_sidcServiceApiSettings.AuthTokenUrl = Properties.Settings.Default.API_AUTH_TOKEN_URL;
            //_sidcServiceApiSettings.AuthUser = Properties.Settings.Default.API_AUTH_USERNAME;
            //_sidcServiceApiSettings.AuthPassword = PasswordDecode(Properties.Settings.Default.API_AUTH_PASSWORD);

            return _sidcServiceApiSettings;
        }

        private string PasswordDecode(string passwordEncoded)
        {

            var passwordDecoded = Convert.FromBase64String(passwordEncoded);
            return Encoding.UTF8.GetString(passwordDecoded);
        }
    }
}

#endregion
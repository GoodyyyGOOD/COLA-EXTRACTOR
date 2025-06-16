using Sofos2ToDatawarehouse.Domain.DTOs;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.CancelTransaction.BulkUpSert;
using Sofos2ToDatawarehouse.Domain.Entity.General;
using Sofos2ToDatawarehouse.Infrastructure.Repository.General;
using Sofos2ToDatawarehouse.Infrastructure.Services.CancelTransaction;
using Sofos2ToDatawarehouse.Infrastructure.Services.LogsEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2.Sender.CancelTransaction.Controller
{
    public class CancelTransactionController
    {
        private string _cancelTransactionModule = "CT";

        private string dropSitePathExtractedCancelTransactionPOBase = string.Empty;
        private string dropSitePathTransferredCancelTransactionBase = string.Empty;
        private string dropSitePathLogsCancelTransactionBase = string.Empty;

        private CancelTransactionService _cancelTransactionService;

        private SIDCAPILogsService _sidcAPILogsService;
        private ProcessLogsService _processLogsService;

        private DropSiteModelRepository _dropSiteModelRepository;
        private string _branchCode = Properties.Settings.Default.BRANCH_CODE;

        public CancelTransactionController()
        {
            InitilizeDropSiteAndRepositories();
            InitializeFolders();
        }


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
                            await _processLogsService.CancelTransactionLogsServiceRequestAsync(cancelTransactionBulkUpsertRequest, _sidcAPILogsService, extractedFile.Name, _branchCode);
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

        #region Private Methods

        private void InitializeFolders()
        {

            string dropSitePathExtractedBaseCT = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathExtracted);
            dropSitePathExtractedCancelTransactionPOBase = Path.Combine(dropSitePathExtractedBaseCT, _dropSiteModelRepository.DropSiteModel.DropSitePathCancelTransaction);
            if (!Directory.Exists(dropSitePathExtractedCancelTransactionPOBase))
                Directory.CreateDirectory(dropSitePathExtractedCancelTransactionPOBase);

            string dropSitePathTransferredBaseCT = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathTransferred);
            dropSitePathTransferredCancelTransactionBase = Path.Combine(dropSitePathTransferredBaseCT, _dropSiteModelRepository.DropSiteModel.DropSitePathCancelTransaction);
            if (!Directory.Exists(dropSitePathTransferredCancelTransactionBase))
                Directory.CreateDirectory(dropSitePathTransferredCancelTransactionBase);

            string dropSitePathLogsBaseCT= Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathLog);
            dropSitePathLogsCancelTransactionBase = Path.Combine(dropSitePathLogsBaseCT, _dropSiteModelRepository.DropSiteModel.DropSitePathCancelTransaction);
            if (!Directory.Exists(dropSitePathLogsCancelTransactionBase))
                Directory.CreateDirectory(dropSitePathLogsCancelTransactionBase);
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
            _cancelTransactionService = new CancelTransactionService(SetSIDCAPIServiceSettings());
            //_sidcAPILogsService = new SIDCAPILogsService(SetSIDCLogServiceApiSettings());
            //_processLogsService = new ProcessLogsService();
        }

        private SIDCAPIServiceSettings SetSIDCAPIServiceSettings()
        {
            var _sidcServiceApiSettings = new SIDCAPIServiceSettings();
            _sidcServiceApiSettings.BaseUrl = Properties.Settings.Default.BASE_URL;
            _sidcServiceApiSettings.CancelTransactionBaseUrl = Properties.Settings.Default.API_CANCELTRANSACTION_URL;
            _sidcServiceApiSettings.APIToken = Properties.Settings.Default.API_TOKEN;
            //_sidcServiceApiSettings.IdentityUrl = Properties.Settings.Default.API_IDENTITY_BASE_URL;
            //_sidcServiceApiSettings.CancelTransactionBaseUrl = Properties.Settings.Default.API_cancelTransaction_BASE_URL;
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
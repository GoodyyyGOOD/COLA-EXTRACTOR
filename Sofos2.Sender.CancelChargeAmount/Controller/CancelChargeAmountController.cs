using Sofos2ToDatawarehouse.Domain.DTOs;
using Sofos2ToDatawarehouse.Domain.Entity.General;
using Sofos2ToDatawarehouse.Infrastructure.Repository.General;
using Sofos2ToDatawarehouse.Infrastructure.Services.CancelChargeAmount;
using Sofos2ToDatawarehouse.Infrastructure.Services.LogsEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2.Sender.CancelChargeAmount.Controller
{
    internal class CancelChargeAmountController
    {
        private string _CancelChargeAmountModule = "CC";

        private string dropSitePathExtractedCancelChargeAmountPOBase = string.Empty;
        private string dropSitePathTransferredCancelChargeAmountBase = string.Empty;
        private string dropSitePathLogsCancelChargeAmountBase = string.Empty;

        private CancelChargeAmountService _cancelChargeAmountService;

        private SIDCAPILogsService _sidcAPILogsService;
        private ProcessLogsService _processLogsService;

        private DropSiteModelRepository _dropSiteModelRepository;
        private string _branchCode = Properties.Settings.Default.BRANCH_CODE;

        public CancelChargeAmountController()
        {
            InitilizeDropSiteAndRepositories();
            InitializeFolders();
        }


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

        #region Private Methods

        private void InitializeFolders()
        {

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
        }

        private SIDCLogsServiceApiSettings SetSIDCLogServiceApiSettings()
        {
            var _sidcLogsServiceApiSettings = new SIDCLogsServiceApiSettings();
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
            _cancelChargeAmountService = new CancelChargeAmountService(SetSIDCAPIServiceSettings());
            //_sidcAPILogsService = new SIDCAPILogsService(SetSIDCLogServiceApiSettings());
            //_processLogsService = new ProcessLogsService();
        }

        private SIDCAPIServiceSettings SetSIDCAPIServiceSettings()
        {
            var _sidcServiceApiSettings = new SIDCAPIServiceSettings();
            _sidcServiceApiSettings.BaseUrl = Properties.Settings.Default.BASE_URL;
            _sidcServiceApiSettings.CancelChargeAmountBaseUrl = Properties.Settings.Default.API_CANCELCHARGEAMOUNT_URL;
            _sidcServiceApiSettings.APIToken = Properties.Settings.Default.API_TOKEN;
            //_sidcServiceApiSettings.IdentityUrl = Properties.Settings.Default.API_IDENTITY_BASE_URL;
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
using Sofos2ToDatawarehouse.Domain.DTOs;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaStub.BulkUpSert;
using Sofos2ToDatawarehouse.Domain.Entity.General;
using Sofos2ToDatawarehouse.Infrastructure.Repository.General;
using Sofos2ToDatawarehouse.Infrastructure.Services.ColaStub;
using Sofos2ToDatawarehouse.Infrastructure.Services.LogsEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2.Sender.ColaStub.Controller
{
    public class ColaStubController
    {
        private string _colaStubModule = "CS";

        private string dropSitePathExtractedColaStubPOBase = string.Empty;
        private string dropSitePathTransferredColaStubBase = string.Empty;
        private string dropSitePathLogsColaStubBase = string.Empty;

        private ColaStubService _colaStubService;

        private SIDCAPILogsService _sidcAPILogsService;
        private ProcessLogsService _processLogsService;

        private DropSiteModelRepository _dropSiteModelRepository;
        private string _branchCode = Properties.Settings.Default.BRANCH_CODE;

        public ColaStubController()
        {
            InitilizeDropSiteAndRepositories();
            InitializeFolders();
        }


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

        #region Private Methods

        private void InitializeFolders()
        {

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
        }

        private SIDCLogsServiceApiSettings SetSIDCLogServiceApiSettings()
        {
            var _sidcLogsServiceApiSettings = new SIDCLogsServiceApiSettings();
            _sidcLogsServiceApiSettings.BaseUrl = Properties.Settings.Default.BASE_URL;
            //_sidcLogsServiceApiSettings.BaseUrl = Properties.Settings.Default.BASE_URL;
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
            _colaStubService = new ColaStubService(SetSIDCAPIServiceSettings());
            //_sidcAPILogsService = new SIDCAPILogsService(SetSIDCLogServiceApiSettings());
            //_processLogsService = new ProcessLogsService();
        }

        private SIDCAPIServiceSettings SetSIDCAPIServiceSettings()
        {
            var _sidcApiServiceSettings = new SIDCAPIServiceSettings();
            _sidcApiServiceSettings.BaseUrl = Properties.Settings.Default.BASE_URL;
            _sidcApiServiceSettings.ColaStubBaseUrl = Properties.Settings.Default.API_COLASTUB_URL;
            _sidcApiServiceSettings.APIToken = Properties.Settings.Default.API_TOKEN;
            //_sidcServiceApiSettings.IdentityUrl = Properties.Settings.Default.API_IDENTITY_BASE_URL;
            //_sidcServiceApiSettings.ColaStubBaseUrl = Properties.Settings.Default.API_colaStub_BASE_URL;
            //_sidcServiceApiSettings.AuthTokenUrl = Properties.Settings.Default.API_AUTH_TOKEN_URL;
            //_sidcServiceApiSettings.AuthUser = Properties.Settings.Default.API_AUTH_USERNAME;
            //_sidcServiceApiSettings.AuthPassword = PasswordDecode(Properties.Settings.Default.API_AUTH_PASSWORD);

            return _sidcApiServiceSettings;
        }

        private string PasswordDecode(string passwordEncoded)
        {

            var passwordDecoded = Convert.FromBase64String(passwordEncoded);
            return Encoding.UTF8.GetString(passwordDecoded);
        }
    }
}
#endregion
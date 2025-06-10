using Sofos2ToDatawarehouse.Domain.DTOs;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Accounting.ChargeAmount.BulkUpSert;
using Sofos2ToDatawarehouse.Domain.Entity.General;
using Sofos2ToDatawarehouse.Infrastructure.Repository.General;
using Sofos2ToDatawarehouse.Infrastructure.Services.Accounting;
using Sofos2ToDatawarehouse.Infrastructure.Services.LogsEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2.Sender.Accounting.Controller
{
    public class AccountingController
        {
            private string _accountingModule = "CA";
            
            private string dropSitePathExtractedChargeAmountPOBase = string.Empty;
            private string dropSitePathTransferredChargeAmountBase = string.Empty;
            private string dropSitePathLogsChargeAmountBase = string.Empty;

            private AccountingService _accountingService;

            private SIDCAPILogsService _sidcAPILogsService;
            private ProcessLogsService _processLogsService;

            private DropSiteModelRepository _dropSiteModelRepository;
            private string _branchCode = Properties.Settings.Default.BRANCH_CODE;

            public AccountingController()
            {
                InitilizeDropSiteAndRepositories();
                InitializeFolders();
            }


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
                                await _processLogsService.ChargeAmountLogsServiceRequestAsync(chargeAmountBulkUpsertRequest, _sidcAPILogsService, extractedFile.Name, _branchCode);
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

            #region Private Methods

            private void InitializeFolders()
            {

                string dropSitePathExtractedBaseCA= Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathExtracted);
            dropSitePathExtractedChargeAmountPOBase = Path.Combine(dropSitePathExtractedBaseCA, _dropSiteModelRepository.DropSiteModel.DropSitePathAccounting);
                if (!Directory.Exists(dropSitePathExtractedChargeAmountPOBase))
                    Directory.CreateDirectory(dropSitePathExtractedChargeAmountPOBase);

                string dropSitePathTransferredBaseCA= Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathTransferred);
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
                _accountingService = new AccountingService(SetSIDCAPIServiceSettings());
                //_sidcAPILogsService = new SIDCAPILogsService(SetSIDCLogServiceApiSettings());
                //_processLogsService = new ProcessLogsService();
            }

            private SIDCAPIServiceSettings SetSIDCAPIServiceSettings()
            {
                var _sidcServiceApiSettings = new SIDCAPIServiceSettings();
            _sidcServiceApiSettings.BaseUrl = Properties.Settings.Default.BASE_URL;
            _sidcServiceApiSettings.AccountingBaseUrl = Properties.Settings.Default.API_ACCOUNTING_URL;
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

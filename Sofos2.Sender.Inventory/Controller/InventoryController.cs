using Sofos2ToDatawarehouse.Domain.DTOs;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Inventory.Items.BulkUpSert;
using Sofos2ToDatawarehouse.Domain.Entity.General;
using Sofos2ToDatawarehouse.Infrastructure.Repository.General;
using Sofos2ToDatawarehouse.Infrastructure.Services.Inventory;
using Sofos2ToDatawarehouse.Infrastructure.Services.LogsEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2.Sender.Inventory.Controller
{
    public class InventoryController
    {
        //ID = Items Data
        private string _itemsModule = "ID";

        private string dropSitePathExtractedItemBase = string.Empty;
        private string dropSitePathTransferredItemsBase = string.Empty;
        private string dropSitePathLogsItemsBase = string.Empty;

        private InventoryService _inventoryService;

        private SIDCAPILogsService _sidcAPILogsService;
        private ProcessLogsService _processLogsService;

        private DropSiteModelRepository _dropSiteModelRepository;
        private string _branchCode = Properties.Settings.Default.BRANCH_CODE;

        public InventoryController()
        {
            InitilizeDropSiteAndRepositories();
            InitializeFolders();
        }


        public async Task SendingItemsToAPIAsync()
        {
            try
            {

                DirectoryInfo directoryInfo = new DirectoryInfo(dropSitePathExtractedItemBase);
                FileInfo[] extractedFiles = directoryInfo.GetFiles("*.json");


                foreach (FileInfo extractedFile in extractedFiles)
                {
                    ItemsBulkUpsertRequest itemsBulkUpsertRequest = new ItemsBulkUpsertRequest();

                    string jsonString = File.ReadAllText(extractedFile.FullName);

                    itemsBulkUpsertRequest.CreateItemsCommand = _inventoryService.DeserializeObjectToItemsBulkUpSertRequest(jsonString);

                    //string tokenForStockRequestService = await _itemsService.SendAuthenticationAsync();

                    itemsBulkUpsertRequest.CreateItemsCommand = itemsBulkUpsertRequest.CreateItemsCommand;
                    var responseSendBulkUpsert = await _inventoryService.PostItemsAsync(itemsBulkUpsertRequest);


                    try
                    {
                        if (responseSendBulkUpsert.Succeeded)
                        {
                            Console.WriteLine("Sending Charge Amount transactions. . .");
                            await _inventoryService.MoveFileToTransferredAsync(extractedFile, Path.Combine(dropSitePathTransferredItemsBase, extractedFile.Name));
                            await _processLogsService.ItemsLogsServiceRequestAsync(itemsBulkUpsertRequest, _sidcAPILogsService, extractedFile.Name, _branchCode);
                        }
                    }
                    catch (Exception ex)
                    {

                        if (ex is IOException || ex is UnauthorizedAccessException moveFileException)
                        {
                            Console.WriteLine($"Error in ItemsToAPIAsync MoveFileToTransferredAsync");
                        }
                        else
                        {
                            Console.WriteLine("Error in ItemsToAPIAsync LogsServiceRequestAsync");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: ItemsToAPIAsync {ex.Message}");
            }
        }

        #region Private Methods

        private void InitializeFolders()
        {

            string dropSitePathExtractedBaseID = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathExtracted);
            dropSitePathExtractedItemBase = Path.Combine(dropSitePathExtractedBaseID, _dropSiteModelRepository.DropSiteModel.DropSitePathInventory);
            if (!Directory.Exists(dropSitePathExtractedItemBase))
                Directory.CreateDirectory(dropSitePathExtractedItemBase);

            string dropSitePathTransferredBaseID = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathTransferred);
            dropSitePathTransferredItemsBase = Path.Combine(dropSitePathTransferredBaseID, _dropSiteModelRepository.DropSiteModel.DropSitePathInventory);
            if (!Directory.Exists(dropSitePathTransferredItemsBase))
                Directory.CreateDirectory(dropSitePathTransferredItemsBase);

            string dropSitePathLogsBaseID = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathLog);
            dropSitePathLogsItemsBase = Path.Combine(dropSitePathLogsBaseID, _dropSiteModelRepository.DropSiteModel.DropSitePathInventory);
            if (!Directory.Exists(dropSitePathLogsItemsBase))
                Directory.CreateDirectory(dropSitePathLogsItemsBase);
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
            _inventoryService = new InventoryService(SetSIDCAPIServiceSettings());
            //_sidcAPILogsService = new SIDCAPILogsService(SetSIDCLogServiceApiSettings());
            //_processLogsService = new ProcessLogsService();
        }

        private SIDCAPIServiceSettings SetSIDCAPIServiceSettings()
        {
            var _sidcServiceApiSettings = new SIDCAPIServiceSettings();
            _sidcServiceApiSettings.BaseUrl = Properties.Settings.Default.BASE_URL;
            _sidcServiceApiSettings.InventoryBaseUrl = Properties.Settings.Default.API_ACCOUNTING_URL;
            //_sidcServiceApiSettings.IdentityUrl = Properties.Settings.Default.API_IDENTITY_BASE_URL;
            //_sidcServiceApiSettings.ItemsBaseUrl = Properties.Settings.Default.API_ACCOUNTING_BASE_URL;
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
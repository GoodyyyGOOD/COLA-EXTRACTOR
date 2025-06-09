using Sofos2ToDatawarehouse.Domain.Entity.General;
using Sofos2ToDatawarehouse.Infrastructure.Helper;
using Sofos2ToDatawarehouse.Infrastructure.Repository.General;
using Sofos2ToDatawarehouse.Infrastructure.Repository.Inventory;
using Sofos2ToDatawarehouse.Infrastructure.Repository.Logs.Inventory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Extractor.Inventory.Controller
{
    public class ItemController
    {
        #region Private Declaration

        private string _module = "INVENTORY";

        private string dropSitePathExtractedInventoryBase = string.Empty;
        private string dropSitePathTransferredInventoryBase = string.Empty;
        private string dropSitePathLogsInventoryBase = string.Empty;

        private InventoryRepository _inventoryRepository;
        private InventoryLogRepository _inventoryLogRepository;
        private DropSiteModelRepository _dropSiteModelRepository;

        #endregion Private Declaration

        public ItemController()
        {
            InitilizeDropSiteAndInventoryRepositories();
            InitializeFolders();
        }

        #region Public Methods

        public void ProcessExtraction()
        {
            try
            {

                var inventoryHeader = _inventoryRepository.GetInventoryData();

                if (inventoryHeader != null)
                {
                    _inventoryLogRepository.ExportToJSONFile(inventoryHeader, _module, _inventoryRepository._company.BranchCode, dropSitePathExtractedInventoryBase, dropSitePathLogsInventoryBase);
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
            dropSitePathExtractedInventoryBase = Path.Combine(dropSitePathExtractedBase, _dropSiteModelRepository.DropSiteModel.DropSitePathInventory);
            if (!Directory.Exists(dropSitePathExtractedInventoryBase))
                Directory.CreateDirectory(dropSitePathExtractedInventoryBase);

            string dropSitePathTransferedBase = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathTransferred);
            dropSitePathTransferredInventoryBase = Path.Combine(dropSitePathTransferedBase, _dropSiteModelRepository.DropSiteModel.DropSitePathInventory);
            if (!Directory.Exists(dropSitePathTransferredInventoryBase))
                Directory.CreateDirectory(dropSitePathTransferredInventoryBase);

            string dropSitePathLogsBase = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathLog);
            dropSitePathLogsInventoryBase = Path.Combine(dropSitePathLogsBase, _dropSiteModelRepository.DropSiteModel.DropSitePathInventory);
            if (!Directory.Exists(dropSitePathLogsInventoryBase))
                Directory.CreateDirectory(dropSitePathLogsInventoryBase);
        }



        private void InitilizeDropSiteAndInventoryRepositories()
        {
            _dropSiteModelRepository = new DropSiteModelRepository()
            {
                DropSiteModel = new DropSiteModel()
                {
                    DropSitePath = Properties.Settings.Default.DROPSITE_FOLDER,
                    QueryMaxFetchLimit = Convert.ToInt32(Properties.Settings.Default.MAX_FETCH_LIMIT)
                }
            };
            _inventoryRepository = new InventoryRepository(SetDBSource());
            _inventoryLogRepository = new InventoryLogRepository();
        }

        #endregion Private Methods
    }
}

using Sofos2ToDatawarehouse.Domain.Entity.General;
using Sofos2ToDatawarehouse.Infrastructure.Helper;
using Sofos2ToDatawarehouse.Infrastructure.Repository.ColaStub;
using Sofos2ToDatawarehouse.Infrastructure.Repository.General;
using Sofos2ToDatawarehouse.Infrastructure.Repository.Logs.ColaStub;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Extractor.ColaStub.Controller
{
    public class ColaStubController
    {
        #region Private Declaration

        private string _module = "COLASTUB";

        private string dropSitePathExtractedColaStubBase = string.Empty;
        private string dropSitePathTransferredColaStubBase = string.Empty;
        private string dropSitePathLogsColaStubBase = string.Empty;

        private ColaStubRepository _colaStubRepository;
        private ColaStubLogRepository _colaStubLogRepository;
        private DropSiteModelRepository _dropSiteModelRepository;

        #endregion Private Declaration

        public ColaStubController()
        {
            InitilizeDropSiteAndColaStubRepositories();
            InitializeFolders();
        }

        #region Public Methods

        public void ProcessExtraction()
        {
            try
            {
                var lastIdLedgerLog = AppSettingHelper.GetSetting("lastIdLedgerLogSales");
                int lastIdlegerFromLog = Int32.Parse(lastIdLedgerLog);

                var colaStubTransaction = _colaStubRepository.GetColaStubData(_dropSiteModelRepository.DropSiteModel.QueryMaxFetchLimit, lastIdlegerFromLog);

                if (colaStubTransaction != null)
                {
                    _colaStubLogRepository.ExportToJSONFile(colaStubTransaction, _module, _colaStubRepository._company.BranchCode, dropSitePathExtractedColaStubBase, dropSitePathLogsColaStubBase);
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
            dropSitePathExtractedColaStubBase = Path.Combine(dropSitePathExtractedBase, _dropSiteModelRepository.DropSiteModel.DropSitePathColaStub);
            if (!Directory.Exists(dropSitePathExtractedColaStubBase))
                Directory.CreateDirectory(dropSitePathExtractedColaStubBase);

            string dropSitePathTransferedBase = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathTransferred);
            dropSitePathTransferredColaStubBase = Path.Combine(dropSitePathTransferedBase, _dropSiteModelRepository.DropSiteModel.DropSitePathColaStub);
            if (!Directory.Exists(dropSitePathTransferredColaStubBase))
                Directory.CreateDirectory(dropSitePathTransferredColaStubBase);

            string dropSitePathLogsBase = Path.Combine(_dropSiteModelRepository.DropSiteModel.DropSitePath, _dropSiteModelRepository.DropSiteModel.DropSitePathLog);
            dropSitePathLogsColaStubBase = Path.Combine(dropSitePathLogsBase, _dropSiteModelRepository.DropSiteModel.DropSitePathColaStub);
            if (!Directory.Exists(dropSitePathLogsColaStubBase))
                Directory.CreateDirectory(dropSitePathLogsColaStubBase);
        }



        private void InitilizeDropSiteAndColaStubRepositories()
        {
            _dropSiteModelRepository = new DropSiteModelRepository()
            {
                DropSiteModel = new DropSiteModel()
                {
                    DropSitePath = Properties.Settings.Default.DROPSITE_FOLDER,
                    QueryMaxFetchLimit = Convert.ToInt32(Properties.Settings.Default.MAX_FETCH_LIMIT)
                }
            };
            _colaStubRepository = new ColaStubRepository(SetDBSource());
            _colaStubLogRepository = new ColaStubLogRepository();
        }

        #endregion Private Methods
    }
}

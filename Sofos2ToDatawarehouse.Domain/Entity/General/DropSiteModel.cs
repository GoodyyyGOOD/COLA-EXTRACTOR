using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Domain.Entity.General
{
    public class DropSiteModel
    {
        public string DropSitePath { get; set; }
        public string DropSitePathExtracted { get; set; } = @"EXTRACTED\";
        public string DropSitePathTransferred { get; set; } = @"TRANSFERED\";

        public string DropSitePathInventoryGI { get; set; } = @"GI\";
        public string DropSitePathInvetoryGR { get; set; } = @"GR\";
        public string DropSitePathInventorySR { get; set; } = @"SR\";

        public string DropSitePathPurchasingGRPO { get; set; } = @"GRPO\";
        public string DropSitePathPurchasingPR { get; set; } = @"PR\";
        public string DropSitePathPurchasingRG { get; set; } = @"RG\";

        public string DropSitePathPaiwi { get; set; } = @"PAIWI\";
        public string DropSitePathBatch { get; set; } = @"BATCH\";
        public string DropSitePathCGO { get; set; } = @"CGO\";

        public string DropSitePathSalesRC { get; set; } = @"RC\";
        public string DropSitePathSales { get; set; } = @"SALES\";

        public string DropSitePathLog { get; set; } = @"LOGS\";

        public string ModuleDropSitePath { get; set; }
        public int QueryMaxFetchLimit { get; set; }
        public int QueryStartAtLedgerId { get; set; }

    }
}

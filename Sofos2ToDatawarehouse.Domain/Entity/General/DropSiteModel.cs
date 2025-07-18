﻿using System;
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

        public string DropSitePathSales { get; set; } = @"SALES\";
        public string DropSitePathColaStub { get; set; } = @"COLASTUB\"; 
        public string DropSitePathCancelTransaction { get; set; } = @"CANCELTRANSACTION\";
        public string DropSitePathInventory { get; set; } = @"INVENTORY\";
        public string DropSitePathAccounting { get; set; } = @"ACCOUNTING\";
        public string DropSitePathCancelChargeAmount { get; set; } = @"CANCELCHARGEAMOUNT\";
        public string DropSitePathLog { get; set; } = @"LOGS\";

        public string ModuleDropSitePath { get; set; }
        public int QueryMaxFetchLimit { get; set; }
        public int QueryStartAtLedgerId { get; set; }

    }
}

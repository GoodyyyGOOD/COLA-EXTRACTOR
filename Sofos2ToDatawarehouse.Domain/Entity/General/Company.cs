using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Domain.Entity.General
{
    public class Company
    {
        public string MainSegment = string.Empty;
        public string BusinessSegment = string.Empty;
        public string BranchCode = string.Empty;
        public string WarehouseCode = string.Empty;
        public string BranchName = string.Empty;
        public int DataSourceId = 0;
    }
}

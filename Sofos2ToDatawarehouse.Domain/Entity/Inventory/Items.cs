using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Domain.Entity.Inventory
{
    public class Items
    {
        public bool Active { get; set; }
        public string ItemCode { get; set; }
        public string UomCode { get; set; }
        public string Barcode { get; set; }
        public decimal Cost { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal RunningQty { get; set; }
        public string BranchCode { get; set; }
        public string WarehouseCode { get; set; }
    }
}

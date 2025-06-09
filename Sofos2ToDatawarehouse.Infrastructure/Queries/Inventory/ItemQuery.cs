using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Infrastructure.Queries.Inventory
{
    public class ItemQuery
    {
        public static StringBuilder GetInventoryQuery(InventoryEnum process)
        {
            var sQuery = new StringBuilder();

            switch (process)
            {


                case InventoryEnum.InventoryHeader:

                    sQuery.Append(@"SELECT
                                    b.active,
                                    a.itemCode,
                                    b.uomCode,
                                    b.barcode,
                                    b.cost,
                                    b.sellingPrice,
                                    a.runningQuantity,
                                    c.branchCode,
                                    c.defaultWarehouse as warehouseCode
            
                                    FROM ii000 a, iiuom b, sscs0 c
                                    WHERE a.itemcode=b.itemcode
                                    AND b.uomcode IN ('COLA','SP','PCS','BAGS', 'KILO')
                                    AND b.active AND left(a.itemcode,3) IN ('GRO','MTP','RCE') AND cost!=0 AND a.isCola;
                                                    ");
                    break;

                default:
                    break;
            }

            return sQuery;
        }

        public enum InventoryEnum
        {
            InventoryHeader
        }
    }
}

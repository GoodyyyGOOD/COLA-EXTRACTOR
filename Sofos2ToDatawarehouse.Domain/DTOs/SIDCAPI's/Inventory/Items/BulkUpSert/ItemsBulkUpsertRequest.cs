using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Inventory.Items.Create;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaTransaction.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Inventory.Items.BulkUpSert
{
    public class ItemsBulkUpsertRequest
    {
        public List<CreateItemsCommand> CreateItemsCommand { get; set; }
    }
}

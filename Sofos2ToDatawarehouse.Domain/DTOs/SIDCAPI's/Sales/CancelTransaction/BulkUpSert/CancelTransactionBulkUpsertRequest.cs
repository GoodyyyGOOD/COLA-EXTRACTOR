using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.CancelTransaction.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.CancelTransaction.BulkUpSert
{
    public class CancelTransactionBulkUpsertRequest
    {
        public List<CreateCancelTransactionCommand> CreateCancelTransactionCommand { get; set; }
    }
}

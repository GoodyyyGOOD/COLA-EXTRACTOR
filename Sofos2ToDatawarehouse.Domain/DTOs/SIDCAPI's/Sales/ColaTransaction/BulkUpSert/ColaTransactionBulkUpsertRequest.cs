using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Accounting.ChargeAmount.Create;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaTransactions.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaTransactions.BulkUpSert
{
    public class ColaStubBulkUpsertRequest
    {
        public List<CreateColaTransactionCommand> CreateColaTransactionCommand { get; set; }
    }
}

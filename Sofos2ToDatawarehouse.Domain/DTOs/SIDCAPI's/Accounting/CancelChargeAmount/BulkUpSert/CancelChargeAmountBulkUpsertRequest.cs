using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Accounting.CancelChargeAmount.Create;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Accounting.ChargeAmount.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Accounting.CancelChargeAmount.BulkUpSert
{
    public class CancelChargeAmountBulkUpsertRequest
    {
        public List<CreateCancelChargeAmountCommand> CreateCancelChargeAmountCommand { get; set; }
    }
}

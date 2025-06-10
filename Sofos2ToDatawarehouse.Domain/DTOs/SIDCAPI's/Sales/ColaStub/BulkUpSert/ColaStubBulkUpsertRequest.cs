using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaStub.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaStub.BulkUpSert
{
    public class ColaStubBulkUpsertRequest
    {
        public List<CreateColaStubCommand> CreateColaStubCommand { get; set; }
    }
}

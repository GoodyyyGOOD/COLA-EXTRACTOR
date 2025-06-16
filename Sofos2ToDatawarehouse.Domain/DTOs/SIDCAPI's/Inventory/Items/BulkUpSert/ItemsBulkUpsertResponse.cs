using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Inventory.Items.BulkUpSert
{
    public class ItemsBulkUpsertResponse
    {
        public ApiResponse response { get; set; }

        public bool Succeeded => response?.status_code == 200;
    }

    public class ApiResponse
    {
        public int status_code { get; set; }
        public Result result { get; set; }
    }

    public class Result
    {
        public string message { get; set; }
    }
}

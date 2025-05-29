using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Domain.DTOs
{
    public class SIDCSalesServiceAPISettings
    {
        public string BaseUrl { get; set; }
        public string IdentityUrl { get; set; }
        public string AuthTokenUrl { get; set; }
        public string AuthUser { get; set; }
        public string AuthPassword { get; set; }

        public string SalesTransactionBulkPost { get; set; }
        public string SalesTransactionBulkUpSert { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Domain.DTOs
{
    public class SIDCAPIServiceSettings
    {
        public string AccountingBaseUrl { get; set; }
        public string BaseUrl { get; set; }
        public string InventoryBaseUrl { get; set; }
        public string SalesBaseUrl { get; set; }
        public string ColaStubBaseUrl { get; set; }
        public string CancelTransactionBaseUrl { get; set; }
        public string CancelChargeAmountBaseUrl { get; set; }
        public string IdentityUrl { get; set; }
        public string AuthTokenUrl { get; set; }
        public string AuthUser { get; set; }
        public string AuthPassword { get; set; }
    }
}

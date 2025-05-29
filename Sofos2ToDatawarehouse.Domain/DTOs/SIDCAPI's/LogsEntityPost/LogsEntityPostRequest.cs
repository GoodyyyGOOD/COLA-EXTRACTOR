using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.LogsEntityPost
{
    public class LogsEntityPostRequest
    {
        public string TransactionType { get; set; }
        public string ServiceType { get; set; }
        public string FileName { get; set; }
        public int FirstIdLedger { get; set; }
        public int LastIdLedger { get; set; }
        public int TransactionCount { get; set; }
        public int BranchId { get; set; } = 0;
        public string BranchCode { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}

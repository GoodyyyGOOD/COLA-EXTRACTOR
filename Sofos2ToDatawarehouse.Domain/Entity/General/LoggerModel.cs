using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Domain.Entity.General
{
    public class LoggerModel
    {
        public int Id { get; set; }
        public string Transactiontype { get; set; }
        public int FirstIdLedger { get; set; }
        public int LastIdLedger { get; set; }
        public int TransactionCount { get; set; }
        public string FileName { get; set; }
        public string CreatedDate { get; set; }
        public string FilePath { get; set; }
        public string BranchCode { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Domain.Entity.Sales
{
    public class ColaStubTransaction
    {
        public int Transnum { get; set; }
        public string Reference { get; set; }
        public string EmployeeId { get; set; }
        public bool Cancelled { get; set; }
        public string Status { get; set; }
        public string BranchCode { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Domain.Entity.Accounting
{
    public class AccountDetails
    {
        public int Transnum { get; set; }
        public string MemberId { get; set; }
        public string TransType { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal ChargeAmount { get; set; }
        public int ColaId { get; set; }
    }
}

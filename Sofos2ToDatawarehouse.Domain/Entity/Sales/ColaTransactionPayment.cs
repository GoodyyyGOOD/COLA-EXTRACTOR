using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Domain.Entity.Sales
{
    public class ColaTransactionPayment
    {
        public string Reference { get; set; }
        public long SalesModeOfPaymentId { get; set; }
        public string SalesModeOfPaymentCode { get; set; }
        public decimal Amount { get; set; }
        public decimal ChangeAmount { get; set; }
        public string CheckNumber { get; set; }
        public string BankCode { get; set; }
        public DateTime? CheckDate { get; set; }
        public DateTime? SystemDate { get; set; }
        public int GLAccountId { get; set; }
        public string GLAccountCode { get; set; }
        public string GLAccountName { get; set; }
        public string IdUser { get; set; }
        public int TransactionTypeId { get; set; }
        public string TransactionTypeCode { get; set; }
        public bool IsExtracted { get; set; }
    }
}

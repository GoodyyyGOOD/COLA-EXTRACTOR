using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Domain.Entity.Sales
{
    public class ColaTransaction
    {
        public int TransNum { get; set; }
        public DateTime TransDate { get; set; }
        public string TransType { get; set; }
        public string Reference { get; set; }
        public string CrossReference { get; set; }
        public int IsNoEffectOnInventory { get; set; }
        public int CustomerType { get; set; }
        public string MemberId { get; set; }
        public string MemberName { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string YoungCoopId { get; set; }
        public string YoungCoopName { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public decimal PaidToDate { get; set; }
        public decimal Total { get; set; }
        public decimal GrossTotal { get; set; }
        public decimal AmountTendered { get; set; }
        public decimal InterestPaid { get; set; }
        public decimal InterestBalance { get; set; }
        public int Cancelled { get; set; }
        public string Status { get; set; }
        public string Extracted { get; set; }
        public string ColaReference { get; set; }
        public string SegmentCode { get; set; }
        public string BusinessSegmentCode { get; set; }
        public string BranchCode { get; set; }
        public string Signatory { get; set; }
        public string Remarks { get; set; }
        public DateTime SystemDate { get; set; }
        public string IdUser { get; set; }
        public string LrBatch { get; set; }
        public string LrType { get; set; }
        public decimal SeniorDiscount { get; set; }
        public decimal FeedsDiscount { get; set; }
        public decimal Vat { get; set; }
        public decimal VatExemptSales { get; set; }
        public decimal VatAmount { get; set; }
        public decimal KanegoDiscount { get; set; }
        public string WarehouseCode { get; set; }
        public string LrReference { get; set; }
        public int ColaId { get; set; }
 
        public virtual List<ColaTransactionDetail> ColaTransactionDetail { get; set; }
    }
}

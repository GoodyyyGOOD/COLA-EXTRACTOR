using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaTransactions.Create
{
    public class CreateColaTransactionCommand
    {
        public int TransNum { get; set; }
        public DateTime TransDate { get; set; }
        public string TransType { get; set; }
        public string Reference { get; set; }
        public string CrossReference { get; set; }
        public bool IsNoEffectOnInventory { get; set; }
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
        public bool Cancelled { get; set; }
        public string Status { get; set; }
        public bool Extracted { get; set; }
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
        public ICollection<CreateColaTransactionDetailCommand> ColaTransactionDetail { get; set; }
    }
    public class CreateColaTransactionDetailCommand
    {
        public int DetailNum { get; set; }
        public int TransNum { get; set; }
        public string Barcode { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string UnitOfMeasureCode { get; set; }
        public string UnitOfMeasureDescription { get; set; }
        public decimal Quantity { get; set; }
        public decimal Cost { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal FeedsDiscount { get; set; }
        public decimal Total { get; set; }
        public decimal Conversion { get; set; }
        public DateTime SystemDate { get; set; }
        public decimal SeniorDiscount { get; set; }
        public decimal RunningQuantity { get; set; }
        public decimal KanegoDiscount { get; set; }
        public decimal AverageCost { get; set; }
        public decimal RunningValue { get; set; }
        public decimal RunningQty { get; set; }
        public decimal LineTotal { get; set; }
        public decimal Vat { get; set; }
        public decimal Vatable { get; set; }
        public decimal VatExempt { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaTransactions.Create
{
    public class CreateColaTransactionCommand
    {
        public int IdLedger { get; set; }
        public DateTime TransactionDate { get; set; }
        public int TransactionTypeId { get; set; }
        public string TransactionTypeCode { get; set; }
        public string Reference { get; set; }
        public string CrossReference { get; set; }
        public bool IsNoEffectOnInventory { get; set; }
        public int CustomerTypeId { get; set; }
        public int BusinessPartnerId { get; set; }
        public string BusinessPartnerCode { get; set; }
        public string BusinessPartnerName { get; set; }
        public string EmployeeCode { get; set; }
        public int GLAccountId { get; set; }
        public string GLAccountCode { get; set; }
        public string GLAccountName { get; set; }
        public decimal PaidToDate { get; set; }
        public decimal Total { get; set; }
        public decimal AmountTendered { get; set; }
        public decimal InterestPaid { get; set; }
        public decimal InterestBalance { get; set; }
        public bool IsCancelled { get; set; }
        public int SalesStatusId { get; set; }
        public string SalesStatusDescription { get; set; }
        public bool IsExtracted { get; set; }
        public string ColaReference { get; set; }
        public int MainSegmentId { get; set; }
        public string MainSegmentCode { get; set; }
        public int BusinessSegmentId { get; set; }
        public string BusinessSegmentCode { get; set; }
        public int YoungCoopId { get; set; }
        public string YoungCoopCode { get; set; }
        public string YoungCoopName { get; set; }
        public int BranchId { get; set; }
        public string BranchCode { get; set; }
        public string Signatory { get; set; }
        public string Remarks { get; set; }
        public DateTime SystemDate { get; set; }
        public decimal SeniorDiscount { get; set; }
        public decimal FeedsDiscount { get; set; }
        public decimal Vat { get; set; }
        public decimal VatExemptSales { get; set; }
        public decimal VatAmount { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseCode { get; set; }
        public string LrBatch { get; set; }
        public string LrType { get; set; }
        public string LrReference { get; set; }
        public decimal KanegoDiscount { get; set; }
        public decimal GrossTotal { get; set; }
        public string Sow { get; set; }
        public string Parity { get; set; }
        public decimal InterestComputed { get; set; }
        public decimal DeductionDiscount { get; set; }
        public string Series { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public bool IsAllowedNoEffectInventory { get; set; }
        public bool IsPrinted { get; set; }
        public string IdUser { get; set; }
        public string AccountNumber { get; set; }
        public string TerminalNumber { get; set; }
        public string MinNumber { get; set; }
        public int DataSourceId { get; set; }
        public int ExternalId { get; set; }
        public string SeniorId { get; set; }
        public bool IsEcommerce { get; set; }
        public bool DwExtract { get; set; }
        public bool Returned { get; set; }
        public int ColaId { get; set; }
        public bool AvailPromo { get; set; }
        public bool IsFpSignatory { get; set; }
        public bool IsDuplicated { get; set; }
        public bool IsSmsSent { get; set; }
        public bool IsInsert { get; set; }
        public bool IsPromoWinner { get; set; }
        public string Module { get; set; }
        public decimal SchoolSuppliesDiscount { get; set; }
        public string LastUpdateUser { get; set; }
        public ICollection<CreateColaTransactionDetailCommand> ColaTransactionDetail { get; set; }
        public ICollection<CreateColaTransactionPaymentCommand> ColaTransactionPayment { get; set; }
    }
    public class CreateColaTransactionDetailCommand
    {
        public int SalesTransanctionId { get; set; }
        public string Barcode { get; set; }
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public int UnitOfMeasureId { get; set; }
        public string UnitOfMeasureCode { get; set; }
        public string UnitOfMeasureDescription { get; set; }
        public decimal Quantity { get; set; }
        public decimal Cost { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal FeedsDiscount { get; set; }
        public decimal Total { get; set; }
        public decimal Conversion { get; set; }
        public decimal SeniorDiscount { get; set; }
        public decimal KanegoDiscount { get; set; }
        public decimal RunningQuantity { get; set; }
        public decimal LineTotal { get; set; }
        public decimal DeductionDiscount { get; set; }
        public decimal Vat { get; set; }
        public decimal Vatable { get; set; }
        public decimal VatExempt { get; set; }
        public decimal CancelledQuantity { get; set; }
        public string IdUser { get; set; }
        public DateTime SystemDate { get; set; }
        public decimal AverageCost { get; set; }
        public decimal RunningValue { get; set; }
        public decimal TransactionValue { get; set; }
        public bool IsEcommerce { get; set; }
        public bool IsInsert { get; set; }
        public string Module { get; set; }
        public string LastUpdateUser { get; set; }
    }

    public class CreateColaTransactionPaymentCommand
    {
        public int SalesTransactionId { get; set; }
        public int SalesModeOfPaymentId { get; set; }
        public string SalesModeOfPaymentCode { get; set; }
        public decimal Amount { get; set; }
        public decimal ChangeAmount { get; set; }
        public string CheckNumber { get; set; }
        public string BankCode { get; set; }
        public DateTime? CheckDate { get; set; }
        public DateTime SystemDate { get; set; }
        public int GLAccountId { get; set; }
        public string GLAccountCode { get; set; }
        public string GLAccountName { get; set; }
        public string IdUser { get; set; }
        public int TransactionTypeId { get; set; }
        public string TransactionTypeCode { get; set; }
        public bool IsExtracted { get; set; }
    }
}

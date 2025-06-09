using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Domain.Entity.Sales
{
    public class ColaTransactionDetail
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
        public decimal RunningQty{ get; set; }
        public decimal LineTotal { get; set; }
        public decimal Vat { get; set; }
        public decimal Vatable { get; set; }
        public decimal VatExempt { get; set; }

    }
}





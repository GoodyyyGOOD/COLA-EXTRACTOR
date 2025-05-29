using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Infrastructure.Queries.Sales
{
    public class ColaTransactionQuery
    {
        public static StringBuilder GetSalesQuery(ColaTransactionEnum process)
        {
            var sQuery = new StringBuilder();

            switch (process)
            {


                case ColaTransactionEnum.ColaHeader:

                    sQuery.Append(@"SELECT a.transNum as 'IdLedger',
                                    DATE_FORMAT(a.transDate, '%Y-%m-%d %H:%i:%s') AS 'TransDate',
                                    a.transType AS 'TransType',
                                    a.reference AS 'Reference',
                                    a.crossreference AS 'CrossReference',
                                    a.NoEffectOnInventory AS 'NoEffectOnInventory',
                                    a.customerType AS 'CustomerType',
                                    a.memberId AS 'MemberId',
                                    a.memberName AS 'MemberName',
                                    a.employeeID as 'EmployeeCode',
                                    a.accountCode AS 'AccountCode',
                                    a.accountName AS 'AccountName',
                                    a.paidToDate AS 'PaidToDate',
                                    a.total AS 'Total',
                                    a.grossTotal AS 'GrossTotal',
                                    SUM(b.sellingPrice * b.quantity) AS 'TotalBasedOnDetails',
                                    a.amountTendered AS 'AmountTendered',
                                    a.interestPaid AS 'InterestPaid',
                                    a.interestBalance AS 'InterestBalance',
                                    a.intComputed as 'InterestComputed',
                                    a.dedDiscount AS 'DeductionDiscount',
                                    a.cancelled AS 'Cancelled',
                                    a.status AS 'Status',
                                    IF(a.extracted in ('Y','y'),1,0) AS 'IsExtracted',
                                    a.colaReference as 'ColaReference',
                                    a.signatory AS 'Signatory',
                                    a.remarks AS 'Remarks',
                                    a.idUser AS 'IdUser',
                                    a.lrBatch AS 'LrBatch',
                                    a.lrType AS 'LrType',
                                    a.lrReference as 'LrReference',
                                    a.srDiscount AS 'SrDiscount',
                                    a.kanegoDiscount AS 'Kanegodiscount',
                                    a.lastPaymentDate as 'LastPaymentDate',
                                    a.sow as 'Sow',
                                    a.parity as 'Parity',
                                    a.series as 'Series',
                                    a.accountNo as 'AccountNumber',
                                    a.terminalNo as 'TerminalNumber',
                                    a.minnumber as 'MinNumber',
                                    a.feedsDiscount AS 'Feedsdiscount',
                                    a.vat AS 'Vat',
                                    a.vatExemptSales AS 'VatExemptSales',
                                    a.vatAmount AS 'VatAmount',
                                    DATE_FORMAT(a.systemDate, '%Y-%m-%d %H:%i:%s') AS 'SystemDate',
                                    a.colaReference AS 'ColaReference',
                                    a.printed as 'IsPrinted',
                                    a.seniorId as 'SeniorId',
                                    a.allowNoEffectInventory as 'IsAllowedNoEffectInventory',
                                    a.dwextract,
                                    a.Returned,
                                    a.ColaId,
                                    a.AvailPromo,
                                    a.IsFPSignatory,
                                    a.IsDuplicated,
                                    a.IsSMSSent,
                                    a.youngCoopId as 'YoungCoopCode',
                                    a.youngCoopName as 'YoungCoopName',
                                    a.lastpaymentdate as 'LastPaymentDate',
                                    a.isInsert as 'IsInsert',
                                    a.module as 'Module',
                                    a.IsPromoWinner,
                                    a.SchoolSuppliesDiscount,
                                    a.last_update_user as 'LastUpdateUser'
                                    FROM sapt0 a
                                    INNER JOIN sapt1 b ON a.transNum = b.transNum
                                    where a.transnum >= (@lastIdLedger + 1)
                                    GROUP BY a.reference
                                    ORDER BY a.transnum ASC LIMIT @limitTransaction;
                            ");
                    break;

                case ColaTransactionEnum.ColaDetail:

                    sQuery.Append(@"SELECT
                                    b.reference AS 'Reference',
                                    a.barcode AS 'Barcode',
                                    a.itemCode AS 'ItemCode',
                                    a.itemDescription AS 'ItemDescription',
                                    a.uomCode AS 'UomCode',
                                    a.uomDescription AS 'UomDescription',
                                    SUM(a.quantity) AS 'Quantity',
                                    a.cost AS 'Cost',
                                    a.sellingPrice AS 'SellingPrice',
                                    a.feedsdiscount AS 'Feedsdiscount',
                                    SUM(a.total) AS 'Total',
                                    a.conversion AS 'Conversion',
                                    DATE_FORMAT(a.systemDate, '%Y-%m-%d %H:%i:%s') AS 'SystemDate',
                                    a.idUser AS 'IdUser',
                                    a.srDiscount AS 'Srdiscount',
                                    SUM(a.runningQty) AS 'RunningQuantity',
                                    a.kanegoDiscount AS 'KanegoDiscount',
                                    a.averageCost AS 'AverageCost',
                                    a.runningValue AS 'RunningValue',
                                    a.transValue AS 'TransactionValue',
                                    SUM(a.total) AS 'Linetotal',
                                    a.dedDiscount AS 'DeductionDiscount',
                                    a.vat AS 'Vat',
                                    a.vatable AS 'Vatable',
                                    a.vatExempt AS 'Vatexempt',
                                    a.cancelledQty AS 'CancelledQuantity',
                                    a.IsEcommerce,
                                    a.isInsert as 'IsInsert',
                                    a.module as 'Module',
                                    a.last_update_user AS 'LastUpdateUser'
                                    FROM sapt1 a
                                    INNER JOIN sapt0 b ON a.transNum = b.transNum
                                    WHERE a.transNum between  @lastIdLedger and @untilIdLedger
                                    GROUP BY b.reference, a.itemcode, a.uomCode
                                    ORDER BY b.transnum ASC;
                                    ");
                    break;

                case ColaTransactionEnum.ColaPayment:
                    sQuery.Append(@"SELECT a.reference AS 'Reference',
                                    b.paymentCode AS 'PaymentCode',
                                    b.amount AS 'Amount',
                                    b.checkNumber as 'CheckNumber',
                                    b.bankCode AS 'BankCode',
                                    DATE_FORMAT(b.checkDate, '%Y-%m-%d %H:%i:%s') AS 'CheckDate',
                                    DATE_FORMAT(b.systemDate, '%Y-%m-%d %H:%i:%s') AS 'SystemDate',
                                    b.idUser AS 'idUser',
                                    b.transType AS 'TransType',
                                    b.accountCode AS 'AccountCode',
                                    b.accountName AS 'AccountName',
                                    b.changeAmount as 'ChangeAmount',
                                    If(a.extracted in ('Y','y'),1,0) AS 'IsExtracted'
                                    FROM sapt0 a
                                    INNER JOIN ftp00 b on a.transnum=b.transnum
                                    where b.transNum between @lastIdLedger and @untilIdLedger
                                    #GROUP BY a.reference
                                    ORDER BY a.transnum ASC;
                                     ");
                    break;

                default:
                    break;
            }

            return sQuery;
        }

        public enum ColaTransactionEnum
        {
            ColaHeader, ColaDetail, ColaPayment
        }
    }
}

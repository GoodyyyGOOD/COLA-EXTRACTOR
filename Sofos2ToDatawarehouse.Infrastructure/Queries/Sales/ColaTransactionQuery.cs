using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Infrastructure.Queries.Sales
{
    public class ColaTransactionQuery
    {
        public static StringBuilder GetColaQuery(ColaTransactionEnum process)
        {
            var sQuery = new StringBuilder();

            switch (process)
            {


                case ColaTransactionEnum.ColaHeader:

                        sQuery.Append(@"SELECT 
                                    transNum,
                                    transDate,
                                    transType,
                                    reference,
                                    crossreference,
                                    NoEffectOnInventory,
                                    customerType,
                                    memberId,
                                    memberName,
                                    employeeID,
                                    employeeName,
                                    youngCoopID,
                                    youngCoopName,
                                    accountCode,
                                    accountName,
                                    paidToDate,
                                    Total,
                                    grossTotal,
                                    amountTendered,
                                    interestPaid,
                                    interestBalance,
                                    cancelled,
                                    status,
                                    extracted,
                                    colaReference,
                                    segmentCode,
                                    businessSegment,
                                    branchCode,
                                    signatory,
                                    remarks,
                                    systemDate,
                                    idUser,
                                    lrBatch,
                                    lrType,
                                    srDiscount,
                                    feedsDiscount,
                                    vat,
                                    vatExemptSales,
                                    vatAmount,
                                    kanegoDiscount,
                                    warehouseCode,
                                    lrReference,
                                    ColaID
                                    FROM sapt0
                                    WHERE transtype='CO' 
                                    AND isInsert=0
                                    AND transnum >= (@lastTransnum + 1)
                                    ORDER BY transnum ASC LIMIT @limitTransaction;
                            ");
                    break;

                case ColaTransactionEnum.ColaDetail:

                    sQuery.Append(@"SELECT
                                    b.detailNum,
                                    b.transNum,
                                    b.barcode,
                                    b.itemCode,
                                    b.itemDescription,
                                    b.uomCode,
                                    b.uomDescription,
                                    b.quantity,
                                    b.cost,
                                    b.sellingPrice,
                                    b.feedsdiscount,
                                    b.Total,
                                    b.conversion,
                                    b.systemDate,
                                    b.idUser,
                                    b.srdiscount,
                                    b.runningQuantity,
                                    b.kanegoDiscount,
                                    b.averageCost,
                                    b.runningValue,
                                    b.runningQty,
                                    b.linetotal,
                                    b.vat,
                                    b.vatexempt
                                    FROM sapt0 a inner join
                                    sapt1 b on a.transnum=b.transNum
                                    WHERE transtype='CO'
                                    AND b.isInsert=0
                                    AND a.transNum BETWEEN  @lastIdLedger and @untilIdLedger
                                    ORDER BY b.transnum ASC;
                                    ");
                    break;


                default:
                    break;
            }

            return sQuery;
        }

        public enum ColaTransactionEnum
        {
            ColaHeader, ColaDetail
        }
    }
}

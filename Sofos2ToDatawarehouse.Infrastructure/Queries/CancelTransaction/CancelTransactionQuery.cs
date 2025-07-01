using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sofos2ToDatawarehouse.Infrastructure.Queries.ColaStub.ColaStubQuery;

namespace Sofos2ToDatawarehouse.Infrastructure.Queries.CancelTransaction
{
    public class CancelTransactionQuery
    {
        public static StringBuilder GetCancelTransactionQuery(CancelTransactionEnum process)
        {
            var sQuery = new StringBuilder();

            switch (process)
            {


                case CancelTransactionEnum.CancelTransaction:

                    sQuery.Append(@"SELECT
                                    transnum,
                                    reference,
                                    crossreference,
                                    employeeID,
                                    cancelled,
                                    status,
                                    branchCode
                                    FROM sapt0
                                    WHERE transType ='CO'
                                    AND cancelled
                                    AND isExtracted = 0
                                    AND DATE(transDate) = CURRENT_DATE()
                                    ORDER BY transnum ASC LIMIT @limitTransaction;
                            ");
                    break;


                default:
                    break;
            }

            return sQuery;
        }
        public static string UpdateCancelTransactionQuery(CancelTransactionEnum process)
        {
            var sQuery = new StringBuilder();

            switch (process)
            {
                case CancelTransactionEnum.UpdateCancelTransaction:

                    sQuery.Append(@"UPDATE sapt0 SET isExtracted=1 WHERE transNum=@transNum");
                    break;

                default:
                    break;
            }

            return sQuery.ToString();
        }

        public enum CancelTransactionEnum
        {
            CancelTransaction, UpdateCancelTransaction
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                                    AND transnum >= (@lastTransnum + 1)
                                    AND MONTH(transdate) = MONTH(current_date())
                                    AND YEAR(transdate) = YEAR(current_date())
                                    ORDER BY transnum ASC LIMIT @limitTransaction;
                            ");
                    break;


                default:
                    break;
            }

            return sQuery;
        }

        public enum CancelTransactionEnum
        {
            CancelTransaction
        }
    }
}

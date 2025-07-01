using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sofos2ToDatawarehouse.Infrastructure.Queries.Sales.ColaTransactionQuery;

namespace Sofos2ToDatawarehouse.Infrastructure.Queries.ColaStub
{
    public class ColaStubQuery
    {
        public static StringBuilder GetColaStubQuery(ColaStubEnum process)
        {
            var sQuery = new StringBuilder();

            switch (process)
            {


                case ColaStubEnum.ColaStub:

                    sQuery.Append(@"SELECT
                                transnum,
                                reference,
                                employeeID,
                                cancelled,
                                status,
                                branchCode
                                FROM hct00
                                WHERE status = 'CLOSED'
                                AND NOT cancelled
                                AND isExtracted = 0
                                AND DATE(transDate) = CURRENT_DATE()
                                ORDER BY transnum ASC
                                LIMIT @limitTransaction;
                            ");
                    break;


                default:
                    break;
            }

            return sQuery;
        }

        public static string UpdateColaQuery(ColaStubEnum process)
        {
            var sQuery = new StringBuilder();

            switch (process)
            {
                case ColaStubEnum.UpdateColaHeader:

                    sQuery.Append(@"UPDATE hct00 SET isExtracted=1 WHERE transNum=@transNum");
                    break;

                default:
                    break;
            }

            return sQuery.ToString();
        }

        public enum ColaStubEnum
        {
            ColaStub, UpdateColaHeader
        }
    }
}

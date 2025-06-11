using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                                    WHERE status='CLOSED'
                                    AND NOT cancelled
                                    AND transnum >= (@lastTransnum + 1)
                                    ORDER BY transnum ASC LIMIT @limitTransaction;
                            ");
                    break;


                default:
                    break;
            }

            return sQuery;
        }

        public enum ColaStubEnum
        {
            ColaStub
        }
    }
}

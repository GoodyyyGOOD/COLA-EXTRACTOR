using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Infrastructure.Queries.Accounting
{
    public class ChargeAmountQuery
    {
        //public static StringBuilder GetAccountingQuery(AccountingEnum process)
        public static StringBuilder GetAccountingQuery(AccountingEnum process, DateTime? specificDate = null)
        {
            var sQuery = new StringBuilder();
            var dateToUse = specificDate ?? DateTime.Today;

            switch (process)
            {


                case AccountingEnum.AccountingHeader:

                    sQuery.Append($@"SELECT
                                    b.transNum,
									a.memberid,
                                    a.transtype,
                                    a.creditlimit,
                                    a.chargeAmount,
                                    a.colaid
            
                                    FROM acl00 a inner join sapt0 b on a.memberId = b.employeeID and a.colaID = b.colaID and a.transType=b.transType
                                    WHERE 
                                    b.transnum >= (@lastTransnum + 1)
                                    AND a.transtype='CO' 
                                    AND a.creditlimit != 0 
                                    AND a.colaid != 0
                                    AND NOT b.cancelled
                                    AND date(b.transdate)=('{dateToUse:yyyy-MM-dd}');
                                    #AND date(b.transdate)=('2024-01-25') 
                                    ORDER BY b.transnum ASC LIMIT @limitTransaction;
                                    ");
                    break;

                default:
                    break;
            }

            return sQuery;
        }

        public enum AccountingEnum
        {
            AccountingHeader
        }
    }
}

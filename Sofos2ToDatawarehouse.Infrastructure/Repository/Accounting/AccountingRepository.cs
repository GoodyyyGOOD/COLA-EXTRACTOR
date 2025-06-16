using Sofos2ToDatawarehouse.Domain.Entity.Accounting;
using Sofos2ToDatawarehouse.Domain.Entity.General;
using Sofos2ToDatawarehouse.Domain.Entity.Inventory;
using Sofos2ToDatawarehouse.Infrastructure.DbContext;
using Sofos2ToDatawarehouse.Infrastructure.Queries.Accounting;
using Sofos2ToDatawarehouse.Infrastructure.Queries.Inventory;
using Sofos2ToDatawarehouse.Infrastructure.Repository.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sofos2ToDatawarehouse.Infrastructure.Queries.Accounting.ChargeAmountQuery;
using static Sofos2ToDatawarehouse.Infrastructure.Queries.Inventory.ItemQuery;

namespace Sofos2ToDatawarehouse.Infrastructure.Repository.Accounting
{
    public class AccountingRepository
    {
        private GlobalRepository _globalRepository;
        public Company _company { get; set; }
        private string _dbSource { get; set; }

        public AccountingRepository(string dbSource)
        {
            _dbSource = dbSource;
            _globalRepository = new GlobalRepository(_dbSource);
            _company = _globalRepository.InitializeBranchForSofos2();
        }

        #region GET

        public List<AccountDetails> GetAccountingData(int maxFetchLimit, int startAtTransnum)
        {
            var accountingHeader = GetAccountingHeader(startAtTransnum, maxFetchLimit);



            return accountingHeader;
        }

        private List<AccountDetails> GetAccountingHeader(int lastTransnum, int maxFetchLimit)
        {
            try
            {
                var result = new List<AccountDetails>();

                var param = new Dictionary<string, object>()
                {
                    { "@lastTransnum", lastTransnum },
                    { "@limitTransaction", maxFetchLimit },
                };

                using (var conn = new ApplicationContext(_dbSource, ChargeAmountQuery.GetAccountingQuery(AccountingEnum.AccountingHeader), param))
                {
                    using (var dr = conn.MySQLReader())
                    {
                        while (dr.Read())
                        {
                            result.Add(new AccountDetails
                            {
                                Transnum = Convert.ToInt32(dr["transNum"]),
                                MemberId = dr["memberid"].ToString(),
                                TransType = dr["transtype"].ToString(),
                                CreditLimit = DBNull.Value == dr["creditlimit"] ? 0 : Convert.ToDecimal(dr["creditlimit"]),
                                ChargeAmount = DBNull.Value == dr["chargeAmount"] ? 0 : Convert.ToDecimal(dr["chargeAmount"]),
                                ColaId = Convert.ToInt32(dr["colaid"])

                            });
                        }
                    }
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        #endregion GET
    }
}

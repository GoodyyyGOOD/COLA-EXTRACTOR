using Sofos2ToDatawarehouse.Domain.Entity.General;
using Sofos2ToDatawarehouse.Domain.Entity.Sales;
using Sofos2ToDatawarehouse.Infrastructure.DbContext;
using Sofos2ToDatawarehouse.Infrastructure.Queries.ColaStub;
using Sofos2ToDatawarehouse.Infrastructure.Repository.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sofos2ToDatawarehouse.Infrastructure.Queries.ColaStub.ColaStubQuery;

namespace Sofos2ToDatawarehouse.Infrastructure.Repository.ColaStub
{
    public class ColaStubRepository
    {
        private GlobalRepository _globalRepository;
        public Company _company { get; set; }
        private string _dbSource { get; set; }

        public ColaStubRepository(string dbSource)
        {
            _dbSource = dbSource;
            _globalRepository = new GlobalRepository(_dbSource);
            _company = _globalRepository.InitializeBranchForSofos2();
        }

        #region GET

        public List<ColaStubTransaction> GetColaStubData(int maxFetchLimit, int startAtTransnum)
        {
            var colaStub = GetColaStub(startAtTransnum, maxFetchLimit);



            return colaStub;
        }

        private List<ColaStubTransaction> GetColaStub(int lastTransnum, int maxFetchLimit)
        {
            try
            {
                var result = new List<ColaStubTransaction>();

                var param = new Dictionary<string, object>()
                {
                    { "@lastTransnum", lastTransnum },
                    { "@limitTransaction", maxFetchLimit },
                };

                using (var conn = new ApplicationContext(_dbSource, ColaStubQuery.GetColaStubQuery(ColaStubEnum.ColaStub), param))
                {
                    using (var dr = conn.MySQLReader())
                    {
                        while (dr.Read())
                        {
                            result.Add(new ColaStubTransaction
                            {
                                Transnum = Convert.ToInt32(dr["transNum"]),
                                Reference = dr["reference"].ToString(),
                                EmployeeId = dr["employeeID"].ToString(),
                                Cancelled = Convert.ToBoolean(dr["cancelled"]),
                                Status = dr["status"].ToString(),
                                BranchCode = dr["branchCode"].ToString(),


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

using Sofos2ToDatawarehouse.Domain.Entity.General;
using Sofos2ToDatawarehouse.Infrastructure.DbContext;
using Sofos2ToDatawarehouse.Infrastructure.Seeds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Infrastructure.Repository.General
{
    public class GlobalRepository
    {
        private string _dbSource { get; set; }

        public GlobalRepository(string dbSource)
        {
            _dbSource = dbSource;
        }

        public Company InitializeBranchForSofos2()
        {
            Company company = new Company();
            try
            {
                string query = string.Empty;

                query = $@"SELECT SUBSTRING_INDEX(a.branchCode, '-', 2) as 'mainsegment',
                               SUBSTRING_INDEX(a.branchCode, '-', 3) as 'businesssegment',
                               a.branchcode as 'branchCode',
                               a.defaultWarehouse as 'whse' ,
                               b.description as branchName, 
                               0 as 'datasourceId'
                               FROM sscs0 a inner join ssw00 b where a.branchcode = b.branchcode";

                using (var conn = new ApplicationContext(_dbSource, new StringBuilder(query)))
                {
                    using (var dr = conn.MySQLReader())
                    {
                        while (dr.Read())
                        {
                            company.MainSegment = dr["mainsegment"].ToString();
                            company.BusinessSegment = dr["businesssegment"].ToString();
                            company.BranchCode = dr["branchCode"].ToString();
                            company.BranchName = dr["branchName"].ToString();
                            company.WarehouseCode = dr["whse"].ToString();
                            company.DataSourceId = DataSourceSeeds.SOFOS2.Id;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

            return company;
        }
    }
}

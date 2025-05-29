using Sofos2ToDatawarehouse.Domain.Entity.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Infrastructure.Seeds
{
    public class DataSourceSeeds
    {
        public static DataSource SOFOS2 = new DataSource()
        {
            Id = 0,
            Description = "SOFOS2"
        };

        public static DataSource SOFOS1 = new DataSource()
        {
            Id = 1,
            Description = "SOFOS1"
        };

        public static DataSource SAP = new DataSource()
        {
            Id = 2,
            Description = "SAP"
        };

        public static DataSource[] DataSourceList = new DataSource[]
        {
            DataSourceSeeds.SOFOS1,
            DataSourceSeeds.SOFOS2,
            DataSourceSeeds.SAP
        };
    }
}

using Sofos2ToDatawarehouse.Domain.Entity.General;
using Sofos2ToDatawarehouse.Domain.Entity.Inventory;
using Sofos2ToDatawarehouse.Domain.Entity.Sales;
using Sofos2ToDatawarehouse.Infrastructure.DbContext;
using Sofos2ToDatawarehouse.Infrastructure.Queries.Inventory;
using Sofos2ToDatawarehouse.Infrastructure.Queries.Sales;
using Sofos2ToDatawarehouse.Infrastructure.Repository.General;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sofos2ToDatawarehouse.Infrastructure.Queries.Inventory.ItemQuery;
using static Sofos2ToDatawarehouse.Infrastructure.Queries.Sales.ColaTransactionQuery;

namespace Sofos2ToDatawarehouse.Infrastructure.Repository.Inventory
{
    public class InventoryRepository
    {
        private GlobalRepository _globalRepository;
        public Company _company { get; set; }
        private string _dbSource { get; set; }

        public InventoryRepository(string dbSource)
        {
            _dbSource = dbSource;
            _globalRepository = new GlobalRepository(_dbSource);
            _company = _globalRepository.InitializeBranchForSofos2();
        }

        #region GET

        public List<Items> GetInventoryData()
        {
            var inventoryHeader = GetInventoryHeader();



            return inventoryHeader;
        }

        private List<Items> GetInventoryHeader()
        {
            try
            {
                var result = new List<Items>();

                using (var conn = new ApplicationContext(_dbSource, ItemQuery.GetInventoryQuery(InventoryEnum.InventoryHeader)))
                {
                    using (var dr = conn.MySQLReader())
                    {
                        while (dr.Read())
                        {
                            result.Add(new Items
                            {
                                Active = Convert.ToBoolean(dr["active"]),
                                ItemCode = dr["itemCode"].ToString(),
                                UomCode = dr["uomCode"].ToString(),
                                Barcode = dr["barcode"].ToString(),
                                Cost = DBNull.Value == dr["cost"] ? 0 : Convert.ToDecimal(dr["cost"]),
                                SellingPrice = DBNull.Value == dr["sellingPrice"] ? 0 : Convert.ToDecimal(dr["sellingPrice"]),
                                RunningQty = DBNull.Value == dr["runningQuantity"] ? 0 : Convert.ToDecimal(dr["runningQuantity"]),
                                BranchCode = dr["branchCode"].ToString(),
                                WarehouseCode = dr["warehouseCode"].ToString()
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

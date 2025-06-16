using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaTransaction.Create;
using Sofos2ToDatawarehouse.Domain.Entity.General;
using Sofos2ToDatawarehouse.Domain.Entity.Sales;
using Sofos2ToDatawarehouse.Infrastructure.DbContext;
using Sofos2ToDatawarehouse.Infrastructure.Queries.Sales;
using Sofos2ToDatawarehouse.Infrastructure.Repository.General;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sofos2ToDatawarehouse.Infrastructure.Queries.Sales.ColaTransactionQuery;

namespace Sofos2ToDatawarehouse.Infrastructure.Repository.Sales
{
    public class SalesRepository
    {
        private GlobalRepository _globalRepository;
        public Company _company { get; set; }
        private string _dbSource { get; set; }

        public SalesRepository(string dbSource)
        {
            _dbSource = dbSource;
            _globalRepository = new GlobalRepository(_dbSource);
            _company = _globalRepository.InitializeBranchForSofos2();
        }

        #region GET

        public List<ColaTransaction> GetColaData(int maxFetchLimit, int startAtLedgerId)
        {
            var colaHeader = GetColaHeader(startAtLedgerId, maxFetchLimit);

            if (colaHeader.Count == 0)
                return null;

            int lastIdLedger = colaHeader.Min(o => o.TransNum);
            int untilIdLedger = colaHeader.Max(o => o.TransNum);
            var colaDetails = GetColaItems(lastIdLedger, untilIdLedger);

            colaHeader.ForEach(x =>
            {
                x.ColaTransactionDetail = colaDetails.Where(o => o.TransNum == x.TransNum).ToList();
                
            });

            return colaHeader;
        }

        private List<ColaTransaction> GetColaHeader(int lastIdLedger, int maxFetchLimit)
        {
            try
            {
                var result = new List<ColaTransaction>();

                var param = new Dictionary<string, object>()
                {
                    { "@lastTransnum", lastIdLedger },
                    { "@limitTransaction", maxFetchLimit },
                };

                using (var conn = new ApplicationContext(_dbSource, ColaTransactionQuery.GetColaQuery(ColaTransactionEnum.ColaHeader), param))
                {
                    using (var dr = conn.MySQLReader())
                    {
                        while (dr.Read())
                        {
                            result.Add(new ColaTransaction
                            {
                                TransNum = dr["transNum"] == DBNull.Value ? 0 : Convert.ToInt32(dr["transNum"]),
                                TransDate = dr["transDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["transDate"]),
                                TransType = dr["transType"].ToString(),
                                Reference = dr["reference"].ToString(),
                                CrossReference = dr["crossreference"].ToString(),
                                //IsNoEffectOnInventory = dr["NoEffectOnInventory"] == DBNull.Value ? false : Convert.ToBoolean(dr["NoEffectOnInventory"]),
                                IsNoEffectOnInventory = dr["NoEffectOnInventory"] == DBNull.Value ? 0 : Convert.ToInt32(dr["NoEffectOnInventory"]),
                                CustomerType = dr["customerType"] == DBNull.Value ? 0 : dr["customerType"].ToString() == "Member" ? 1 : 2,
                                MemberId = dr["memberId"].ToString(),
                                MemberName = dr["memberName"].ToString(),
                                EmployeeId = dr["employeeID"].ToString(),
                                EmployeeName = dr["employeeName"].ToString(),
                                YoungCoopId = dr["youngCoopID"].ToString(),
                                YoungCoopName = dr["youngCoopName"].ToString(),
                                AccountCode = dr["accountCode"].ToString(),
                                AccountName = dr["accountName"].ToString(),
                                PaidToDate = dr["paidToDate"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["paidToDate"]),
                                Total = dr["Total"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["Total"]),
                                GrossTotal = dr["grossTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["grossTotal"]),
                                AmountTendered = dr["amountTendered"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["amountTendered"]),
                                InterestPaid = dr["interestPaid"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["interestPaid"]),
                                InterestBalance = dr["interestBalance"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["interestBalance"]),
                                //Cancelled = dr["cancelled"] == DBNull.Value ? false : Convert.ToBoolean(dr["cancelled"]),
                                Cancelled = dr["cancelled"] == DBNull.Value ? 0 : Convert.ToInt32(dr["cancelled"]),
                                Status = dr["status"].ToString(),
                                //Extracted = dr["extracted"] == DBNull.Value ? false : Convert.ToBoolean(dr["extracted"]),
                                Extracted = dr["extracted"].ToString(),
                                ColaReference = dr["colaReference"].ToString(),
                                SegmentCode = dr["segmentCode"].ToString(),
                                BusinessSegmentCode = dr["businessSegment"].ToString(),
                                BranchCode = dr["branchCode"].ToString(),
                                Signatory = dr["signatory"].ToString(),
                                Remarks = dr["remarks"].ToString(),
                                SystemDate = dr["systemDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["systemDate"]),
                                IdUser = dr["IdUser"].ToString(),
                                LrBatch = dr["lrBatch"].ToString(),
                                LrType = dr["lrType"].ToString(),
                                SeniorDiscount = dr["srDiscount"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["srDiscount"]),
                                FeedsDiscount = dr["FeedsDiscount"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["FeedsDiscount"]),
                                Vat = dr["vat"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["vat"]),
                                VatExemptSales = dr["vatExemptSales"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["vatExemptSales"]),
                                VatAmount = dr["vatAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["vatAmount"]),
                                KanegoDiscount = dr["Kanegodiscount"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["Kanegodiscount"]),
                                WarehouseCode = dr["warehouseCode"].ToString(),
                                LrReference = dr["lrReference"] == DBNull.Value ? "" : dr["lrReference"].ToString(),
                                ColaId = dr["colaId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["colaId"]),


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

        private List<ColaTransactionDetail> GetColaItems(int lastIdLedger, int untilIdLedger)
        {
            try
            {
                var result = new List<ColaTransactionDetail>();

                var param = new Dictionary<string, object>()
                {
                    { "@lastIdLedger", lastIdLedger },
                    { "@untilIdLedger", untilIdLedger }
                };

                using (var conn = new ApplicationContext(_dbSource, ColaTransactionQuery.GetColaQuery(ColaTransactionEnum.ColaDetail), param))
                {
                    using (var dr = conn.MySQLReader())
                    {
                        while (dr.Read())
                        {
                            result.Add(new ColaTransactionDetail
                            {
                                DetailNum = Convert.ToInt32(dr["detailNum"]),
                                TransNum = Convert.ToInt32(dr["transNum"]),
                                Barcode =  dr["barcode"].ToString(),
                                ItemCode = dr["itemCode"].ToString(),
                                ItemDescription = dr["itemDescription"].ToString(),
                                UnitOfMeasureCode = dr["uomCode"].ToString(),
                                UnitOfMeasureDescription = dr["uomDescription"].ToString(),
                                Quantity = DBNull.Value == dr["quantity"] ? 0 : Convert.ToDecimal(dr["quantity"]),
                                Cost = DBNull.Value == dr["cost"] ? 0 : Convert.ToDecimal(dr["cost"]),
                                SellingPrice = DBNull.Value == dr["sellingPrice"] ? 0 : Convert.ToDecimal(dr["sellingPrice"]),
                                FeedsDiscount = DBNull.Value == dr["feedsDiscount"] ? 0 : Convert.ToDecimal(dr["feedsDiscount"]),
                                Total = DBNull.Value == dr["Total"] ? 0 : Convert.ToDecimal(dr["Total"]),
                                Conversion = DBNull.Value == dr["conversion"] ? 0 : Convert.ToDecimal(dr["conversion"]),
                                SystemDate = DBNull.Value == dr["systemDate"] ? DateTime.MinValue : Convert.ToDateTime(dr["systemDate"]),
                                SeniorDiscount = DBNull.Value == dr["srdiscount"] ? 0 : Convert.ToDecimal(dr["srdiscount"]),
                                RunningQuantity = DBNull.Value == dr["runningQuantity"] ? 0 : Convert.ToDecimal(dr["runningQuantity"]),
                                KanegoDiscount = DBNull.Value == dr["kanegoDiscount"] ? 0 : Convert.ToDecimal(dr["kanegoDiscount"]),
                                AverageCost = DBNull.Value == dr["averageCost"] ? 0 : Convert.ToDecimal(dr["averageCost"]),
                                RunningValue = DBNull.Value == dr["runningValue"] ? 0 : Convert.ToDecimal(dr["runningValue"]),
                                RunningQty = DBNull.Value == dr["runningQty"] ? 0 : Convert.ToDecimal(dr["runningQty"]),
                                LineTotal = DBNull.Value == dr["lineTotal"] ? 0 : Convert.ToDecimal(dr["lineTotal"]),
                                Vat = DBNull.Value == dr["vat"] ? 0 : Convert.ToDecimal(dr["vat"]),
                                VatExempt = DBNull.Value == dr["vatExempt"] ? 0 : Convert.ToDecimal(dr["vatExempt"]),


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

        //public void MarkColaAsInserted(List<CreateColaTransactionCommand> colaTransactions)
        public async Task MarkColaAsInserted(List<CreateColaTransactionCommand> colaTransactions)
        {
            if (colaTransactions == null || !colaTransactions.Any())
                return;

            foreach (var transaction in colaTransactions)
            {
                try
                {
                    // Update header
                    var headerParam = new Dictionary<string, object>
                    {
                        { "@transNum", transaction.TransNum }
                    };

                    using (var conn = new ApplicationContext(_dbSource, ColaTransactionQuery.UpdateColaQuery(ColaTransactionEnum.UpdateColaHeader), headerParam))
                    {
                        conn.ExecuteMySQL();
                    }

                    // Update each detail
                    foreach (var detail in transaction.ColaTransactionDetail)
                    {
                        var detailParam = new Dictionary<string, object>
                        {
                            { "@detailNum", detail.DetailNum }
                        };

                        using (var conn = new ApplicationContext(_dbSource, ColaTransactionQuery.UpdateColaQuery(ColaTransactionEnum.UpdateColaDetail), detailParam))
                        {
                            conn.ExecuteMySQL();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle/log the exception as needed
                    throw new Exception($"Failed to update isInsert flag for transaction {transaction.TransNum}: {ex.Message}", ex);
                }
            }
        }




        #endregion GET

    }
}

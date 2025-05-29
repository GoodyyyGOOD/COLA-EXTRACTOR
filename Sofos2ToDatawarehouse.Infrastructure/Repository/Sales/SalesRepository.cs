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

        public List<ColaTransaction> GetSalesData(int maxFetchLimit, int startAtLedgerId)
        {
            var salesHeader = GetSalesHeader(startAtLedgerId, maxFetchLimit);

            if (salesHeader.Count == 0)
                return null;

            int lastIdLedger = salesHeader.Min(o => o.IdLedger);
            int untilIdLedger = salesHeader.Max(o => o.IdLedger);
            var colaDetails = GetColaItems(lastIdLedger, untilIdLedger);
            var colaPayments = GetColaPayments(lastIdLedger, untilIdLedger);

            salesHeader.ForEach(x =>
            {
                x.ColaTransactionDetail = colaDetails.Where(o => o.Reference == x.Reference).ToList();
                x.ColaTransactionPayment = colaPayments.Where(o => o.Reference == x.Reference).ToList();
            });

            return salesHeader;
        }

        private List<ColaTransaction> GetSalesHeader(int lastIdLedger, int maxFetchLimit)
        {
            try
            {
                var result = new List<ColaTransaction>();

                var param = new Dictionary<string, object>()
                {
                    { "@lastIdLedger", lastIdLedger },
                    { "@limitTransaction", maxFetchLimit },
                };

                using (var conn = new ApplicationContext(_dbSource, ColaTransactionQuery.GetSalesQuery(ColaTransactionEnum.ColaHeader), param))
                {
                    using (var dr = conn.MySQLReader())
                    {
                        while (dr.Read())
                        {
                            DateTime dateResult;
                            bool lastUpdatedDateIsValid = DateTime.TryParseExact(dr["LastPaymentDate"].ToString(),
                            "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateResult);
                            DateTime? lastPaymentDate = null;

                            if (lastUpdatedDateIsValid)
                                lastPaymentDate = dateResult;
                            result.Add(new ColaTransaction
                            {
                                IdLedger = Convert.ToInt32(dr["IdLedger"]),
                                TransactionDate = Convert.ToDateTime(dr["TransDate"]),
                                TransactionTypeCode = dr["TransType"].ToString(),
                                Reference = dr["Reference"].ToString(),
                                CrossReference = dr["CrossReference"].ToString(),
                                IsNoEffectOnInventory = Convert.ToBoolean(dr["NoEffectOnInventory"]),
                                IsAllowedNoEffectInventory = Convert.ToBoolean(dr["IsAllowedNoEffectInventory"]),
                                CustomerTypeId = DBNull.Value == dr["CustomerType"] ? 0 : dr["CustomerType"].ToString() == "Member" ? 1 : 2,
                                BusinessPartnerCode = dr["MemberId"].ToString(),
                                BusinessPartnerName = dr["MemberName"].ToString(),
                                EmployeeCode = dr["EmployeeCode"].ToString(),
                                YoungCoopCode = dr["YoungCoopCode"].ToString(),
                                YoungCoopName = dr["YoungCoopName"].ToString(),
                                GLAccountCode = dr["AccountCode"].ToString(),
                                GLAccountName = dr["AccountName"].ToString(),
                                PaidToDate = DBNull.Value == dr["PaidToDate"] ? 0 : Convert.ToDecimal(dr["PaidToDate"]),
                                Total = DBNull.Value == dr["Total"] ? 0 : Convert.ToDecimal(dr["Total"]),
                                InterestComputed = DBNull.Value == dr["InterestComputed"] ? 0 : Convert.ToDecimal(dr["InterestComputed"]),// for check
                                InterestPaid = DBNull.Value == dr["InterestPaid"] ? 0 : Convert.ToDecimal(dr["InterestPaid"]),
                                InterestBalance = DBNull.Value == dr["InterestBalance"] ? 0 : Convert.ToDecimal(dr["InterestBalance"]),
                                AmountTendered = DBNull.Value == dr["AmountTendered"] ? 0 : Convert.ToDecimal(dr["AmountTendered"]),
                                IsCancelled = Convert.ToBoolean(dr["Cancelled"]),
                                SalesStatusDescription = dr["Status"].ToString(),
                                SalesStatusId = dr["Status"].ToString() == "CLOSED" ? 2 : 1,
                                IsExtracted = Convert.ToBoolean(dr["IsExtracted"]),
                                IsDuplicated = Convert.ToBoolean(dr["IsDuplicated"]),
                                IsInsert = Convert.ToBoolean(dr["IsInsert"]),
                                IsPromoWinner = Convert.ToBoolean(dr["IsPromoWinner"]),
                                ColaReference = dr["ColaReference"].ToString(),
                                Signatory = dr["Signatory"].ToString(),
                                Remarks = dr["Remarks"].ToString(),
                                IdUser = dr["IdUser"].ToString(),
                                LrBatch = dr["LrBatch"].ToString(),
                                LrType = dr["LrType"].ToString(),
                                SeniorDiscount = DBNull.Value == dr["SrDiscount"] ? 0 : Convert.ToDecimal(dr["SrDiscount"]),
                                FeedsDiscount = DBNull.Value == dr["FeedsDiscount"] ? 0 : Convert.ToDecimal(dr["FeedsDiscount"]),
                                SchoolSuppliesDiscount = DBNull.Value == dr["SchoolSuppliesDiscount"] ? 0 : Convert.ToDecimal(dr["SchoolSuppliesDiscount"]),
                                Vat = DBNull.Value == dr["Vat"] ? 0 : Convert.ToDecimal(dr["Vat"]),// for check
                                VatExemptSales = DBNull.Value == dr["VatExemptSales"] ? 0 : Convert.ToDecimal(dr["VatExemptSales"]),// for check
                                VatAmount = DBNull.Value == dr["VatAmount"] ? 0 : Convert.ToDecimal(dr["VatAmount"]),// for check
                                LrReference = DBNull.Value == dr["LrReference"] ? "" : dr["LrReference"].ToString(),
                                LastPaymentDate = DBNull.Value == dr["LastPaymentDate"] ? lastPaymentDate : Convert.ToDateTime(dr["LastPaymentDate"]),
                                Sow = DBNull.Value == dr["Sow"] ? "" : dr["Sow"].ToString(),
                                Parity = DBNull.Value == dr["Parity"] ? "" : dr["Parity"].ToString(),
                                Series = DBNull.Value == dr["Series"] ? "" : dr["Series"].ToString(),
                                KanegoDiscount = DBNull.Value == dr["Kanegodiscount"] ? 0 : Convert.ToDecimal(dr["Kanegodiscount"]),
                                AccountNumber = DBNull.Value == dr["AccountNumber"] ? "" : dr["AccountNumber"].ToString(),
                                DeductionDiscount = DBNull.Value == dr["DeductionDiscount"] ? 0 : Convert.ToDecimal(dr["DeductionDiscount"]),
                                TerminalNumber = DBNull.Value == dr["TerminalNumber"] ? "" : dr["TerminalNumber"].ToString(),
                                MinNumber = DBNull.Value == dr["MinNumber"] ? "" : dr["MinNumber"].ToString(),
                                GrossTotal = DBNull.Value == dr["GrossTotal"] ? 0 : Convert.ToDecimal(dr["GrossTotal"]), // for check
                                SystemDate = Convert.ToDateTime(dr["SystemDate"]),
                                SeniorId = DBNull.Value == dr["SeniorId"] ? "" : dr["SeniorId"].ToString(),
                                LastUpdateUser = DBNull.Value == dr["LastUpdateUser"] ? "" : dr["LastUpdateUser"].ToString(),
                                Module = DBNull.Value == dr["Module"] ? "" : dr["Module"].ToString(),
                                ExternalId = Convert.ToInt32(dr["IdLedger"]),
                                IsPrinted = Convert.ToInt32(dr["IsPrinted"]) == 1 ? true : false,
                                DwExtract = Convert.ToBoolean(dr["dwextract"]),
                                Returned = Convert.ToBoolean(dr["Returned"]),
                                ColaId = Convert.ToInt32(dr["ColaId"]),
                                AvailPromo = Convert.ToBoolean(dr["AvailPromo"]),
                                IsFpSignatory = Convert.ToBoolean(dr["IsFPSignatory"]),
                                IsSmsSent = Convert.ToBoolean(dr["IsSMSSent"]),
                                DataSourceId = _company.DataSourceId,
                                MainSegmentCode = _company.MainSegment,
                                BusinessSegmentCode = _company.BusinessSegment,
                                BranchCode = _company.BranchCode,
                                WarehouseCode = _company.WarehouseCode
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

                using (var conn = new ApplicationContext(_dbSource, ColaTransactionQuery.GetSalesQuery(ColaTransactionEnum.ColaDetail), param))
                {
                    using (var dr = conn.MySQLReader())
                    {
                        while (dr.Read())
                        {
                            result.Add(new ColaTransactionDetail
                            {
                                Reference = dr["Reference"].ToString(),
                                Barcode = DBNull.Value == dr["Barcode"] ? "" : dr["Barcode"].ToString(),
                                ItemCode = DBNull.Value == dr["ItemCode"] ? "" : dr["ItemCode"].ToString(),
                                ItemDescription = DBNull.Value == dr["ItemDescription"] ? "" : dr["ItemDescription"].ToString(),
                                UnitOfMeasureCode = DBNull.Value == dr["UomCode"] ? "" : dr["UomCode"].ToString(),
                                UnitOfMeasureDescription = DBNull.Value == dr["UomDescription"] ? "" : dr["UomDescription"].ToString(),
                                Quantity = DBNull.Value == dr["Quantity"] ? 0 : Convert.ToDecimal(dr["Quantity"]),
                                Cost = DBNull.Value == dr["Cost"] ? 0 : Convert.ToDecimal(dr["Cost"]),
                                SellingPrice = DBNull.Value == dr["SellingPrice"] ? 0 : Convert.ToDecimal(dr["SellingPrice"]),
                                FeedsDiscount = DBNull.Value == dr["Feedsdiscount"] ? 0 : Convert.ToDecimal(dr["Feedsdiscount"]),
                                Total = DBNull.Value == dr["Total"] ? 0 : Convert.ToDecimal(dr["Total"]),
                                Conversion = DBNull.Value == dr["Conversion"] ? 0 : Convert.ToDecimal(dr["Conversion"]),
                                SystemDate = Convert.ToDateTime(dr["SystemDate"]),
                                IdUser = DBNull.Value == dr["IdUser"] ? "" : dr["IdUser"].ToString(),
                                SeniorDiscount = DBNull.Value == dr["Srdiscount"] ? 0 : Convert.ToDecimal(dr["Srdiscount"]),
                                KanegoDiscount = DBNull.Value == dr["KanegoDiscount"] ? 0 : Convert.ToDecimal(dr["KanegoDiscount"]),
                                RunningQuantity = DBNull.Value == dr["RunningQuantity"] ? 0 : Convert.ToDecimal(dr["RunningQuantity"]),
                                Vat = DBNull.Value == dr["Vat"] ? 0 : Convert.ToDecimal(dr["Vat"]),
                                Vatable = DBNull.Value == dr["Vatable"] ? 0 : Convert.ToDecimal(dr["Vatable"]),
                                VatExempt = DBNull.Value == dr["Vatexempt"] ? 0 : Convert.ToDecimal(dr["Vatexempt"]),
                                LineTotal = DBNull.Value == dr["LineTotal"] ? 0 : Convert.ToDecimal(dr["LineTotal"]),
                                DeductionDiscount = DBNull.Value == dr["DeductionDiscount"] ? 0 : Convert.ToDecimal(dr["DeductionDiscount"]),
                                CancelledQuantity = DBNull.Value == dr["CancelledQuantity"] ? 0 : Convert.ToDecimal(dr["CancelledQuantity"]),
                                IsEcommerce = Convert.ToBoolean(dr["IsEcommerce"]),
                                IsInsert = Convert.ToBoolean(dr["IsInsert"]),
                                Module = DBNull.Value == dr["Module"] ? "" : dr["Module"].ToString(),
                                LastUpdateUser = DBNull.Value == dr["LastUpdateUser"] ? "" : dr["LastUpdateUser"].ToString(),

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

        private List<ColaTransactionPayment> GetColaPayments(int lastIdLedger, int untilIdLedger)
        {
            try
            {
                var result = new List<ColaTransactionPayment>();

                var param = new Dictionary<string, object>()
                {
                    { "@lastIdLedger", lastIdLedger },
                    { "@untilIdLedger", untilIdLedger }
                };

                using (var conn = new ApplicationContext(_dbSource, ColaTransactionQuery.GetSalesQuery(ColaTransactionEnum.ColaPayment), param))
                {
                    using (var dr = conn.MySQLReader())
                    {
                        while (dr.Read())
                        {
                            DateTime dateResult;
                            bool lastUpdatedDateIsValid = DateTime.TryParseExact(dr["CheckDate"].ToString(),
                            "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateResult);
                            DateTime? checkDate = null;

                            if (lastUpdatedDateIsValid)
                                checkDate = dateResult;

                            result.Add(new ColaTransactionPayment
                            {
                                SalesModeOfPaymentId = 0,
                                Reference = dr["Reference"].ToString(),
                                SalesModeOfPaymentCode = DBNull.Value == dr["PaymentCode"] ? "" : dr["PaymentCode"].ToString(),
                                Amount = DBNull.Value == dr["Amount"] ? 0 : Convert.ToDecimal(dr["Amount"]),
                                ChangeAmount = DBNull.Value == dr["ChangeAmount"] ? 0 : Convert.ToDecimal(dr["ChangeAmount"]),
                                CheckNumber = DBNull.Value == dr["CheckNumber"] ? "" : dr["CheckNumber"].ToString(),
                                BankCode = DBNull.Value == dr["BankCode"] ? "" : dr["BankCode"].ToString(),
                                CheckDate = DBNull.Value == dr["CheckDate"] ? (DateTime?)null : Convert.ToDateTime(dr["CheckDate"]),
                                //CheckDate = checkDate,
                                SystemDate = Convert.ToDateTime(dr["SystemDate"]),
                                GLAccountId = 0,
                                GLAccountCode = DBNull.Value == dr["AccountCode"] ? "" : dr["AccountCode"].ToString(),
                                GLAccountName = DBNull.Value == dr["AccountName"] ? "" : dr["AccountName"].ToString(),
                                IdUser = dr["IdUser"].ToString(),
                                IsExtracted = Convert.ToBoolean(dr["IsExtracted"]),
                                TransactionTypeCode = dr["TransType"].ToString()
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

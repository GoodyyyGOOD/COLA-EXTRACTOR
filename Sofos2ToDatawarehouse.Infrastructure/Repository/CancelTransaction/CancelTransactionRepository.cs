using MySqlConnector;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.CancelTransaction.Create;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaStub.Create;
using Sofos2ToDatawarehouse.Domain.Entity.General;
using Sofos2ToDatawarehouse.Domain.Entity.Sales;
using Sofos2ToDatawarehouse.Infrastructure.DbContext;
using Sofos2ToDatawarehouse.Infrastructure.Queries.CancelTransaction;
using Sofos2ToDatawarehouse.Infrastructure.Queries.ColaStub;
using Sofos2ToDatawarehouse.Infrastructure.Repository.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sofos2ToDatawarehouse.Infrastructure.Queries.CancelTransaction.CancelTransactionQuery;

namespace Sofos2ToDatawarehouse.Infrastructure.Repository.CancelTransaction
{
    public class CancelTransactionRepository
    {
        private GlobalRepository _globalRepository;
        public Company _company { get; set; }
        private string _dbSource { get; set; }

        public CancelTransactionRepository(string dbSource)
        {
            _dbSource = dbSource;
            _globalRepository = new GlobalRepository(_dbSource);
            _company = _globalRepository.InitializeBranchForSofos2();
        }

        #region GET

        public async Task<List<CancelTransactions>> GetCancelTransactionData(int maxFetchLimit, int startAtTransnum)
        {
            var CancelTransaction = GetCancelTransaction(startAtTransnum, maxFetchLimit);



            return CancelTransaction;
        }

        private List<CancelTransactions> GetCancelTransaction(int lastTransnum, int maxFetchLimit)
        {
            try
            {
                var result = new List<CancelTransactions>();

                var param = new Dictionary<string, object>()
                {
                    { "@lastTransnum", lastTransnum },
                    { "@limitTransaction", maxFetchLimit },
                };

                using (var conn = new ApplicationContext(_dbSource, CancelTransactionQuery.GetCancelTransactionQuery(CancelTransactionEnum.CancelTransaction), param))
                {
                    using (var dr = conn.MySQLReader())
                    {
                        while (dr.Read())
                        {
                            result.Add(new CancelTransactions
                            {
                                Transnum = Convert.ToInt32(dr["transNum"]),
                                Reference = dr["reference"].ToString(),
                                CrossReference = dr["crossreference"].ToString(),
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

        public async Task MarkColaAsExtracted(List<CreateCancelTransactionCommand> cancelTransaction)
        {
            if (cancelTransaction == null || !cancelTransaction.Any())
                return;

            foreach (var stub in cancelTransaction)
            {
                var conn = new MySqlConnection(_dbSource);
                await conn.OpenAsync();

                var trx = await conn.BeginTransactionAsync(IsolationLevel.Serializable);
                try
                {

                    var headerQuery = CancelTransactionQuery.UpdateCancelTransactionQuery(CancelTransactionEnum.UpdateCancelTransaction);
                    Console.WriteLine("Query: " + headerQuery);
                    Console.WriteLine("TransNum: " + stub.Transnum);
                    using (var headerCmd = new MySqlCommand(headerQuery, conn, (MySqlTransaction)trx))
                    {
                        Console.WriteLine(headerCmd.CommandText);
                        foreach (MySqlParameter param in headerCmd.Parameters)
                        {
                            Console.WriteLine($"{param.ParameterName} = {param.Value}");
                        }
                        headerCmd.Parameters.AddWithValue("@transNum", stub.Transnum);
                        await headerCmd.ExecuteNonQueryAsync();
                    }

                    await trx.CommitAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Exception: {ex}");
                    await trx.RollbackAsync();
                    throw new Exception($"Failed to update isInsert flag for transaction {stub.Transnum}: {ex.Message}", ex);
                }
                finally
                {
                    await conn.CloseAsync();
                    conn.Dispose();
                }
            }
        }

        #endregion GET
    }
}

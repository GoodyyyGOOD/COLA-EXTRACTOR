using MySqlConnector;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaStub.Create;
using Sofos2ToDatawarehouse.Domain.Entity.General;
using Sofos2ToDatawarehouse.Domain.Entity.Sales;
using Sofos2ToDatawarehouse.Infrastructure.DbContext;
using Sofos2ToDatawarehouse.Infrastructure.Queries.ColaStub;
using Sofos2ToDatawarehouse.Infrastructure.Queries.Sales;
using Sofos2ToDatawarehouse.Infrastructure.Repository.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sofos2ToDatawarehouse.Infrastructure.Queries.ColaStub.ColaStubQuery;
using static Sofos2ToDatawarehouse.Infrastructure.Queries.Sales.ColaTransactionQuery;

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

        public async Task<List<ColaStubTransaction>> GetColaStubData(int maxFetchLimit, int startAtTransnum)
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

        public async Task MarkColaAsExtracted(List<CreateColaStubCommand> colaStub)
        {
            if (colaStub == null || !colaStub.Any())
                return;

            foreach (var stub in colaStub)
            {
                var conn = new MySqlConnection(_dbSource);
                await conn.OpenAsync();

                var trx = await conn.BeginTransactionAsync(IsolationLevel.Serializable);
                try
                {
 
                    var headerQuery = ColaStubQuery.UpdateColaQuery(ColaStubEnum.UpdateColaHeader);
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

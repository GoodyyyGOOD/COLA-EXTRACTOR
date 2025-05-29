using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.LogsEntityPost;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaTransactions.BulkUpSert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Infrastructure.Services.LogsEntity
{
    #region Sales

    public class ProcessLogsService
    {
        public async Task ColaTransactionLogsServiceRequestAsync(ColaTransactionBulkUpsertRequest colaTransactionBulkUpsertRequest, SIDCAPILogsService sidcAPILogsService, string fileName, string branchCode)
        {
            string tokenForLogsService = await Task.Run(() => sidcAPILogsService.SendAuthenticationAsync());

            LogsEntityPostRequest logsEntityPostRequest = new LogsEntityPostRequest()
            {
                TransactionType = "SALES",
                ServiceType = "SALES SERVICE",
                FileName = fileName,
                FirstIdLedger = colaTransactionBulkUpsertRequest.CreateColaTransactionCommand.Min(o => o.IdLedger),
                LastIdLedger = colaTransactionBulkUpsertRequest.CreateColaTransactionCommand.Max(o => o.IdLedger),
                TransactionCount = colaTransactionBulkUpsertRequest.CreateColaTransactionCommand.Count(),
                BranchCode = branchCode
            };

            await sidcAPILogsService.SendPostAsync(logsEntityPostRequest, tokenForLogsService);
        }

        #endregion Sales
    }
}

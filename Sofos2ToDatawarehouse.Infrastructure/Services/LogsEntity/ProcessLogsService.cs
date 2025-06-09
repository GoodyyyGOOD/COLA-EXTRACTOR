using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Accounting.ChargeAmount.BulkUpSert;
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
                FirstIdLedger = colaTransactionBulkUpsertRequest.CreateColaTransactionCommand.Min(o => o.TransNum),
                LastIdLedger = colaTransactionBulkUpsertRequest.CreateColaTransactionCommand.Max(o => o.TransNum),
                TransactionCount = colaTransactionBulkUpsertRequest.CreateColaTransactionCommand.Count(),
                BranchCode = branchCode
            };

            await sidcAPILogsService.SendPostAsync(logsEntityPostRequest, tokenForLogsService);
        }

        #endregion Sales

        #region Accounting

        public async Task ChargeAmountLogsServiceRequestAsync(ChargeAmountBulkUpsertRequest chargeAmountBulkUpsertRequest, SIDCAPILogsService sidcAPILogsService, string fileName, string branchCode)
        {
            string tokenForLogsService = await Task.Run(() => sidcAPILogsService.SendAuthenticationAsync());

            LogsEntityPostRequest logsEntityPostRequest = new LogsEntityPostRequest()
            {
                TransactionType = "SALES",
                ServiceType = "SALES SERVICE",
                FileName = fileName,
                FirstIdLedger = chargeAmountBulkUpsertRequest.CreateChargeAmountCommand.Min(o => o.TransNum),
                LastIdLedger = chargeAmountBulkUpsertRequest.CreateChargeAmountCommand.Max(o => o.TransNum),
                TransactionCount = chargeAmountBulkUpsertRequest.CreateChargeAmountCommand.Count(),
                BranchCode = branchCode
            };

            await sidcAPILogsService.SendPostAsync(logsEntityPostRequest, tokenForLogsService);
        }

        #endregion
    }
}

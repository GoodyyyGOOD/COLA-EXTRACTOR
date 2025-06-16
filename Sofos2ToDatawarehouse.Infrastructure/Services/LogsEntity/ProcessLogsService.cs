using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Accounting.CancelChargeAmount.BulkUpSert;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Accounting.ChargeAmount.BulkUpSert;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Inventory.Items.BulkUpSert;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.LogsEntityPost;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.CancelTransaction.BulkUpSert;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaStub.BulkUpSert;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaTransaction.BulkUpSert;
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
                TransactionType = "ACCOUNTING",
                ServiceType = "ACCOUNTING SERVICE",
                FileName = fileName,
                FirstIdLedger = chargeAmountBulkUpsertRequest.CreateChargeAmountCommand.Min(o => o.TransNum),
                LastIdLedger = chargeAmountBulkUpsertRequest.CreateChargeAmountCommand.Max(o => o.TransNum),
                TransactionCount = chargeAmountBulkUpsertRequest.CreateChargeAmountCommand.Count(),
                BranchCode = branchCode
            };

            await sidcAPILogsService.SendPostAsync(logsEntityPostRequest, tokenForLogsService);
        }

        #endregion

        #region Inventory

        public async Task ItemsLogsServiceRequestAsync(ItemsBulkUpsertRequest itemsBulkUpsertRequest, SIDCAPILogsService sidcAPILogsService, string fileName, string branchCode)
        {
            string tokenForLogsService = await Task.Run(() => sidcAPILogsService.SendAuthenticationAsync());

            LogsEntityPostRequest logsEntityPostRequest = new LogsEntityPostRequest()
            {
                TransactionType = "INVENTORY",
                ServiceType = "INVENTORY SERVICE",
                FileName = fileName,
                //FirstIdLedger = itemsBulkUpsertRequest.CreateItemsCommand.Min(o => o.TransNum),
                //LastIdLedger = itemsBulkUpsertRequest.CreateItemsCommand.Max(o => o.TransNum),
                TransactionCount = itemsBulkUpsertRequest.CreateItemsCommand.Count(),
                BranchCode = branchCode
            };

            await sidcAPILogsService.SendPostAsync(logsEntityPostRequest, tokenForLogsService);
        }

        #endregion
        #region ColaStub
        public async Task ColaStubLogsServiceRequestAsync(ColaStubBulkUpsertRequest colaStubBulkUpsertRequest, SIDCAPILogsService sidcAPILogsService, string fileName, string branchCode)
        {
            string tokenForLogsService = await Task.Run(() => sidcAPILogsService.SendAuthenticationAsync());

            LogsEntityPostRequest logsEntityPostRequest = new LogsEntityPostRequest()
            {
                TransactionType = "SALES",
                ServiceType = "SALES SERVICE",
                FileName = fileName,
                FirstIdLedger = colaStubBulkUpsertRequest.CreateColaStubCommand.Min(o => o.Transnum),
                LastIdLedger = colaStubBulkUpsertRequest.CreateColaStubCommand.Max(o => o.Transnum),
                TransactionCount = colaStubBulkUpsertRequest.CreateColaStubCommand.Count(),
                BranchCode = branchCode
            };

            await sidcAPILogsService.SendPostAsync(logsEntityPostRequest, tokenForLogsService);
        }

        #endregion ColaStub
        #region CancelTransaction
        public async Task CancelTransactionLogsServiceRequestAsync(CancelTransactionBulkUpsertRequest cancelTransactionBulkUpsertRequest, SIDCAPILogsService sidcAPILogsService, string fileName, string branchCode)
        {
            string tokenForLogsService = await Task.Run(() => sidcAPILogsService.SendAuthenticationAsync());

            LogsEntityPostRequest logsEntityPostRequest = new LogsEntityPostRequest()
            {
                TransactionType = "SALES",
                ServiceType = "SALES SERVICE",
                FileName = fileName,
                FirstIdLedger = cancelTransactionBulkUpsertRequest.CreateCancelTransactionCommand.Min(o => o.Transnum),
                LastIdLedger = cancelTransactionBulkUpsertRequest.CreateCancelTransactionCommand.Max(o => o.Transnum),
                TransactionCount = cancelTransactionBulkUpsertRequest.CreateCancelTransactionCommand.Count(),
                BranchCode = branchCode
            };

            await sidcAPILogsService.SendPostAsync(logsEntityPostRequest, tokenForLogsService);
        }

        #endregion CancelTransaction
        #region Cancel ChargeAmount

        public async Task CancelChargeAmountLogsServiceRequestAsync(CancelChargeAmountBulkUpsertRequest cancelChargeAmountBulkUpsertRequest, SIDCAPILogsService sidcAPILogsService, string fileName, string branchCode)
        {
            string tokenForLogsService = await Task.Run(() => sidcAPILogsService.SendAuthenticationAsync());

            LogsEntityPostRequest logsEntityPostRequest = new LogsEntityPostRequest()
            {
                TransactionType = "ACCOUNTING",
                ServiceType = "ACCOUNTING SERVICE",
                FileName = fileName,
                FirstIdLedger = cancelChargeAmountBulkUpsertRequest.CreateCancelChargeAmountCommand.Min(o => o.TransNum),
                LastIdLedger = cancelChargeAmountBulkUpsertRequest.CreateCancelChargeAmountCommand.Max(o => o.TransNum),
                TransactionCount = cancelChargeAmountBulkUpsertRequest.CreateCancelChargeAmountCommand.Count(),
                BranchCode = branchCode
            };

            await sidcAPILogsService.SendPostAsync(logsEntityPostRequest, tokenForLogsService);
        }

        #endregion


    }
}

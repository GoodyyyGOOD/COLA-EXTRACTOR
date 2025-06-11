using Newtonsoft.Json;
using Sofos2ToDatawarehouse.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.CancelTransaction.BulkUpSert;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.CancelTransaction.Create;

namespace Sofos2ToDatawarehouse.Infrastructure.Services.CancelTransaction
{
    public class CancelTransactionService : SIDCAPICancelTransactionService
    {
        //private string ChargeAmountBulkUpSert = "v1/accounting/transactions/AccountingTransaction/bulk-update-insert";

        public CancelTransactionService(SIDCAPIServiceSettings sidcAPIServiceSettings)
        {
            _sidcAPIServiceSettings = sidcAPIServiceSettings;
        }

        //public async Task<ChargeAmountBulkUpsertResponse> SendBulkUpSertAsync(ChargeAmountBulkUpsertRequest chargeAmountBulkUpsertRequest)
        //{
        //    var chargeAmountBulkUpsertResponse = new ChargeAmountBulkUpsertResponse();

        //    try
        //    {
        //        webAddr = string.Format("{0}{1}", _sidcAPIServiceSettings.InventoryBaseUrl, ChargeAmountBulkUpSert);

        //        WebRequest request = WebRequest.Create(webAddr);

        //        request.Headers.Add("Authorization", "Bearer " + token);
        //        request.Method = "POST";
        //        string postData = JsonConvert.SerializeObject(chargeAmountBulkUpsertRequest);

        //        byte[] byteArray = Encoding.UTF8.GetBytes(postData);

        //        request.ContentType = "application/json";

        //        request.ContentLength = byteArray.Length;

        //        Stream dataStream = request.GetRequestStream();

        //        dataStream.Write(byteArray, 0, byteArray.Length);

        //        dataStream.Close();

        //        WebResponse response = request.GetResponse();

        //        using (dataStream = response.GetResponseStream())
        //        {
        //            StreamReader reader = new StreamReader(dataStream);

        //            string responseFromServer = await reader.ReadToEndAsync();

        //            chargeAmountBulkUpsertResponse = JsonConvert.DeserializeObject<ChargeAmountBulkUpsertResponse>(responseFromServer);
        //        }
        //        response.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    return chargeAmountBulkUpsertResponse;
        //}
        public async Task<CancelTransactionBulkUpsertResponse> PostCancelTransactionAsync(CancelTransactionBulkUpsertRequest cancelTransactionBulkUpsertRequest)
        {
            var CancelTransactionBulkUpsertResponse = new CancelTransactionBulkUpsertResponse();

            try
            {
                // Construct the URL
                webAddr = string.Format("{0}{1}", _sidcAPIServiceSettings.BaseUrl, _sidcAPIServiceSettings.AccountingBaseUrl);

                // Create the request
                WebRequest request = WebRequest.Create(webAddr);
                request.Method = "POST";
                request.ContentType = "application/json";

                // Serialize the request body
                string postData = JsonConvert.SerializeObject(cancelTransactionBulkUpsertRequest);
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = byteArray.Length;

                // Write the data to the request stream
                using (Stream dataStream = request.GetRequestStream())
                {
                    await dataStream.WriteAsync(byteArray, 0, byteArray.Length);
                }

                // Get the response
                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string responseFromServer = await reader.ReadToEndAsync();
                            CancelTransactionBulkUpsertResponse = JsonConvert.DeserializeObject<CancelTransactionBulkUpsertResponse>(responseFromServer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to POST Cancel Transaction: {ex.Message}");
            }

            return CancelTransactionBulkUpsertResponse;
        }

        #region Deserialize

        public List<CreateCancelTransactionCommand> DeserializeObjectToCancelTransactionBulkUpSertRequest(string jsonString)
        {
            return JsonConvert.DeserializeObject<List<CreateCancelTransactionCommand>>(jsonString);
        }

        #endregion Deserialize

        public async Task MoveFileToTransferredAsync(FileInfo extractedFile, string fileNameToTransfer)
        {
            if (File.Exists(fileNameToTransfer))
            {
                File.Delete(fileNameToTransfer);
            }

            await Task.Run(() => extractedFile.MoveTo(fileNameToTransfer));
        }
    }
}

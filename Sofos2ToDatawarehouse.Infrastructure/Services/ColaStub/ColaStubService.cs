using Newtonsoft.Json;
using Sofos2ToDatawarehouse.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaStub.BulkUpSert;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaStub.Create;

namespace Sofos2ToDatawarehouse.Infrastructure.Services.ColaStub
{
    public class ColaStubService : SIDCAPIColaStubService
    {
        //private string ChargeAmountBulkUpSert = "v1/accounting/transactions/AccountingTransaction/bulk-update-insert";

        public ColaStubService(SIDCAPIServiceSettings sidcAPIServiceSettings)
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
        public async Task<ColaStubBulkUpsertResponse> PostColaStubAsync(ColaStubBulkUpsertRequest colaStubBulkUpsertRequest)
        {
            var colaStubBulkUpsertResponse = new ColaStubBulkUpsertResponse();

            try
            {
                // Construct the URL
                webAddr = string.Format("{0}{1}", _sidcAPIServiceSettings.BaseUrl, _sidcAPIServiceSettings.AccountingBaseUrl);

                // Create the request
                WebRequest request = WebRequest.Create(webAddr);
                request.Method = "POST";
                request.ContentType = "application/json";

                // Serialize the request body
                string postData = JsonConvert.SerializeObject(colaStubBulkUpsertRequest);
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
                            colaStubBulkUpsertResponse = JsonConvert.DeserializeObject<ColaStubBulkUpsertResponse>(responseFromServer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to POST ColaStub: {ex.Message}");
            }

            return colaStubBulkUpsertResponse;
        }

        #region Deserialize

        public List<CreateColaStubCommand> DeserializeObjectToColaStubBulkUpSertRequest(string jsonString)
        {
            return JsonConvert.DeserializeObject<List<CreateColaStubCommand>>(jsonString);
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

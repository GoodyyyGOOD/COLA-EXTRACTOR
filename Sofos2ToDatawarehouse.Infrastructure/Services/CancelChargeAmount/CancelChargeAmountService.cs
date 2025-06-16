using Newtonsoft.Json;
using Sofos2ToDatawarehouse.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Accounting.CancelChargeAmount.BulkUpSert;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Accounting.CancelChargeAmount.Create;

namespace Sofos2ToDatawarehouse.Infrastructure.Services.CancelChargeAmount
{
    public class CancelChargeAmountService : SIDCAPICancelChargeAmountService
    {
        //private string CancelChargeAmountBulkUpSert = "v1/CancelChargeAmount/transactions/CancelChargeAmountTransaction/bulk-update-insert";

        public CancelChargeAmountService(SIDCAPIServiceSettings sidcAPIServiceSettings)
        {
            _sidcAPIServiceSettings = sidcAPIServiceSettings;
        }

        //public async Task<CancelChargeAmountBulkUpsertResponse> SendBulkUpSertAsync(CancelChargeAmountBulkUpsertRequest CancelChargeAmountBulkUpsertRequest)
        //{
        //    var CancelChargeAmountBulkUpsertResponse = new CancelChargeAmountBulkUpsertResponse();

        //    try
        //    {
        //        webAddr = string.Format("{0}{1}", _sidcAPIServiceSettings.InventoryBaseUrl, CancelChargeAmountBulkUpSert);

        //        WebRequest request = WebRequest.Create(webAddr);

        //        request.Headers.Add("Authorization", "Bearer " + token);
        //        request.Method = "POST";
        //        string postData = JsonConvert.SerializeObject(CancelChargeAmountBulkUpsertRequest);

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

        //            CancelChargeAmountBulkUpsertResponse = JsonConvert.DeserializeObject<CancelChargeAmountBulkUpsertResponse>(responseFromServer);
        //        }
        //        response.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    return CancelChargeAmountBulkUpsertResponse;
        //}
        public async Task<CancelChargeAmountBulkUpsertResponse> PostCancelChargeAmountAsync(CancelChargeAmountBulkUpsertRequest CancelChargeAmountBulkUpsertRequest)
        {
            var CancelChargeAmountBulkUpsertResponse = new CancelChargeAmountBulkUpsertResponse();

            try
            {
                // Construct the URL
                webAddr = string.Format("{0}{1}", _sidcAPIServiceSettings.BaseUrl, _sidcAPIServiceSettings.CancelChargeAmountBaseUrl);

                // Create the request
                WebRequest request = WebRequest.Create(webAddr);
                request.Method = "POST";
                request.ContentType = "application/json";

                // Serialize the request body
                string postData = JsonConvert.SerializeObject(CancelChargeAmountBulkUpsertRequest);
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
                            CancelChargeAmountBulkUpsertResponse = JsonConvert.DeserializeObject<CancelChargeAmountBulkUpsertResponse>(responseFromServer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to POST CancelChargeAmount: {ex.Message}");
            }

            return CancelChargeAmountBulkUpsertResponse;
        }

        #region Deserialize

        public List<CreateCancelChargeAmountCommand> DeserializeObjectToCancelChargeAmountBulkUpSertRequest(string jsonString)
        {
            return JsonConvert.DeserializeObject<List<CreateCancelChargeAmountCommand>>(jsonString);
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

using Newtonsoft.Json;
using Sofos2ToDatawarehouse.Domain.DTOs;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Accounting.ChargeAmount.BulkUpSert;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaTransaction;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaTransaction.BulkUpSert;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaTransaction.Create;
using Sofos2ToDatawarehouse.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Sofos2ToDatawarehouse.Infrastructure.Queries.Sales.ColaTransactionQuery;

namespace Sofos2ToDatawarehouse.Infrastructure.Services.Sales
{
    public class ColaTransactionService : SIDCAPISaleService
    {
        private string SalesTransactionBulkUpSert = "v1/sales/transactions/SalesTransaction/bulk-update-insert";

        public ColaTransactionService(SIDCAPIServiceSettings sidcAPIServiceSettings)
        {
            _sidcAPIServiceSettings = sidcAPIServiceSettings;
        }

        public async Task<ColaTransactionBulkUpsertResponse> SendBulkUpSertAsync(ColaTransactionBulkUpsertRequest salesTransactionBulkUpsertRequest, string token)
        {
            var colaTransactionBulkUpsertResponse = new ColaTransactionBulkUpsertResponse();

            try
            {
                webAddr = string.Format("{0}{1}", _sidcAPIServiceSettings.SalesBaseUrl, SalesTransactionBulkUpSert);

                WebRequest request = WebRequest.Create(webAddr);

                var apitoken = _sidcAPIServiceSettings.APIToken;
                request.Headers.Add("Authorization", "Bearer " + apitoken);
                request.Method = "POST";
                string postData = JsonConvert.SerializeObject(salesTransactionBulkUpsertRequest);

                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                request.ContentType = "application/json";

                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();

                dataStream.Write(byteArray, 0, byteArray.Length);

                dataStream.Close();

                WebResponse response = request.GetResponse();

                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);

                    string responseFromServer = await reader.ReadToEndAsync();

                    colaTransactionBulkUpsertResponse = JsonConvert.DeserializeObject<ColaTransactionBulkUpsertResponse>(responseFromServer);
                }
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return colaTransactionBulkUpsertResponse;
        }

        public async Task<ColaTransactionBulkUpsertResponse> PostColaTransactionAsync(ColaTransactionBulkUpsertRequest colaTransactionBulkUpsertRequest)
        {
            var colaTransactionBulkUpsertResponse = new ColaTransactionBulkUpsertResponse();

            try
            {
                // Construct the URL
                webAddr = string.Format("{0}{1}", _sidcAPIServiceSettings.BaseUrl, _sidcAPIServiceSettings.SalesBaseUrl);

                // Create the request
                WebRequest request = WebRequest.Create(webAddr);
                var apitoken = _sidcAPIServiceSettings.APIToken;
                request.Headers.Add("Authorization", "Bearer " + apitoken);
                request.Method = "POST";
                request.ContentType = "application/json";

                // Serialize the request body
                string postData = JsonConvert.SerializeObject(colaTransactionBulkUpsertRequest);
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
                            colaTransactionBulkUpsertResponse = JsonConvert.DeserializeObject<ColaTransactionBulkUpsertResponse>(responseFromServer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to POST Cola Transaction: {ex.Message}");
            }
            
            return colaTransactionBulkUpsertResponse;
        }

        public async Task UpdateIsInsertedBatchAsync(List<(ColaTransactionEnum Type, string ReferenceNumber)> updates)
        {
            if (updates == null || updates.Count == 0)
                return;

            var commandList = new List<(string Query, Dictionary<string, object> Parameters)>();

            foreach (var (type, referenceNumber) in updates)
            {
                string query = UpdateColaQuery(type).ToString();
                var param = new Dictionary<string, object>();

                if (type == ColaTransactionEnum.UpdateColaHeader)
                    param.Add("@transNum", referenceNumber);
                else if (type == ColaTransactionEnum.UpdateColaDetail)
                    param.Add("@detailNum", referenceNumber);

                commandList.Add((query, param));
            }

            //using (var context = new ApplicationContext(_dbSource))
            //{
            //    try
            //    {
            //        await context.ExecuteTransactionAsync(commandList); // You’ll need to implement this if it doesn't exist
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Batch update failed: {ex.Message}");
            //        // Optional: log or rethrow
            //    }
            //}
        }


        #region Deserialize

        public List<CreateColaTransactionCommand> DeserializeObjectToColaTransactionBulkUpSertRequest(string jsonString)
        {
            return JsonConvert.DeserializeObject<List<CreateColaTransactionCommand>>(jsonString);
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

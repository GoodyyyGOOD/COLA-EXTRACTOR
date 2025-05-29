using Newtonsoft.Json;
using Sofos2ToDatawarehouse.Domain.DTOs;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaTransactions;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaTransactions.BulkUpSert;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaTransactions.Create;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Infrastructure.Services.Sales
{
    public class SalesTransactionService : SIDCAPISaleService
    {
        private string SalesTransactionBulkUpSert = "v1/sales/transactions/SalesTransaction/bulk-update-insert";

        public SalesTransactionService(SIDCAPIServiceSettings sidcAPIServiceSettings)
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

                request.Headers.Add("Authorization", "Bearer " + token);
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

        #region Deserialize

        public List<CreateColaTransactionCommand> DeserializeObjectToSalesTransactionBulkUpSertRequest(string jsonString)
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

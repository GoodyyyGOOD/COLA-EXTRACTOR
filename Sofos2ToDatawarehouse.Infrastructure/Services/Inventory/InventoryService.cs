using Newtonsoft.Json;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaTransactions.BulkUpSert;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Sales.ColaTransactions.Create;
using Sofos2ToDatawarehouse.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.Inventory.Items.BulkUpSert;

namespace Sofos2ToDatawarehouse.Infrastructure.Services.Inventory
{
    public class InventoryService : SIDCAPIInventoryService
    {
        private string ItemsBulkUpSert = "v1/inventory/transactions/InventoryTransaction/bulk-update-insert";

        public InventoryService(SIDCAPIServiceSettings sidcAPIServiceSettings)
        {
            _sidcAPIServiceSettings = sidcAPIServiceSettings;
        }

        public async Task<ItemsBulkUpsertResponse> SendBulkUpSertAsync(ItemsBulkUpsertRequest itemsBulkUpsertRequest, string token)
        {
            var itemsBulkUpsertResponse = new ItemsBulkUpsertResponse();

            try
            {
                webAddr = string.Format("{0}{1}", _sidcAPIServiceSettings.InventoryBaseUrl, ItemsBulkUpSert);

                WebRequest request = WebRequest.Create(webAddr);

                request.Headers.Add("Authorization", "Bearer " + token);
                request.Method = "POST";
                string postData = JsonConvert.SerializeObject(itemsBulkUpsertRequest);

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

                    itemsBulkUpsertResponse = JsonConvert.DeserializeObject<ItemsBulkUpsertResponse>(responseFromServer);
                }
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return itemsBulkUpsertResponse;
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

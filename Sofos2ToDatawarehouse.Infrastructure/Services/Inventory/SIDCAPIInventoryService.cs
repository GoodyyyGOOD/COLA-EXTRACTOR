using Newtonsoft.Json;
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCToken;
using Sofos2ToDatawarehouse.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sofos2ToDatawarehouse.Infrastructure.Services.Inventory
{
    public class SIDCAPIInventoryService
    {
        public string webAddr = null;
        public SIDCAPIServiceSettings _sidcAPIServiceSettings { get; set; }

        public async Task<string> SendAuthenticationAsync()
        {
            SIDCTokenRequest sidcTokenRequest = new SIDCTokenRequest()
            {
                Username = _sidcAPIServiceSettings.AuthUser,
                Password = _sidcAPIServiceSettings.AuthPassword,
            };

            string token = string.Empty;

            try
            {
                webAddr = string.Format("{0}{1}", _sidcAPIServiceSettings.IdentityUrl, _sidcAPIServiceSettings.AuthTokenUrl);

                WebRequest request = WebRequest.Create(webAddr);

                token = _sidcAPIServiceSettings.APIToken;

                request.Method = "POST";
                string postData = JsonConvert.SerializeObject(sidcTokenRequest);

                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                request.ContentType = "application/json";

                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();

                dataStream.Write(byteArray, 0, byteArray.Length);

                dataStream.Close();

                WebResponse response = request.GetResponse();

                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);

                    string responseFromServer = await reader.ReadToEndAsync();

                    var sidcTokenResponse = JsonConvert.DeserializeObject<SIDCTokenResponse>(responseFromServer);

                    token = sidcTokenResponse.Data.JwToken.ToString();
                }
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return token;
        }
    }
}

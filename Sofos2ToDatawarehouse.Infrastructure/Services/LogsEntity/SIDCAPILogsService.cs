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
using Sofos2ToDatawarehouse.Domain.DTOs.SIDCAPI_s.LogsEntityPost;

namespace Sofos2ToDatawarehouse.Infrastructure.Services.LogsEntity
{
    public class SIDCAPILogsService
    {
        public string webAddr = null;
        public SIDCLogsServiceApiSettings _sidcLogsServiceApiSettings { get; set; }
        private string LogsEntityPostUrl = "v1/LogsEntity";

        public SIDCAPILogsService(SIDCLogsServiceApiSettings sidcLogsServiceApiSettings)
        {
            _sidcLogsServiceApiSettings = sidcLogsServiceApiSettings;
        }

        public async Task<string> SendAuthenticationAsync()
        {
            SIDCTokenRequest sidcTokenRequest = new SIDCTokenRequest()
            {
                Username = _sidcLogsServiceApiSettings.AuthUser,
                Password = _sidcLogsServiceApiSettings.AuthPassword,
            };

            string token = string.Empty;

            try
            {
                webAddr = string.Format("{0}{1}", _sidcLogsServiceApiSettings.IdentityUrl, _sidcLogsServiceApiSettings.AuthTokenUrl);

                WebRequest request = WebRequest.Create(webAddr);

                request.Method = "POST";
                string postData = JsonConvert.SerializeObject(sidcTokenRequest);

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

        public async Task<LogsEntityPostResponse> SendPostAsync(LogsEntityPostRequest logsEntityPostRequest, string token)
        {
            var logsEntityPostResponse = new LogsEntityPostResponse();

            try
            {
                webAddr = string.Format("{0}{1}", _sidcLogsServiceApiSettings.BaseUrl, LogsEntityPostUrl);

                WebRequest request = WebRequest.Create(webAddr);

                request.Headers.Add("Authorization", "Bearer " + token);
                request.Method = "POST";
                string postData = JsonConvert.SerializeObject(logsEntityPostRequest);

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

                    logsEntityPostResponse = JsonConvert.DeserializeObject<LogsEntityPostResponse>(responseFromServer);
                }
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return logsEntityPostResponse;
        }

    }
}

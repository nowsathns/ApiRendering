using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using ApiRendering.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ApiRendering.Services
{
    public interface IDLService
    {
        DLAPIResponse GetDLInformation(string number, string dob);
    }

    public class DLService : IDLService
    {
        private string DLURL
        {
            get
            {
                return configuration.GetValue<string>("Api:DLURL");
            }
        }

        private string ApiKey
        {
            get
            {
                return configuration.GetValue<string>("Api:Key");
            }
        }

        private readonly IConfiguration configuration;

        public DLService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public DLAPIResponse GetDLInformation(string number, string dob)
        {
            DLResponse response = null;
            try
            {
                string apiResponse = GetAPIResponse(number, dob);
                response =
                    JsonConvert.DeserializeObject<DLResponse>(apiResponse);
                return response.response;
            }
            finally
            {
                response = null;
            }
        }

        private string GetAPIResponse(string cardnumber, string dob)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-api-key", ApiKey);

            var formContent =
                new FormUrlEncodedContent(new []
                    {
                        new KeyValuePair<string, string>("cardNumber",
                            cardnumber),
                        new KeyValuePair<string, string>("dob", dob)
                    });

            //ApiURL + "dl/shortDetail"
            HttpResponseMessage response =
                client.PostAsync(DLURL, formContent).GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();
            string responseBody =
                response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return responseBody;
        }
    }
}

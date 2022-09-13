using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using ApiRendering.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ApiRendering.Services
{
    public interface IAadhaarService {
        AadhaarInfo GetAadhaarInformation(string number);
     }

    public class AadhaarService : IAadhaarService
    {
        private string AadhaarURL
        {
            get
            {
                return configuration.GetValue<string>("Api:AadhaarURL");
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


        public AadhaarService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public AadhaarInfo GetAadhaarInformation(string number)
        {
            AadhaarResponse response = null;
            string apiResponse;
            try
            {
                apiResponse = GetAPIResponse(number);
                
                response =
                    JsonConvert.DeserializeObject<AadhaarResponse>(apiResponse);
                return response.response;
            }
            finally
            {
                response = null;
            }
        }

        private string GetAPIResponse(string number)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-api-key", ApiKey);

            var formContent =
                new FormUrlEncodedContent(new []
                    {
                        new KeyValuePair<string, string>("aadhaarNumber",
                            number),
                        new KeyValuePair<string, string>("demographicCheck",
                            true.ToString())
                    });

            //ApiURL + "dl/shortDetail"
            HttpResponseMessage response =
                client.PostAsync(AadhaarURL, formContent).GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();
            string responseBody =
                response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return responseBody;
        }
    }
}

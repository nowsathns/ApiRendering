using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using ApiRendering.Data;
using ApiRendering.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ApiRendering.Services
{
    public interface IVehicleService
    {
        RegistrationResponse GetVehicleInformation(string number);
    }

    public class VehicleService : IVehicleService
    {
        private string VehicleURL
        {
            get
            {
                return configuration.GetValue<string>("Api:VehicleURL");
            }
        }
        private string VehicleSearchURL
        {
            get
            {
                return configuration.GetValue<string>("Api:VehicleSearchURL");
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

        private APIDBContext _dbContext;

        public VehicleService(
            IConfiguration configuration,
            APIDBContext dbContext
        )
        {
            this.configuration = configuration;
            this._dbContext = dbContext;
        }

        public RegistrationResponse GetVehicleInformation(string number)
        {
            ApiResponse response = null;
            VehicleDetails details = null;
            string apiResponse;
            try
            {
                //API or Database Logic Start
               // details =
               //     _dbContext
               //         .VehicleDetails
               //         .FirstOrDefault(exp => exp.VehicleNo == number);
               //if (details == null)
                    
                //{
                    //From API
                    apiResponse = GetAPIResponse(number);
                //if (!string.IsNullOrEmpty(apiResponse)) { apiResponse = apiResponse.Replace("/","");  }
                //    _dbContext
                //        .VehicleDetails
                //        .Add(new VehicleDetails()
                //        { ResponseJSON = apiResponse, VehicleNo = number });
                //    this._dbContext.SaveChanges();
                //}
                //else
                //{
                //    //From Database
                //    apiResponse = details.ResponseJSON;
                //}
                response =
                    JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
                response.response.RegNo = number;
                return response.response;
            }
            //API or Database Logic End
            finally
            {
                response = null;
            }
        }

        private string GetAPIResponse(string number)
        {
            HttpResponseMessage response = null;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-api-key", ApiKey);

            if (string.IsNullOrEmpty(VehicleSearchURL))
            {
                var formContent =
                    new FormUrlEncodedContent(new[]
                        {
                        new KeyValuePair<string, string>("vehicleNumber",
                            number)
                        });

                //ApiURL + "vehicle-registration/basic"
                response =
                    client
                        .PostAsync(VehicleURL, formContent)
                        .GetAwaiter()
                        .GetResult();
            }
            else
            {
                string url = VehicleSearchURL + "?vehicleNumber=" + number;
                response =
                    client
                        .GetAsync(url)
                        .GetAwaiter()
                        .GetResult();
                
            }
            response.EnsureSuccessStatusCode();


            string responseBody =
                response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return responseBody;
        }
    }
}

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
                details =
                    _dbContext
                        .VehicleDetails
                        .FirstOrDefault(exp => exp.VehicleNo == number);
                if (details == null)
                {
                    apiResponse = GetAPIResponse(number);
                    _dbContext
                        .VehicleDetails
                        .Add(new VehicleDetails()
                        { ResponseJSON = apiResponse, VehicleNo = number });
                    this._dbContext.SaveChanges();
                }
                else
                {
                    apiResponse = details.ResponseJSON;
                }
                response =
                    JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
                response.response.RegNo = number;
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
                        new KeyValuePair<string, string>("vehicleNumber",
                            number)
                    });

            //ApiURL + "vehicle-registration/basic"
            HttpResponseMessage response =
                client
                    .PostAsync(VehicleURL, formContent)
                    .GetAwaiter()
                    .GetResult();
            response.EnsureSuccessStatusCode();
            string responseBody =
                response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return responseBody;
        }
    }
}

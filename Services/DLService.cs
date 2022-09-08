using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using ApiRendering.Data;
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

        private APIDBContext _dbContext;

        public DLService(IConfiguration configuration, APIDBContext dbContext)
        {
            this.configuration = configuration;
            this._dbContext = dbContext;
        }

        public DLAPIResponse GetDLInformation(string number, string dob)
        {
            DLResponse response = null;
            LicenseDetails details = null;
            string apiResponse;
            try
            {
                DateTime date =
                    DateTime
                        .ParseExact(dob,
                        "dd/MM/yyyy",
                        CultureInfo.InvariantCulture);

                details =
                    _dbContext
                        .LicenseDetails
                        .FirstOrDefault(exp =>
                            exp.LicenseNo == number && exp.DOB == date);
                if(details  == null)
                {
                    apiResponse = GetAPIResponse(number, dob);
                    _dbContext.LicenseDetails.Add(new LicenseDetails(){ DOB = date, ResponseJSON = apiResponse, LicenseNo = number });
                    _dbContext.SaveChanges();
                }
                else{ apiResponse = details.ResponseJSON; }

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

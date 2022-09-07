using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ApiRendering.Models;
using ApiRendering.Services;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace ApiRendering.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IConfiguration configuration;
        private IDLService dlService;
        private IVehicleService vehicleService;
    
        public HomeController(ILogger<HomeController> logger, IConfiguration config, IDLService dlService, IVehicleService vehicleService)
        {
            _logger = logger;
            this.configuration = config;
            this.dlService = dlService;
            this.vehicleService = vehicleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult VehicleSearch(APIInput input)
        {
            try
            {
                var response = this.vehicleService.GetVehicleInformation(input.vehicleNumber);
                return View(response);
            }
            catch(Exception e)
            {
                this._logger.LogError(e.InnerException == null ? "" : e.InnerException.ToString());
            }
            return View();
        }

        public IActionResult DLView()
        {
            return View();
        }

        public IActionResult DLSearch(APIInput input)
        {
            try
            {
                var response = this.dlService.GetDLInformation(input.cardNumber,input.dob);
                return View(response);
            }
            catch(Exception e)
            {
                this._logger.LogError(e.InnerException == null ? "" : e.InnerException.ToString());
            }
            return View();
        }

        // [HttpPost]
        // public ActionResult VehicleSearch(APIInput input)
        // {
        //     ApiResponse response = null;
        //     try
        //     {
        //         string apiResponse = GetAPIResponse(input.vehicleNumber);
        //         response = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
        //         response.response.RegNo = input.vehicleNumber;
        //         return View(response.response);
        //     }
        //     catch (Exception e)
        //     {
        //         ViewBag.ErrorMessage = "Server encounted an error. Please try after sometime.";

        //     }
        //     return View();
        // }
        // private string GetAPIResponse(string number)
        // {
        //     ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //     HttpClient client = new HttpClient();
        //     client.DefaultRequestHeaders.Add("x-api-key", ApiKey);

        //     var formContent = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("vehicleNumber", number), });

        //     //ApiURL + "vehicle-registration/basic"
        //     HttpResponseMessage response = client.PostAsync(VehicleURL, formContent).GetAwaiter().GetResult();
        //     response.EnsureSuccessStatusCode();
        //     string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        //     return responseBody;
        // }
        // [HttpPost]
        // public ActionResult DLSearch(APIInput input)
        // {
        //     DLResponse response = null;
        //     try
        //     {
        //         if(input == null) { return View(); }
        //         string apiResponse = GetDLAPIResponse(input.cardNumber,input.dob);
        //         response = JsonConvert.DeserializeObject<DLResponse>(apiResponse);
        //         return View(response.response);
        //     }
        //     catch (Exception e)
        //     {
        //         ViewBag.ErrorMessage = "Server encounted an error. Please try after sometime.";
        //         this._logger.LogError(e.InnerException == null ? "" : e.InnerException.ToString());
        //     }
        //     return View();
        // }
        // private string GetDLAPIResponse(string cardnumber, string dob)
        // {
        //     ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //     HttpClient client = new HttpClient();
        //     client.DefaultRequestHeaders.Add("x-api-key", ApiKey);

        //     var formContent = new FormUrlEncodedContent(new[] 
        //     { new KeyValuePair<string, string>("cardNumber", cardnumber), 
        //         new KeyValuePair<string, string>("dob", dob) });

        //     //ApiURL + "dl/shortDetail"
        //     HttpResponseMessage response = client.PostAsync(DLURL, formContent).GetAwaiter().GetResult();
        //     response.EnsureSuccessStatusCode();
        //     string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        //     return responseBody;
        // }
    }
}

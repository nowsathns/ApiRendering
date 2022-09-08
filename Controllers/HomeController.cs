using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ApiRendering.Models;
using ApiRendering.Services;
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
    }
}

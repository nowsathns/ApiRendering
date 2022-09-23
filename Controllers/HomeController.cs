using System;
using System.Diagnostics;
using ApiRendering.Models;
using ApiRendering.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ApiRendering.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private IConfiguration configuration;

        private IDLService dlService;

        private IVehicleService vehicleService;

        private IAadhaarService aadhaarService;

        public HomeController(
            ILogger<HomeController> logger,
            IConfiguration config,
            IDLService dlService,
            IVehicleService vehicleService,
            IAadhaarService aadhaarService
        )
        {
            _logger = logger;
            this.configuration = config;
            this.dlService = dlService;
            this.vehicleService = vehicleService;
            this.aadhaarService = aadhaarService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [
            ResponseCache(
                Duration = 0,
                Location = ResponseCacheLocation.None,
                NoStore = true)
        ]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

        public IActionResult VehicleSearch(APIInput input)
        {
            try
            {
                var response =
                    this
                        .vehicleService
                        .GetVehicleInformation(input.vehicleNumber);
                return View(response);
            }
            catch (Exception e)
            {
                this
                    ._logger
                    .LogError(e.InnerException == null
                        ? ""
                        : e.InnerException.ToString());
                ViewBag.Error = "Invalid Vehicle Number";
                return View();
            }
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
            catch (Exception e)
            {
                this
                    ._logger
                    .LogError(e.InnerException == null
                        ? ""
                        : e.InnerException.ToString());
                ViewBag.Error = "Invalid License Number";

                //TempData["Data"] = "Invalid";
                return View();
            }
            //return View();
        }

        public IActionResult AadharView()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AadharSearch(AadhaarInfo aadharInfo)
        {
            try
            {
                var response =
                    aadhaarService
                        .GetAadhaarInformation(aadharInfo.aadharNumber);
                return View(response);
            }
            catch (Exception e)
            {
                this._logger .LogError(e.InnerException == null ? "" : e.InnerException.ToString());
                ViewBag.Error = "Invalid Aadhar Number";
                return View();
            }
        }
    }
}

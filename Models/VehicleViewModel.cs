using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRendering.Models
{
    public class Status
    {
        public int statusCode { get; set; }

        public string statusMessage { get; set; }
    }

    public class HypotecationDetail
    {
        public string financier { get; set; }
    }

    public class Insurance
    {
        public string policyNumber { get; set; }

        public string company { get; set; }

        public string validTill { get; set; }
    }

    public class Pucc
    {
        public string number { get; set; }

        public string upto { get; set; }
    }

    public class RegistrationDetail
    {
        public string blacklistStatus { get; set; }

        public string rcStatusAsOn { get; set; }

        public string authority { get; set; }

        public string date { get; set; }

        public string expiryDate { get; set; }

        public string number { get; set; }

        public string nocDetails { get; set; }

        public string taxUpto { get; set; }
    }

    public class RegistrationResponse
    {
        public string owner { get; set; }

        public string ownerNumber { get; set; }

        public string ownerFathersName { get; set; }

        public Address presentAddress { get; set; }

        public Address permanentAddress { get; set; }

        public RegistrationDetail registrationDetail { get; set; }

        public HypotecationDetail hypotecationDetail { get; set; }

        public Vehicle vehicle { get; set; }

        public Insurance insurance { get; set; }

        public Pucc pucc { get; set; }

        public string RegNo { get; set; }
    }

    public class Address
    {
        public string completeAddress { get; set; }

        public string addressLine { get; set; }

        public List<string> city { get; set; }

        public List<string> district { get; set; }

        public List<string> country { get; set; }

        public string pincode { get; set; }
    }

    public class Vehicle
    {
        public string number { get; set; }

        public string fuelType { get; set; }

        public string normsDesc { get; set; }

        public string vehicleMMV { get; set; }

        public string companyName { get; set; }

        public string modelName { get; set; }

        public string category { get; set; }

        public string color { get; set; }

        public string chassis { get; set; }

        public string @class { get; set; }

        public string engine { get; set; }

        public string manufacturingDate { get; set; }

        public string cubicCapacity { get; set; }

        public string grossWeight { get; set; }

        public string unladenWeight { get; set; }

        public string noCyl { get; set; }

        public string seatCap { get; set; }

        public string wheelBase { get; set; }
    }

    public class ApiResponse
    {
        public Status status { get; set; }

        public RegistrationResponse response { get; set; }
    }

    public class APIInput
    {
        [Required]
        public string vehicleNumber { get; set; }
        
        public string cardNumber { get; set; }

        public string dob { get; set; }

        public ApiResponse Response { get; set; }
    }
}

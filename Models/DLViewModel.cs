using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRendering.Models
{
    public class CovDetail
    {
        [JsonProperty("COV Category")]
        public string COVCategory { get; set; }

        [JsonProperty("COV Issue Date")]
        public string COVIssueDate { get; set; }

        [JsonProperty("Class Of Vehicle")]
        public string ClassOfVehicle { get; set; }
    }

    public class Details
    {
        public string status { get; set; }
        public string name { get; set; }
        public List<CovDetail> covDetails { get; set; }
    }

    public class NonTransport
    {
        public string From { get; set; }
        public string To { get; set; }
    }

    public class DLAPIResponse
    {
        public int code { get; set; }
        public Validity validity { get; set; }
        public Details details { get; set; }
    }

    public class DLResponse
    {
        public Status status { get; set; }
        public DLAPIResponse response { get; set; }
    }

    public class Transport
    {
        public string From { get; set; }
        public string To { get; set; }
    }

    public class Validity
    {
        [JsonProperty("Non-Transport")]
        public NonTransport NonTransport { get; set; }
        public Transport Transport { get; set; }
    }

}

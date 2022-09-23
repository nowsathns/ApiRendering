using System.ComponentModel.DataAnnotations;

namespace ApiRendering.Models
{
    public class AadhaarResponse 
    { 
        public AadhaarInfo response { get; set; }
        public Status status { get; set; }
    }

    public class AadhaarInfo
    {
       
        public string verified { get; set; }

        public string ageBand { get; set; }

        public string gender { get; set; }

        public string mobileNumber { get; set; }

        public string state { get; set; }
        [Required(ErrorMessage = "Enter Valid Aadhar Number")]
        public string aadharNumber { get; set; }
    }
}

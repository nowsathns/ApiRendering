using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiRendering.Data
{
 
    [Table("LicenseDetails")]
    public class LicenseDetails
    { 
        [Key]
        public long Id { get; set; }
        [Column(TypeName ="VARCHAR(50)")]
        public string LicenseNo { get; set; }
        public DateTime DOB { get; set; }

        [Column(TypeName ="NVarchar(MAX)")]
        public string ResponseJSON { get; set; }
    }

    [Table("VehicleDetails")]
    public class VehicleDetails
    {
        [Key]
        public long Id { get; set; }
        [Column(TypeName ="VARCHAR(50)")]
        public string VehicleNo { get; set; }

        [Column(TypeName ="NVarchar(MAX)")]
        public string ResponseJSON { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;

namespace ApiRendering.Data
{
    public class APIDBContext : DbContext
    {
        public APIDBContext(DbContextOptions<APIDBContext> options) : base (options)
        {
            
        }  
        public DbSet<VehicleDetails> VehicleDetails { get; set; } 
        public DbSet<LicenseDetails> LicenseDetails { get; set; }
    }

}
#pragma warning restore format
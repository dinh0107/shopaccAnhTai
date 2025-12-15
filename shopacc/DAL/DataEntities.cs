using shopacc.Models;
using System.Data.Entity;

namespace shopacc.DAL
{
    public class DataEntities : DbContext
    {
        public DataEntities() : base("name=DataEntities")
        {
        }

        public DbSet<Admin  > Admins { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<ConfigSite> ConfigSites { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Table> Tables { get; set; }
    }
}

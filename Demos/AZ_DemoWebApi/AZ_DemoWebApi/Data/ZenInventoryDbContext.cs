using Microsoft.EntityFrameworkCore;
using AZ_DemoWebApi.Models;

namespace AZ_DemoWebApi.Data
{
    public class ZenInventoryDbContext : DbContext
    {
        public ZenInventoryDbContext(DbContextOptions<ZenInventoryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}

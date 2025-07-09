using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TradingApp;

namespace TradingApp.BlazorUI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TradeEntry> TradeEntries { get; set; } //DbSet<TradeEntry> maps to a table called TradeEntries
        public DbSet<Company> Companies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductQuality> ProductQualities { get; set; }
        public DbSet<DeliveryInfo> DeliveryInfos { get; set; }
    }
}

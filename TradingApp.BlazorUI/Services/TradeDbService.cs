using Microsoft.EntityFrameworkCore;
using TradingApp;
using TradingApp.BlazorUI.Data;

namespace TradingApp.BlazorUI.Services
{
    public class TradeDbService
    {
        private readonly ApplicationDbContext _db;

        public TradeDbService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<TradeEntry>> GetAllTradesAsync()
        {
            return await _db.TradeEntries
                .Include(t => t.Company)
                .Include(t => t.Product).ThenInclude(p => p.ProductQuality)
                .Include(t => t.DeliveryInfo)
                .ToListAsync();
        }

        public async Task AddTradeAsync(TradeEntry trade)
        {
            _db.TradeEntries.Add(trade);
            await _db.SaveChangesAsync();
        }
    }
}


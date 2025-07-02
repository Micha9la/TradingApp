using System.Diagnostics;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using TradingApp.BlazorUI.Data;

namespace TradingApp.BlazorUI.Services
{
    public class EFTradeService : ITradeService
    {
        private readonly ApplicationDbContext _context;
        //ApplicationDbContext is injected into the service, to query the database.
        public EFTradeService(ApplicationDbContext context)
        {
            _context = context;
        }
        //The GetAllTradesAsync() method loads all trade entries and their related data(Company, Product, etc.).
        public async Task<List<TradeEntry>> GetAllTradesAsync()
        {
            return await _context.TradeEntries
                .Include(t => t.Company)
                .Include(t => t.Product)
                    .ThenInclude(p => p.ProductQuality)
                .Include(t => t.DeliveryInfo)
                .ToListAsync();
        }
        //The AddTradeAsync() method adds a new trade and saves it to the DB.
        public async Task AddTradeAsync(TradeEntry entry)
        {
            _context.TradeEntries.Add(entry);
            await _context.SaveChangesAsync();
        }
    }
}


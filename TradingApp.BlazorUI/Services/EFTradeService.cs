using System.Diagnostics;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using TradingApp.BlazorUI.Data;

namespace TradingApp.BlazorUI.Services
{
    public class EFTradeService : ITradeService
    {
        private readonly ApplicationDbContext _context;
        //ApplicationDbContext is injected into the service, to query/access the database in EFCore.
        public EFTradeService(ApplicationDbContext context)
        {
            _context = context;
        }
        //The GetAllTradesAsync() method loads all entry entries and their related data(Company, Product, etc.).
        public async Task<List<TradeEntry>> GetAllTradesAsync()
        {
            return await _context.TradeEntries
                .Include(t => t.Company)
                .Include(t => t.Product)
                    .ThenInclude(p => p.ProductQuality)
                .Include(t => t.DeliveryInfo)
                .ToListAsync();
        }
        //The AddTradeAsync() method adds a new entry and saves it to the DB.
        public async Task AppendTradeAsync(TradeEntry entry)
        {
            _context.TradeEntries.Add(entry);
            await _context.SaveChangesAsync();
        }
    }
}


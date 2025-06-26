using TradingApp;
using TradingApp.BlazorUI.Services;

namespace TradingApp.BlazorUI.Services
{
    public class InMemoryTradeService
    {
    }
}
public class InMemoryTradeService : ITradeService
{
    private readonly List<TradeEntry> _trades = new();
    private int _nextCatalogNumber = 1;

    public Task<List<TradeEntry>> GetAllTradesAsync()
    {
        return Task.FromResult(_trades);
    }

    public Task AddTradeAsync(TradeEntry trade)
    {
        trade.CatalogNumber = _nextCatalogNumber++;
        _trades.Add(trade);
        return Task.CompletedTask;
    }
}


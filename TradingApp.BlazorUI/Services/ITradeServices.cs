using TradingApp;

namespace TradingApp.BlazorUI.Services

{
    public interface ITradeService
    {
        Task<List<TradeEntry>> GetAllTradesAsync();
        Task AddTradeAsync(TradeEntry trade);
    }
}


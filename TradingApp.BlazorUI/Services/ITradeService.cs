using TradingApp;

namespace TradingApp.BlazorUI.Services

{
    public interface ITradeService
    {
        Task<List<TradeEntry>> GetAllTradesAsync();
        Task AddTradeAsync(TradeEntry trade);

        //Task<List<string>> GetAllCompanyNamesAsync();
        //Task<List<string>> GetAllContactPersonsAsync();   
    }

    public class TradeService : ITradeService
    {
        private readonly GoogleSheetTradeSyncService _googleSheetService;

        public TradeService(GoogleSheetTradeSyncService googleSheetService)
        {
            _googleSheetService = googleSheetService;
        }

        public async Task<List<TradeEntry>> GetAllTradesAsync()
        {
            // Already implemented somewhere
            return await _googleSheetService.ReadTradesAsync();
        }

        public async Task AddTradeAsync(TradeEntry trade)
        {
            await _googleSheetService.AddTradeAsync(trade);
        }

        public async Task<List<string>> GetAllCompanyNamesAsync()
        {
            var trades = await GetAllTradesAsync();
            return trades
                .Where(t => !string.IsNullOrWhiteSpace(t.Company?.CompanyName))
                .Select(t => t.Company.CompanyName!.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(name => name)
                .ToList();
        }

        public async Task<List<string>> GetAllContactPersonsAsync()
        {
            var trades = await GetAllTradesAsync();
            return trades
                .Where(t => !string.IsNullOrWhiteSpace(t.Company?.ContactPerson))
                .Select(t => t.Company.ContactPerson!.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(name => name)
                .ToList();
        }
    }

}


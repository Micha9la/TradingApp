using Google.Apis.Sheets.v4.Data;
using TradingApp;
using static System.Net.Mime.MediaTypeNames;


namespace TradingApp.BlazorUI.Services

{
    public interface ITradeService
    {
        Task<List<TradeEntry>> GetAllTradesAsync();
        Task AppendTradeAsync(TradeEntry trade);
        Task<int> GetNextCatalogNumberAsync();
        //Task<List<string>> GetAllCompanyNamesAsync();
        //Task<List<string>> GetAllContactPersonsAsync();
        //Task<TradeEntry?> GetTradeByIdAsync(int id) — for detail view.
        //Task UpdateTradeAsync(TradeEntry trade) — for corrections.
        //Task DeleteTradeAsync(int id) — for cancellations.
        //Possibly search/filter methods(GetTradesByCompanyAsync, etc.).
        //adding update/delete and get by id will make sense soon...
    }

    //public class TradeService : ITradeService
    //{
    //    private readonly GSTradeService _googleSheetService;

    //    public TradeService(GSTradeService googleSheetService)
    //    {
    //        _googleSheetService = googleSheetService;
    //    }

    //    public async Task<List<TradeEntry>> GetAllTradesAsync()
    //    {
    //        // Already implemented somewhere
    //        return await _googleSheetService.GetAllTradesAsync();
    //    }
    //    public async Task AppendTradeAsync(TradeEntry trade)
    //    {
    //        await _googleSheetService.AppendTradeAsync(trade);
    //    }
    //    public async Task<List<string>> GetAllCompanyNamesAsync()
    //    {
    //        var trades = await GetAllTradesAsync();
    //        return trades
    //            .Where(t => !string.IsNullOrWhiteSpace(t.Company?.CompanyName))
    //            .Select(t => t.Company.CompanyName!.Trim())
    //            .Distinct(StringComparer.OrdinalIgnoreCase)
    //            .OrderBy(name => name)
    //            .ToList();
    //    }

    //    public async Task<List<string>> GetAllContactPersonsAsync()
    //    {
    //        var trades = await GetAllTradesAsync();
    //        return trades
    //            .Where(t => !string.IsNullOrWhiteSpace(t.Company?.ContactPerson))
    //            .Select(t => t.Company.ContactPerson!.Trim())
    //            .Distinct(StringComparer.OrdinalIgnoreCase)
    //            .OrderBy(name => name)
    //            .ToList();
 //       }
  //  }

}


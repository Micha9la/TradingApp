using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Humanizer;
using TradingApp.BlazorUI.Components;

namespace TradingApp.BlazorUI.Services
{
    public class GoogleSheetTradeSyncService
    {
        //fields
        private readonly SheetsService _sheetsService;
        private readonly string _spreadsheetId;
        private readonly string _sheetName;

        //Constructor: Load service account credentials.
        //Initialize the Google Sheets API client(SheetsService)
        //Store spreadsheetId and sheetName from appsettings.json
        public GoogleSheetTradeSyncService(IConfiguration configuration)
        {
            // Load credentials and create Google Sheets API service
            // 1. Read the JSON file from disk
            GoogleCredential credential;
            using (var stream = new FileStream(configuration["GoogleSheets:ServiceAccountKeyPath"], FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(SheetsService.Scope.Spreadsheets);
            }
            // 2. Initialize Google Sheets API client
            _sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "TradingApp"
            });
            // 3. Read spreadsheet ID and sheet name from config
            _spreadsheetId = configuration["GoogleSheets:SpreadsheetId"];
            _sheetName = configuration["GoogleSheets:SheetName"];
        }
        //Converts a TradeEntry to a Google Sheet row (List<string>)
        //Sends it to Google Sheets to append after the last row
        public async Task AppendTradeAsync(TradeEntry trade)
        {
            var row = MapTradeToRow(trade); //map object to row of strings

            var valueRange = new ValueRange 
            { 
                Values = new List<IList<object>> 
                { 
                    row.Cast<object>().ToList() 
                } 
            };

            var appendRequest = _sheetsService.Spreadsheets.Values.Append(valueRange, _spreadsheetId, $"{_sheetName}!A1");
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;

            await appendRequest.ExecuteAsync();
        }
        //Read (Fetch rows from the sheet and convert to C#)
        public async Task<List<TradeEntry>> ReadTradesAsync()
        {
            var request = _sheetsService.Spreadsheets.Values.Get(_spreadsheetId, $"{_sheetName}!A2:Z");
            var response = await request.ExecuteAsync();

            var trades = new List<TradeEntry>();
            foreach (var row in response.Values)
            {
                trades.Add(MapRowToTrade(row));
            }

            return trades;
        }
        //all column headers inside
        private List<string> MapTradeToRow(TradeEntry trade)
        {
            return new List<string>
        {
            trade.CatalogNumber.ToString(),
            trade.Date.ToString("yyyy-MM-dd"),
            trade.TradeDirection.ToString(),
            trade.Product?.ProductName ?? "",
            trade.Company?.CompanyName ?? "",
            trade.Company?.ContactPerson ?? "",
            trade.Product?.ProductQuality?.Protein.ToString(),
            trade.Product?.ProductQuality?.TestWeight.ToString(),
            trade.Product?.ProductQuality?.FallingNumber.ToString(),
            trade.Product?.Quantity.ToString(),
            trade.DeliveryInfo?.DeliveryParity.ToString(),
            trade.DeliveryInfo?.LocationDetail ?? "",
            trade.Price.ToString(),
            trade.Currency,
            trade.GMP.ToString(),
            trade.ISCC.ToString(),
            trade.Records ?? "",
            trade.PrivateNotes ?? ""
        };
        }

        private TradeEntry MapRowToTrade(IList<object> row)
        {
            // handle missing values gracefully here.
            // Cast values from row[x] back into TradeEntry.
            // error handling and validation needed?
        }
    }

}


using System.Diagnostics;
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
        //Sends it to Google Sheets to append after the last row.
        //it will append a new row to the sheet after the last row
        public async Task AppendTradeAsync(TradeEntry trade)
        {
            var row = MapTradeToRow(trade); //map object TradeEntry to row of strings. 
            //Converting it to the format the Sheets API expects
            var valueRange = new ValueRange 
            { 
                Values = new List<IList<object>> 
                { 
                    row.Cast<object>().ToList() 
                } 
            };
            //Creating the append request and executing it
            var appendRequest = _sheetsService.Spreadsheets.Values.Append(valueRange, _spreadsheetId, $"{_sheetName}!A1");
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;

            await appendRequest.ExecuteAsync();
        }
        //Read (Fetch rows from the sheet and convert to C#)
        public async Task<List<TradeEntry>> ReadTradesAsync()
        {
            Console.WriteLine("GoogleSheetService: Reading trades...");
            var request = _sheetsService.Spreadsheets.Values.Get(_spreadsheetId, $"{_sheetName}!A2:Z");
            var response = await request.ExecuteAsync();
            var trades = new List<TradeEntry>();

            if (response.Values != null)
            {
                foreach (var row in response.Values)
                {
                    var trade = MapRowToTrade(row); 
                    if (trade != null)
                        trades.Add(trade);
                }
            }

            Console.WriteLine($"GoogleSheetService: Found {trades.Count} trades.");

            return trades;
        }
        //all column headers inside. to export data to csv file
        private List<string> MapTradeToRow(TradeEntry trade)
        {
            return new List<string>
            {
            trade.CatalogNumber.ToString(),
            trade.Date.ToString("yyyy-MM-dd"),
            trade.TradeDirection.ToString(),

            trade.Company?.CompanyName ?? "",
            trade.Company?.ContactPerson ?? "",

            trade.Product?.ProductName ?? "",
            trade.Product?.Quantity.ToString(),
            trade.Product?.ProductQuality?.Protein.ToString(),
            trade.Product?.ProductQuality?.TestWeight.ToString(),
            trade.Product?.ProductQuality?.FallingNumber.ToString(),
            trade.Product?.ProductQuality?.Glassiness,
            trade.Product?.ProductQuality?.OilContent,
            trade.Product?.ProductQuality?.DamagedKernels,
            trade.Product?.ProductQuality?.Don,
            trade.Product?.ProductQuality?.Afla,

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
        // handle missing values gracefully.
        // Cast values from row[x] back into TradeEntry.
        // error handling and validation needed?
        private TradeEntry MapRowToTrade(IList<object> row)
        {
            if (row == null || row.Count < 23)
                return null;

            try
            {
                int i = 0;
                
                return new TradeEntry
                {
                    CatalogNumber = int.TryParse(CleanCellValue(row[i++])?.ToString(), out var catalog) ? catalog : 0,
                    Date = DateTime.TryParse(CleanCellValue(row[i++])?.ToString(), out var date) ? date : DateTime.MinValue,
                    TradeDirection = Enum.TryParse<TradeDirectionType>(CleanCellValue(row[i++])?.ToString(), true, out var direction) ? direction : TradeDirectionType.Offer,

                    Company = new Company
                    {
                        CompanyName = CleanCellValue(row[i++])?.ToString() ?? string.Empty,
                        ContactPerson = CleanCellValue(row[i++])?.ToString() ?? string.Empty
                    },

                    Product = new Product
                    {
                        ProductName = CleanCellValue(row[i++])?.ToString() ?? string.Empty,
                        Quantity = float.TryParse(CleanCellValue(row[i++])?.ToString(), out var quantity) ? quantity : 0,

                        ProductQuality = new ProductQuality
                        {
                            Protein = float.TryParse(CleanCellValue(row[i++])?.ToString(), out var protein) ? protein : 0,
                            TestWeight = int.TryParse(CleanCellValue(row[i++])?.ToString(), out var testWeight) ? testWeight : 0,
                            FallingNumber = int.TryParse(CleanCellValue(row[i++])?.ToString(), out var falling) ? falling : 0,
                            Glassiness = int.TryParse(CleanCellValue(row[i++])?.ToString(), out var glassiness) ? glassiness : 0,
                            OilContent = int.TryParse(CleanCellValue(row[i++])?.ToString(), out var oil) ? oil : 0,
                            DamagedKernels = int.TryParse(CleanCellValue(row[i++])?.ToString(), out var damaged) ? damaged : 0,
                            Don = int.TryParse(CleanCellValue(row[i++])?.ToString(), out var don) ? don : 0,
                            Afla = int.TryParse(CleanCellValue(row[i++])?.ToString(), out var afla) ? afla : 0
                        }
                    },

                    DeliveryInfo = new DeliveryInfo
                    {
                        DeliveryParity = Enum.TryParse<ParityType>(CleanCellValue(row[i++])?.ToString(), true, out var parity) ? parity : ParityType.FCA,
                        LocationDetail = CleanCellValue(row[i++])?.ToString() ?? string.Empty
                    },

                    Price = decimal.TryParse(CleanCellValue(row[i++])?.ToString(), out var price) ? price : 0,
                    Currency = CleanCellValue(row[i++])?.ToString() ?? "EUR",
                    GMP = Enum.TryParse<GMP>(CleanCellValue(row[i++])?.ToString(), true, out var gmp) ? gmp : GMP.NonGMP,
                    ISCC = Enum.TryParse<ISCC>(CleanCellValue(row[i++])?.ToString(), true, out var iscc) ? iscc : ISCC.NonISCC,
                    Records = CleanCellValue(row[i++])?.ToString() ?? string.Empty,
                    PrivateNotes = CleanCellValue(row[i++])?.ToString() ?? string.Empty
                };
            }
            catch (Exception error)
            {
                Console.WriteLine($"Failed to map row to TradeEntry. Row: {string.Join(", ", row)}");
                Console.WriteLine($"Error: {error.Message}");
                return null;
            }
        }
        
        private string CleanCellValue(object cell)
        {
            var value = cell?.ToString()?.Trim();

            if (string.IsNullOrWhiteSpace(value) || value == "-" || value == "/" || value.ToLowerInvariant() == "n/a")
                return null;

            return value;
        }
        
    }

}


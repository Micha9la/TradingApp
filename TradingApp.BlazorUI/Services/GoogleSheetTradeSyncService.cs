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
            var request = _sheetsService.Spreadsheets.Values.Get(_spreadsheetId, $"{_sheetName}!A2:Z");
            var response = await request.ExecuteAsync();

            var trades = new List<TradeEntry>();
            foreach (var row in response.Values)
            {
                trades.Add(MapRowToTrade(row));
            }

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
        // handle missing values gracefully.
        // Cast values from row[x] back into TradeEntry.
        // error handling and validation needed?
        private TradeEntry MapRowToTrade(IList<object> row)
        {
            // Basic null/length check. row-- inside the row, we access it by column index row[1]
            if (row == null || row.Count < 23) //A row(cell) with exactly 23 columns is valid.Even though the indices go from 0–22, Count is the total number of elements, not the highest index
                return null;

            try
            {
                return new TradeEntry
                {
                    CatalogNumber = int.TryParse(row[0]?.ToString(), out var catalog) ? catalog : 0,
                    Date = DateTime.TryParse(row[1]?.ToString(), out var date) ? date : DateTime.MinValue,
                    TradeDirection = Enum.TryParse<TradeDirectionType>(row[2]?.ToString(), true, out var direction) ? direction : TradeDirectionType.Offer,

                    Company = new Company
                    {
                        CompanyName = row[4]?.ToString() ?? string.Empty,
                        ContactPerson = row[5]?.ToString() ?? string.Empty
                    },

                    Product = new Product
                    {
                        ProductName = row[3]?.ToString() ?? string.Empty,
                        Quantity = float.TryParse(row[6]?.ToString(), out var quantity) ? quantity : 0,
                        ProductQuality = new ProductQuality
                        {
                            Protein = float.TryParse(row[7]?.ToString(), out var protein) ? protein : 0,
                            TestWeight = int.TryParse(row[8]?.ToString(), out var weight) ? weight : 0,
                            FallingNumber = int.TryParse(row[9]?.ToString(), out var falling) ? falling : 0,
                            Glassiness = int.TryParse(row[10]?.ToString(), out var glassiness) ? glassiness : 0,
                            OilContent = int.TryParse(row[11]?.ToString(), out var oil) ? oil : 0,
                            DamagedKernels = int.TryParse(row[12]?.ToString(), out var damaged) ? damaged : 0,
                            Don = int.TryParse(row[13]?.ToString(), out var don) ? don : 0,
                            Afla = int.TryParse(row[14]?.ToString(), out var afla) ? afla : 0
                        }
                    },
                    
                    DeliveryInfo = new DeliveryInfo
                    {
                        DeliveryParity = Enum.TryParse<ParityType>(row[15]?.ToString(), true, out var parity) ? parity : ParityType.FCA,
                        LocationDetail = row[16]?.ToString() ?? string.Empty
                    },

                    Price = decimal.TryParse(row[17]?.ToString(), out var price) ? price : 0m, //0m is the default value for a decimal (m = money/literal decimal)
                    Currency = row[18]?.ToString() ?? "EUR",
                    GMP = Enum.TryParse<GMP>(row[19]?.ToString(), true, out var gmp) ? gmp : GMP.NonGMP,
                    ISCC = Enum.TryParse<ISCC>(row[20]?.ToString(), true, out var iscc) ? iscc : ISCC.NonISCC,

                    Records = row[21]?.ToString() ?? string.Empty,
                    PrivateNotes = row[22]?.ToString() ?? string.Empty
                };
            }
            //If any error occurs while creating the TradeEntry (e.g., bad formatting, missing fields), this catch will run.
            catch (Exception error)
            {
                Console.WriteLine($"Failed to map row to TradeEntry. Row content: {string.Join(", ", row)}");
                Console.WriteLine($"Error: {error.Message}");
                return null; // skip this row and continue
            }
        }
    }

}


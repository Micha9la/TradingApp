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
            {// TryParse with fallback to default values
                int.TryParse(row[0]?.ToString(), out var catalog);
                DateTime.TryParse(row[1]?.ToString(), out var date);
                Enum.TryParse<TradeDirectionType>(row[2]?.ToString(), true, out var direction);

                float.TryParse(row[7]?.ToString(), out var protein);
                int.TryParse(row[8]?.ToString(), out var testWeight);
                int.TryParse(row[9]?.ToString(), out var falling);
                int.TryParse(row.ElementAtOrDefault(10)?.ToString(), out var glassiness);
                int.TryParse(row.ElementAtOrDefault(11)?.ToString(), out var oilContent);
                int.TryParse(row.ElementAtOrDefault(12)?.ToString(), out var damagedKernels);
                int.TryParse(row.ElementAtOrDefault(13)?.ToString(), out var don);
                int.TryParse(row.ElementAtOrDefault(14)?.ToString(), out var afla);
                float.TryParse(row.ElementAtOrDefault(6)?.ToString(), out var quantity);
                decimal.TryParse(row.ElementAtOrDefault(17)?.ToString(), out var price);

                Enum.TryParse<ParityType>(row.ElementAtOrDefault(15)?.ToString(), true, out var parity);
                Enum.TryParse<GMP>(row.ElementAtOrDefault(19)?.ToString(), true, out var gmp);
                Enum.TryParse<ISCC>(row.ElementAtOrDefault(20)?.ToString(), true, out var iscc);

                return new TradeEntry
                {
                    CatalogNumber = catalog,
                    Date = date == DateTime.MinValue ? DateTime.Now : date,
                    TradeDirection = direction,

                    Company = new Company
                    {
                        CompanyName = row[4]?.ToString() ?? string.Empty,
                        ContactPerson = row[5]?.ToString() ?? string.Empty
                    },

                    Product = new Product
                    {
                        ProductName = row.ElementAtOrDefault(3)?.ToString() ?? string.Empty,
                        Quantity = quantity,
                        ProductQuality = new ProductQuality
                        {
                            Protein = protein,
                            TestWeight = testWeight,
                            FallingNumber = falling,
                            Glassiness = glassiness,
                            OilContent = oilContent,
                            DamagedKernels = damagedKernels,
                            Don = don,
                            Afla = afla
                        }
                    },
                    
                    DeliveryInfo = new DeliveryInfo
                    {
                        DeliveryParity = parity,
                        LocationDetail = row.ElementAtOrDefault(16)?.ToString() ?? ""
                    },

                    Price = price,
                    Currency = row.ElementAtOrDefault(19)?.ToString() ?? "EUR",
                    GMP = gmp,
                    ISCC = iscc,
                    Records = row.ElementAtOrDefault(22)?.ToString() ?? "",
                    PrivateNotes = row.ElementAtOrDefault(23)?.ToString() ?? ""
                };
            }
            //If any error occurs while creating the TradeEntry (e.g., bad formatting, missing fields), this catch will run.
            catch (Exception error)
            {
                Console.WriteLine($"Failed to map row to TradeEntry. Row: {string.Join(", ", row)}");
                Console.WriteLine($"Error: {error.Message}");
                return null;
            }
        }
    }

}


using System.Diagnostics;
using System.Text;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Humanizer;
using TradingApp.BlazorUI.Components;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TradingApp.BlazorUI.Services
{
    public class GSTradeService : ITradeService
    {
        //fields
        private readonly SheetsService _sheetsService;
        private readonly string _spreadsheetId;
        private readonly string _sheetName;

        //Constructor: Load service account credentials.
        //Initialize the Google Sheets API client(SheetsService)
        //Store spreadsheetId and sheetName from appsettings.json
        public GSTradeService(IConfiguration configuration)
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
        public async Task<List<TradeEntry>> GetAllTradesAsync()
        {
            Console.WriteLine("GoogleSheetService: Reading trades...");

            // Request the ENTIRE sheet (all rows, all columns)
            var request = _sheetsService.Spreadsheets.Values.Get(_spreadsheetId, _sheetName);
            var response = await request.ExecuteAsync();

            var trades = new List<TradeEntry>();

            if (response.Values != null && response.Values.Count > 1) // must be > 1 because row 0 = header
            {
                int rowIndex = 2; // start at row 2 in Google Sheets
                foreach (var row in response.Values.Skip(1)) // Skip the header row
                {
                    Console.WriteLine($"Row {rowIndex}: {string.Join(" | ", row)}");
                    rowIndex++;

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
            trade.Product?.ProductQuality?.Glassiness.ToString(),
            trade.Product?.ProductQuality?.OilContent.ToString(),
            trade.Product?.ProductQuality?.DamagedKernels.ToString(),
            trade.Product?.ProductQuality?.Don.ToString(),
            trade.Product?.ProductQuality?.Afla.ToString(),

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
            // If row is null or completely empty -> skip
            if (row == null)
                return null;

            // local helper: safely get cleaned string or null (handles out-of-range)
            //Each row is just a List<object> (one row from Google Sheets). Each element in row is a cell
            //idx → the column index (0 = column A catalog number, 1 = column B, etc.).
            static string GetCell(IList<object> r, int idx, Func<object, string> cleaner)
            {
                if (r == null || idx < 0 || idx >= r.Count)
                    return null;
                return cleaner(r[idx]);
            }

            try
            {
                int i = 0;

                // Read cells safely (CleanCellValue is your existing function)
                //defaulting to 0 or empty string to get a usable TradeEntry object.
                //Without it, it would crash or return null for every incomplete row
                var catalogStr = GetCell(row, i++, CleanCellValue);
                var dateStr = GetCell(row, i++, CleanCellValue);
                var directionStr = GetCell(row, i++, CleanCellValue);

                var companyName = GetCell(row, i++, CleanCellValue) ?? string.Empty;
                var contactPerson = GetCell(row, i++, CleanCellValue) ?? string.Empty;

                var productName = GetCell(row, i++, CleanCellValue) ?? string.Empty;
                var quantityStr = GetCell(row, i++, CleanCellValue);

                var proteinStr = GetCell(row, i++, CleanCellValue);
                var testWeightStr = GetCell(row, i++, CleanCellValue);
                var fallingNumberStr = GetCell(row, i++, CleanCellValue);
                var glassinessStr = GetCell(row, i++, CleanCellValue);
                var oilContentStr = GetCell(row, i++, CleanCellValue);
                var damagedKernelsStr = GetCell(row, i++, CleanCellValue);
                var donStr = GetCell(row, i++, CleanCellValue);
                var aflaStr = GetCell(row, i++, CleanCellValue);

                var parityStr = GetCell(row, i++, CleanCellValue);
                var locationDetail = GetCell(row, i++, CleanCellValue) ?? string.Empty;

                var priceStr = GetCell(row, i++, CleanCellValue);
                var currencyStr = GetCell(row, i++, CleanCellValue) ?? "EUR";

                var gmpStr = GetCell(row, i++, CleanCellValue);
                var isccStr = GetCell(row, i++, CleanCellValue);

                var recordsStr = GetCell(row, i++, CleanCellValue) ?? string.Empty;
                var privateNotesStr = GetCell(row, i++, CleanCellValue) ?? string.Empty;

                // Parse with fallbacks
                var catalogNumber = int.TryParse(catalogStr, out var catalog) ? catalog : 0;
                var date = DateTime.TryParse(dateStr, out var parsedDate) ? parsedDate : DateTime.MinValue;
                var tradeDirection = Enum.TryParse<TradeDirectionType>(directionStr, true, out var dir) ? dir : TradeDirectionType.Offer;

                var quantity = int.TryParse(quantityStr, out var qty) ? qty : 0;

                var protein = float.TryParse(proteinStr, out var proteinVal) ? proteinVal : 0f;
                var testWeight = int.TryParse(testWeightStr, out var testW) ? testW : 0;
                var fallingNumber = int.TryParse(fallingNumberStr, out var fall) ? fall : 0;
                var glassiness = int.TryParse(glassinessStr, out var g) ? g : 0;
                var oilContent = int.TryParse(oilContentStr, out var oc) ? oc : 0;
                var damagedKernels = int.TryParse(damagedKernelsStr, out var dk) ? dk : 0;
                var don = int.TryParse(donStr, out var d) ? d : 0;
                var afla = int.TryParse(aflaStr, out var a) ? a : 0;

                var deliveryParity = Enum.TryParse<ParityType>(parityStr, true, out var parity) ? parity : ParityType.FCA;

                var price = decimal.TryParse(priceStr, out var p) ? p : 0m;
                var gmp = Enum.TryParse<GMP>(gmpStr, true, out var gmpEnum) ? gmpEnum : GMP.NonGMP;
                var iscc = Enum.TryParse<ISCC>(isccStr, true, out var isccEnum) ? isccEnum : ISCC.NonISCC;

                // If the row is essentially empty (no catalog, no company, no product) -> skip it
                //= filter to decide if the row is basically “empty” or just junk from the sheet
                //focusing only on the main identifiers (Company, Contact, Product, Catalog)
                var isEmpty = catalogNumber == 0 &&
                              string.IsNullOrWhiteSpace(companyName) &&
                              string.IsNullOrWhiteSpace(productName) &&
                              string.IsNullOrWhiteSpace(contactPerson);

                if (isEmpty)
                {
                    // helpful debug: optional                    
                    // Console.WriteLine("Skipping empty row (all important cells blank).");
                    return null;
                }

                // Build and return object
                return new TradeEntry
                {
                    CatalogNumber = catalogNumber,
                    Date = date,
                    TradeDirection = tradeDirection,
                    Company = new Company
                    {
                        CompanyName = companyName,
                        ContactPerson = contactPerson
                    },
                    Product = new Product
                    {
                        ProductName = productName,
                        Quantity = quantity,
                        ProductQuality = new ProductQuality
                        {
                            Protein = protein,
                            TestWeight = testWeight,
                            FallingNumber = fallingNumber,
                            Glassiness = glassiness,
                            OilContent = oilContent,
                            DamagedKernels = damagedKernels,
                            Don = don,
                            Afla = afla
                        }
                    },
                    DeliveryInfo = new DeliveryInfo
                    {
                        DeliveryParity = deliveryParity,
                        LocationDetail = locationDetail
                    },
                    Price = price,
                    Currency = currencyStr,
                    GMP = gmp,
                    ISCC = iscc,
                    Records = recordsStr,
                    PrivateNotes = privateNotesStr
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to map row to TradeEntry. Raw row: {(row == null ? "NULL" : string.Join(" | ", row))}");
                Console.WriteLine($"Error: {ex}");
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

        public async Task<int> GetNextCatalogNumberAsync()
        {
            var trades = await GetAllTradesAsync();
            //Checks how many trades are being read and what the actual max number is.
            Console.WriteLine($"Total trades read: {trades.Count}");
            Console.WriteLine($"Max catalog number: {trades.Max(t => t.CatalogNumber)}");


            if (!trades.Any())
                return 1; // start at 1 if sheet is empty
                    
            return trades.Where(t => t.CatalogNumber > 0).Max(t => t.CatalogNumber) + 1;

        }

    }

}


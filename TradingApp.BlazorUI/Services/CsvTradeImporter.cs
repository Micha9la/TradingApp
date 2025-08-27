using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using TradingApp;
using TradingApp.BlazorUI.Data;

namespace TradingApp.BlazorUI.Services
{
    public class CsvTradeImporter
    {
        private readonly ApplicationDbContext _context;

        public CsvTradeImporter(ApplicationDbContext context)
        {
            _context = context;
        }

        //public async Task ImportAsync()
        //{
            // actual CSV import logic
        //}

        public async Task ImportTradesFromCsv(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
                IgnoreBlankLines = true,
                BadDataFound = null,
                TrimOptions = TrimOptions.Trim
            };

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);

            var records = csv.GetRecords<TradeCsvModel>().ToList();

            foreach (var record in records)
            {
                if (_context.TradeEntries.Any(t => t.CatalogNumber == record.CatalogNumber))
                    continue;

                Enum.TryParse<TradeDirectionType>(record.TradeDirection, out var tradeDirection);
                Enum.TryParse<ParityType>(record.DeliveryParity, out var parity);
                Enum.TryParse<GMP>(record.GMP, out var gmp);
                Enum.TryParse<ISCC>(record.ISCC, out var iscc);
                DateTime.TryParse(record.Date, out var parsedDate);

                var trade = new TradeEntry
                {                   
                    CatalogNumber = record.CatalogNumber ?? 0,
                    TradeDirection = tradeDirection,

                    Company = new Company { CompanyName = record.CompanyName },
                    Product = new Product
                    {
                        ProductName = record.ProductName,
                        Quantity = (int)(record.Quantity ?? 0),
                        ProductQuality = new ProductQuality
                        {
                            Protein = record.Protein ?? 0,
                            TestWeight = record.TestWeight ?? 0,
                            FallingNumber = record.FallingNumber ?? 0,
                            Glassiness = record.Glassiness ?? 0,
                            OilContent = record.OilContent ?? 0,
                            DamagedKernels = record.DamagedKernels ?? 0,
                            Don = record.Don ?? 0,
                            Afla = record.Afla ?? 0
                        }
                    },
                    DeliveryInfo = new DeliveryInfo
                    {
                        DeliveryParity = Enum.Parse<ParityType>(record.DeliveryParity),
                        LocationDetail = record.LocationDetail
                    },
                    Price = record.Price ?? 0,
                    Currency = record.Currency,
                    Date = parsedDate,
                    GMP = gmp,
                    ISCC = iscc,
                    Records = record.Records,
                    PrivateNotes = record.PrivateNotes
                };
                _context.TradeEntries.Add(trade);
            }
            await _context.SaveChangesAsync();
        }
    }
}


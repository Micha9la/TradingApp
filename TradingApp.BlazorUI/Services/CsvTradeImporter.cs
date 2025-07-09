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
                MissingFieldFound = null
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
                    CatalogNumber = record.CatalogNumber,
                    TradeDirection = tradeDirection,

                    Company = new Company { CompanyName = record.CompanyName },
                    Product = new Product
                    {
                        ProductName = record.ProductName,
                        Quantity = record.Quantity,
                        ProductQuality = new ProductQuality
                        {
                            Protein = record.Protein,
                            TestWeight = record.TestWeight,
                            FallingNumber = record.FallingNumber,
                            Glassiness = record.Glassiness,
                            OilContent = record.OilContent,
                            DamagedKernels = record.DamagedKernels,
                            Don = record.Don,
                            Afla = record.Afla
                        }
                    },
                    DeliveryInfo = new DeliveryInfo
                    {
                        DeliveryParity = Enum.Parse<ParityType>(record.DeliveryParity),
                        LocationDetail = record.LocationDetail
                    },
                    Price = record.Price,
                    Currency = record.Currency,
                    Date = parsedDate,
                    GMP = gmp,
                    ISCC = iscc,
                    PublicNotes = record.PublicNotes,
                    PrivateNotes = record.PrivateNotes
                };
                _context.TradeEntries.Add(trade);
            }
            await _context.SaveChangesAsync();
        }
    }
}


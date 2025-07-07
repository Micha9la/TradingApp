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

        public async Task ImportAsync()
        {
            // actual CSV import logic
        }

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
                var trade = new TradeEntry
                {                   
                    CatalogNumber = record.CatalogNumber,
                    TradeDirection = Enum.Parse<TradeDirectionType>(record.TradeDirection),
                    Company = new Company { CompanyName = record.CompanyName },
                    Product = new Product
                    {
                        ProductName = record.ProductName,
                        Quantity = record.Quantity,
                        ProductQuality = new ProductQuality
                        {
                            Protein = record.Protein,
                            TestWeight = record.TestWeight,
                            FallingNumber = record.FallingNumber
                        }
                    },
                    DeliveryInfo = new DeliveryInfo
                    {
                        DeliveryParity = Enum.Parse<ParityType>(record.Parity),
                        LocationDetail = record.LocationDetail
                    },
                    Price = record.Price,
                    Currency = record.Currency,
                    Date = DateTime.Parse(record.Date),
                    GMP = Enum.Parse<GMP>(record.GMP),
                    ISCC = Enum.Parse<ISCC>(record.ISCC),
                    PublicNotes = record.PublicNotes,
                    PrivateNotes = record.PrivateNotes
                };
                _context.TradeEntries.Add(trade);
            }
            await _context.SaveChangesAsync();
        }
    }
}


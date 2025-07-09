using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TradingApp
{
    public static class TestData
    {
        private static int _nextCatalogNumber = 1;

        public static TradeEntry CreateTradeEntry(TradeEntry trade)
        {
            trade.CatalogNumber = _nextCatalogNumber++;
            return trade;
        }
        public static List<TradeEntry> GetSampleTrades()
        {
            return new List<TradeEntry>
        {
            CreateTradeEntry(new TradeEntry
            {
                    TradeDirection = TradeDirectionType.Offer,
                    Company = new Company { CompanyName = "AgroLife" },
                    Product = new Product
                    {
                        ProductName = "Wheat",
                        ProductQuality = new ProductQuality
                        {
                            Protein = 13,
                            TestWeight = 78,
                            FallingNumber = 250,
                        },
                        Quantity = 1000,
                    },
                    DeliveryInfo = new DeliveryInfo
                    {
                        DeliveryParity = ParityType.FCA,
                        LocationDetail = "Nitra+25km"
                    },
                    Price = 200,
                    Currency = "Euro/t",
                    Date = new DateTime(2025, 8, 21),
                    GMP = GMP.NonGMP,
                    ISCC = ISCC.NonISCC,
                    Records = "crop 26, prepayment, moldy smell...",
                    PrivateNotes = "Oponice",

            })
        };

            new TradeEntry
            {
                TradeDirection = TradeDirectionType.Demand,
                Company = new Company { CompanyName = "Agrolife" },
                Product = new Product
                {
                    ProductName = "Wheat",
                    ProductQuality = new ProductQuality
                    {
                        Protein = 13,
                        TestWeight = 78,
                        FallingNumber = 250,
                    },
                    Quantity = 1000,
                },
                DeliveryInfo = new DeliveryInfo
                {
                    DeliveryParity = ParityType.FCA,
                    LocationDetail = "West SK+20km"
                },
                Price = 200,
                Currency = "Euro/t",
                Date = new DateTime(2025, 8, 21),
                GMP = GMP.NonGMP,
                ISCC = ISCC.NonISCC,
                Records = "Crop 26, Buyers Call",
                PrivateNotes = "Comission Buyer 1Euro",
            };
        }
    }
}


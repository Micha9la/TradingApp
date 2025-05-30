using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingApp
{
    public static class TestData
    {
        public static List<TradeEntry> GetSampleTrades()
        {
            return new List<TradeEntry>
            {
                new TradeEntry
                {
                    TradeDirection = TradeDirectionType.Offer,
                    Company = new Company { CompanyName = "AgroLife" },
                    Product = new Product
                    {
                        ProductName = "Barley",
                        ProductQuality = new ProductQuality
                        {
                            Protein = 13f,
                            Weight = 1000,
                            FallingNumber = 1000,
                        }
                    },
                    DeliveryInfo = new DeliveryInfo
                    {
                        DeliveryParity = ParityType.FCA,
                        LocationDetail = "Cityname+20km"
                    },
                    Price = 200,
                    Currency = "Euro",
                    Date = new DateTime(2024, 10, 7),
                    Notes = "New Crop" 
                }
                };

                new TradeEntry
                {
                    TradeDirection = TradeDirectionType.Demand,
                    Company = new Company { CompanyName = "Agrolife" },                    
                    Product = new Product
                    {
                        ProductName = "Barley",
                        ProductQuality = new ProductQuality
                        {
                            Protein = 13f,
                            Weight = 1000,
                            FallingNumber = 1000,
                        }
                    },
                    DeliveryInfo = new DeliveryInfo
                    {
                        DeliveryParity = ParityType.FCA,
                        LocationDetail = "Cityname+20km"
                    },
                    Price = 200,
                    Currency = "Euro",
                    Date = new DateTime(7 - 10 / 24),
                    Notes = "New Crop"
                };
            }
        }
    }


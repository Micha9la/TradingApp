namespace TradingApp.BlazorUI.Services
{
    public class TradeCsvModel
    {
        public int CatalogNumber { get; set; }
        public string TradeDirection { get; set; }

        // Company info
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }

        // Product info
        public string ProductName { get; set; }
        public float Quantity { get; set; }

        // Delivery info
        public string DeliveryParity { get; set; }
        public string LocationDetail { get; set; }

        // Pricing
        public decimal Price { get; set; }
        public string Currency { get; set; }

        // Date. will be parsed to DateTime
        public string Date { get; set; } 

        // Optional labels or certifications
        public string GMP { get; set; }
        public string ISCC { get; set; }

        // Notes
        public string PublicNotes { get; set; }
        public string PrivateNotes { get; set; }

        // Product Quality – these map to ProductQuality entity
        public float Protein { get; set; }               // %
        public int FallingNumber { get; set; }           // seconds
        public int TestWeight { get; set; }              // kg/hl
        public int Glassiness { get; set; }              // %
        public int OilContent { get; set; }              // %
        public int DamagedKernels { get; set; }          // %
        public int Don { get; set; }                     // ppb
        public int Afla { get; set; }                    // ppb
    }
}



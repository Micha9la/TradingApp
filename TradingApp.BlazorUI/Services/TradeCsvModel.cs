namespace TradingApp.BlazorUI.Services
{
    public class TradeCsvModel
    {
        public int CatalogNumber { get; set; }
        public string TradeDirection { get; set; }
        public string CompanyName { get; set; }
        public string ProductName { get; set; }
        public float Protein { get; set; }
        public int TestWeight { get; set; }
        public int FallingNumber { get; set; }
        public float Quantity { get; set; }
        public string Parity { get; set; }
        public string LocationDetail { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public string Date { get; set; }
        public string GMP { get; set; }
        public string ISCC { get; set; }
        public string PublicNotes { get; set; }
        public string PrivateNotes { get; set; }
    }
}


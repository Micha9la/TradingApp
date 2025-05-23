namespace TradingApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var trades = TestData.GetSampleTrades();

            foreach (var trade in trades)
            {
                Console.WriteLine($"{trade.TradeDirection} - {trade.Product.ProductName} for {trade.Price} {trade.Currency} at {trade.DeliveryInfo.LocationDetail}");
            };
        }
    }
}
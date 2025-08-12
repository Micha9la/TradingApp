namespace TradingApp.BlazorUI.Data
{
    public static class DisplayExtensions
    {
        public static string ValueOrEmpty(this object value)
        {
            if (value == null) return string.Empty;
            if (value is string s) return string.IsNullOrWhiteSpace(s) ? string.Empty : s;
            if (value.Equals(0)) return string.Empty;
            if (value.Equals(0.0f) || value.Equals(0.0d) || value.Equals(0.0m)) return string.Empty;
            if (value.GetType().IsEnum && value.Equals(Activator.CreateInstance(value.GetType()))) return string.Empty;

            return value.ToString();
        }
    }
}

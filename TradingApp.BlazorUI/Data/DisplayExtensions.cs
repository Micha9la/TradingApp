namespace TradingApp.BlazorUI.Data
{
    public static class NumericExtensions
    {
        public static string ValueOrEmpty<TEnum>(this TEnum value) where TEnum : struct, Enum
        {
            // If value is the default (usually 0), return empty
            if (EqualityComparer<TEnum>.Default.Equals(value, default))//checks if the value is equal to the enum’s default.
                return string.Empty;

            return value.ToString();// value.ToString = name of enum
        }
        public static string ValueOrEmpty(this int x)
        {
            if (x != 0)
                return x.ToString();
            else
                return string.Empty;
        }

        public static string ValueOrEmpty(this float x)
        {
            if (x != 0)
                return x.ToString();
            else
                return string.Empty;
        }

        public static string ValueOrEmpty(this decimal x)
        {
            if (x != 0)
                return x.ToString();
            else
                return string.Empty;
        }
        public static string ValueOrEmpty(this decimal? x)//nullable value type
        {
            if (x.HasValue && x.Value != 0)
                return x.Value.ToString();
            else
                return string.Empty;
        }
        public static string ValueOrEmpty(this int? x)
        {
            if (x.HasValue && x.Value != 0)
                return x.Value.ToString();
            else
                return string.Empty;
        }

        public static string ValueOrEmpty(this float? x)
        {
            if (x.HasValue && x.Value != 0)
                return x.Value.ToString();
            else
                return string.Empty;
        }

        public static string ValueOrEmpty(this string x)
        {
            if (!string.IsNullOrWhiteSpace(x))
                return x;
            else
                return string.Empty;
        }
    }
}

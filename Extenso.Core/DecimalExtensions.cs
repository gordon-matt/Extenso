namespace Extenso
{
    public static class DecimalExtensions
    {
        public static bool Between(this decimal value, int lower, int higher)
        {
            return value > lower && value < higher;
        }

        public static bool IsNullOrDefault(this decimal? value)
        {
            return value == null || value == default(decimal);
        }
    }
}
namespace Extenso
{
    public static class SingleExtensions
    {
        public static bool IsNullOrDefault(this float? value)
        {
            return value == null || value == default(float);
        }
    }
}
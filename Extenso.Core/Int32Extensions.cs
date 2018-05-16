namespace Extenso
{
    public static class Int32Extensions
    {
        public static bool Between(this int value, int lower, int higher)
        {
            return value > lower && value < higher;
        }

        /// <summary>
        /// Indicates whether this System.Int32 is a multiple of the specified System.Int32.
        /// </summary>
        /// <param name="i">This instance of System.Int32.</param>
        /// <param name="numberToCompare">The System.Int32 to compare with</param>
        /// <returns>true if this System.Int32 is a multiple of the specified System.Int32; otherwise, false.</returns>
        public static bool IsMultipleOf(this int i, int numberToCompare)
        {
            return i % numberToCompare == 0;
        }

        public static bool IsNullOrDefault(this int? value)
        {
            return value == null || value == default(int);
        }
    }
}
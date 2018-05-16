namespace Extenso
{
    public static class CharExtensions
    {
        /// <summary>
        /// <para>Takes a System.Char and returns a System.String of that System.Char</para>
        /// <para>repeated [n] number of times</para>
        /// </summary>
        /// <param name="c">The Char</param>
        /// <param name="count">The number of times to repeat the System.Char</param>
        /// <returns>System.String of the specified System.Char repeated [n] number of times</returns>
        public static string Repeat(this char c, int count)
        {
            return new string(c, count);
        }
    }
}
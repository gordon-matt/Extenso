using System.Drawing;

namespace Extenso.Drawing
{
    public static class ColorExtensions
    {
        public static string ToHex(this Color color)
        {
            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        public static string ToRGB(this Color color)
        {
            return $"RGB({color.R},{color.G},{color.B})";
        }
    }
}
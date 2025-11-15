using System.Drawing;

namespace Extenso.Drawing;

public static class ColorExtensions
{
    extension(Color color)
    {
        public string Hex => $"#{color.R:X2}{color.G:X2}{color.B:X2}";

        public string RGB => $"RGB({color.R},{color.G},{color.B})";
    }
}
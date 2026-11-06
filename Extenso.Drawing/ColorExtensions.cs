using System.Drawing;

namespace Extenso.Drawing;

/// <summary>
/// Provides extension methods for colors.
/// </summary>
public static class ColorExtensions
{
    extension(Color color)
    {
        /// <summary>
        /// Gets the hex.
        /// </summary>
        public string Hex => $"#{color.R:X2}{color.G:X2}{color.B:X2}";

        /// <summary>
        /// Gets the rgb.
        /// </summary>
        public string RGB => $"RGB({color.R},{color.G},{color.B})";
    }
}
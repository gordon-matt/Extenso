using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Extenso.Drawing;

public static class ImageExtensions
{
    /// <summary>
    /// Calculates picture dimensions whilst maintaining aspect
    /// </summary>
    /// <param name="originalSize">The original picture size</param>
    /// <param name="targetSize">The target picture size (longest side)</param>
    /// <returns></returns>
    private static Size CalculateDimensions(Size originalSize, int targetSize)
    {
        var newSize = new Size();
        if (originalSize.Height > originalSize.Width) // portrait
        {
            newSize.Width = (int)(originalSize.Width * (float)(targetSize / (float)originalSize.Height));
            newSize.Height = targetSize;
        }
        else // landscape or square
        {
            newSize.Height = (int)(originalSize.Height * (float)(targetSize / (float)originalSize.Width));
            newSize.Width = targetSize;
        }

        if (newSize.Width < 1)
        {
            newSize.Width = 1;
        }
        if (newSize.Height < 1)
        {
            newSize.Height = 1;
        }

        return newSize;
    }

    /// <summary>
    /// Returns the first ImageCodecInfo instance with the specified mime type.
    /// </summary>
    /// <param name="mimeType">Mime type</param>
    /// <returns>ImageCodecInfo</returns>
    private static ImageCodecInfo GetImageCodecInfoFromMimeType(string mimeType) => ImageCodecInfo.GetImageEncoders()
            .FirstOrDefault(ici => ici.MimeType.Equals(mimeType, StringComparison.OrdinalIgnoreCase));

    public static MemoryStream ResizeToStream(this Image image, int targetSize, byte qualityPercent = 80, string mimeType = "image/jpeg")
    {
        var newSize = CalculateDimensions(image.Size, targetSize);

        using var stream = new MemoryStream();
        using var newBitmap = new Bitmap(newSize.Width, newSize.Height);
        using (var g = Graphics.FromImage(newBitmap))
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawImage(image, 0, 0, newSize.Width, newSize.Height);

            var parameters = new EncoderParameters();
            parameters.Param[0] = new EncoderParameter(Encoder.Quality, (long)qualityPercent);

            var encoder = GetImageCodecInfoFromMimeType(mimeType);
            newBitmap.Save(stream, encoder, parameters);
        }
        return stream;
    }

    public static void ResizeToFile(this Image image, int targetSize, string fileName, byte qualityPercent = 80, string mimeType = "image/jpeg")
    {
        var newSize = CalculateDimensions(image.Size, targetSize);

        using var stream = new MemoryStream();
        using var newBitmap = new Bitmap(newSize.Width, newSize.Height);
        using var g = Graphics.FromImage(newBitmap);
        g.SmoothingMode = SmoothingMode.HighQuality;
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.CompositingQuality = CompositingQuality.HighQuality;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        g.DrawImage(image, 0, 0, newSize.Width, newSize.Height);

        var parameters = new EncoderParameters();
        parameters.Param[0] = new EncoderParameter(Encoder.Quality, (long)qualityPercent);

        var encoder = GetImageCodecInfoFromMimeType(mimeType);
        newBitmap.Save(fileName, encoder, parameters);
    }
}
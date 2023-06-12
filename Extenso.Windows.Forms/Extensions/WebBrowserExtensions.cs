using System.Drawing.Imaging;
using Extenso.Windows.Forms.Win32;

namespace Extenso.Windows.Forms;

public static class WebBrowserExtensions
{
    public static Bitmap GetScreenshot(this WebBrowser webBrowser) => webBrowser.GetScreenshot(null);

    public static Bitmap GetScreenshot(this WebBrowser webBrowser, Size? size)
    {
        var screenshot = webBrowser.GenerateScreenshot(size);
        return screenshot;
    }

    public static void SaveScreenshot(this WebBrowser webBrowser, string fileName)
    {
        new DirectoryInfo(Path.GetDirectoryName(fileName)).EnsureExists();
        using var screenshot = webBrowser.GetScreenshot(null);
        screenshot.Save(fileName);
    }

    public static void SaveScreenshot(this WebBrowser webBrowser, string fileName, ImageFormat imageFormat)
    {
        new DirectoryInfo(Path.GetDirectoryName(fileName)).EnsureExists();
        using var screenshot = webBrowser.GetScreenshot(null);
        screenshot.Save(fileName, imageFormat);
    }

    public static void SaveScreenshot(this WebBrowser webBrowser, string fileName, Size? size)
    {
        new DirectoryInfo(Path.GetDirectoryName(fileName)).EnsureExists();
        using var screenshot = webBrowser.GetScreenshot(size);
        screenshot.Save(fileName);
    }

    public static void SaveScreenshot(this WebBrowser webBrowser, string fileName, Size? size, ImageFormat imageFormat)
    {
        new DirectoryInfo(Path.GetDirectoryName(fileName)).EnsureExists();
        using var screenshot = webBrowser.GetScreenshot(size);
        screenshot.Save(fileName, imageFormat);
    }

    //Working for most websites now, but not all
    //Many thanks to mariscn: http://www.codeproject.com/KB/graphics/html2image.aspx
    private static Bitmap GenerateScreenshot(this WebBrowser webBrowser, Size? size)
    {
        Application.DoEvents();

        var parent = webBrowser.Parent;
        var dockStyle = webBrowser.Dock;
        bool scrollbarsEnabled = webBrowser.ScrollBarsEnabled;

        if (parent != null)
        {
            parent.Controls.Remove(webBrowser);
        }

        var screen = Screen.PrimaryScreen.Bounds;
        Size? imageSize = null;

        var body = webBrowser.Document.Body.ScrollRectangle;

        //check if the document width/height is greater than screen width/height
        var docRectangle = new Rectangle
        {
            Location = new Point(0, 0),
            Size = new Size(
                body.Width > screen.Width ? body.Width : screen.Width,
                body.Height > screen.Height ? body.Height : screen.Height)
        };
        //set the width and height of the WebBrowser object
        webBrowser.ScrollBarsEnabled = false;
        webBrowser.Size = new Size(docRectangle.Width, docRectangle.Height);

        //Cannot update scrollrectangle. Wehn web browser size changed, only width of body changed; not height
        //webBrowser.Document.Body.ScrollRectangle.Width = webBrowser.Width;
        //webBrowser.Document.Body.ScrollRectangle.Height = webBrowser.Height;

        //if the imgsize is null, the size of the image will
        //be the same as the size of webbrowser object
        //otherwise  set the image size to imgsize
        Rectangle imgRectangle;
        if (imageSize == null)
        { imgRectangle = docRectangle; }
        else
            imgRectangle = new Rectangle()
            {
                Location = new Point(0, 0),
                Size = imageSize.Value
            };
        //create a bitmap object
        var bitmap = new Bitmap(imgRectangle.Width, imgRectangle.Height);
        //get the viewobject of the WebBrowser
        var viewObject = webBrowser.Document.DomDocument as IViewObject;

        using (var g = Graphics.FromImage(bitmap))
        {
            //get the handle to the device context and draw
            var hdc = g.GetHdc();
            viewObject.Draw(1, -1, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, hdc, ref imgRectangle, ref docRectangle, IntPtr.Zero, 0);
            g.ReleaseHdc(hdc);
        }

        if (parent != null)
        {
            parent.Controls.Add(webBrowser);
            webBrowser.Dock = dockStyle;
        }
        webBrowser.ScrollBarsEnabled = scrollbarsEnabled;

        return bitmap;
    }
}
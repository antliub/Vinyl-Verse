using SkiaSharp;
using Svg.Skia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace VinylVerse.Behaviors
{
    public static class SvgHelper
    {
        public static BitmapImage RenderSvgToBitmap(string svgFilePath, int width, int height)
        {
            using var stream = new FileStream(svgFilePath, FileMode.Open, FileAccess.Read);
            var svg = new SKSvg();
            svg.Load(stream);

            var bitmap = new SKBitmap(width, height);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.Transparent);
            canvas.DrawPicture(svg.Picture);

            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            var bitmapImage = new BitmapImage();
            using var memoryStream = new MemoryStream(data.ToArray());
            memoryStream.Position = 0;

            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.EndInit();

            return bitmapImage;
        }
    }
}

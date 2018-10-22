using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PixelArt_Drawing_Tool
{
    class DrawingPage
    {
        public WriteableBitmap Source { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        private int stride;

        public DrawingPage(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            Create(width, height);
        }

        private void Create(int width, int height)
        {
            // defining parameters
            PixelFormat format = PixelFormats.Bgra32;
            stride = (width * format.BitsPerPixel + 7) / 8;
            byte[] rawImage = new byte[stride * height];
            
            // creating a bitmap
            BitmapSource bitmap = BitmapSource.Create(
                width,
                height,
                96,
                96,
                format,
                null,
                rawImage,
                stride);

            Source = new WriteableBitmap(bitmap);
        }

        /// <summary>
        ///  Saves a file of the image
        ///  at the specified location.
        /// </summary>
        /// <param name="path">
        ///  Full path to the file
        ///  (Example: C:\untitled.png).
        /// </param>
        public void Save(string path)
        {
            using (
                FileStream stream = new FileStream(path,
                FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(Source));
                encoder.Save(stream);
            }
        }

        /// <summary>
        ///  Draws a pixel on the specified
        ///  location in the page.
        /// </summary>
        public void Draw(int x, int y, Color color)
        {
            byte[] colorData = { color.B, color.G, color.R, color.A };
            Int32Rect rect = new Int32Rect(x, y, 1, 1);
            Source.WritePixels(rect, colorData, stride, 0);
        }
    }
}

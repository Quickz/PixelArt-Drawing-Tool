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
        public WriteableBitmap Source => bitmap.Source;

        /// <summary>
        ///  Called when Source is deleted
        ///  or overwritten with a new one.
        /// </summary>
        public event EventHandler PageSourceChanged;

        public int Width
        {
            get
            {
                return Source.PixelWidth;
            }
        }
        public int Height
        {
            get
            {
                return Source.PixelHeight;
            }
        }

        private Bitmap bitmap;
        private Pixel hoveredPixel;

        public DrawingPage(int width, int height)
        {
            Create(width, height);
        }

        /// <summary>
        ///  Creates a new blank page.
        /// </summary>
        private void Create(int width, int height)
        {
            if (width < 1 || height < 1)
            {
                return;
            }

            hoveredPixel = null;
            bitmap = new Bitmap(width, height, PixelFormats.Bgra32);
        }

        /// <summary>
        ///  Changes the current size
        ///  of the page.
        /// </summary>
        public void ResizeTo(int width, int height)
        {
            if (this.Width == width && this.Height == height)
            {
                return;
            }

            Create(width, height);
            OnPageSourceChanged();
        }

        /// <summary>
        ///  Called when Source is deleted
        ///  or overwritten with a new one.
        /// </summary>
        private void OnPageSourceChanged()
        {
            PageSourceChanged?.Invoke(this, EventArgs.Empty);
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

        public void Load(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                var decoder = new PngBitmapDecoder(
                    stream,
                    BitmapCreateOptions.None,
                    BitmapCacheOption.Default);
                BitmapFrame image = decoder.Frames.First();
                bitmap = new Bitmap(image);

                hoveredPixel = null;
                OnPageSourceChanged();
            }
        }

        /// <summary>
        ///  Draws a pixel on the specified
        ///  location in the page.
        /// </summary>
        public void Draw(Color color)
        {
            hoveredPixel.color = color;
        }

        public void Hover(int x, int y)
        {
            if (hoveredPixel != null)
            {
                // restoring all pixel
                bitmap.WritePixel(
                    hoveredPixel.x,
                    hoveredPixel.y,
                    hoveredPixel.color);
            }

            // saving the current pixel
            hoveredPixel = new Pixel(x, y, bitmap.PixelColorAt(x, y));

            double averageColor =
                (hoveredPixel.color.R +
                hoveredPixel.color.G +
                hoveredPixel.color.B);

            // 255 * 3 / 2

            Color hoveredColor;

            // dark
            if (averageColor < 382.5d)
            {
                hoveredColor = Color.FromArgb(
                    255,
                    Lighter(hoveredPixel.color.R),
                    Lighter(hoveredPixel.color.G),
                    Lighter(hoveredPixel.color.B));
            }
            // light
            else
            {
                hoveredColor = Color.FromArgb(
                    255,
                    Darker(hoveredPixel.color.R),
                    Darker(hoveredPixel.color.G),
                    Darker(hoveredPixel.color.B));
            }

            // highlighting current pixel
            bitmap.WritePixel(x, y, hoveredColor);


            byte Lighter(byte number)
            {
                if (number + 75 > 255)
                {
                    return 255;
                }
                return (byte)(number + 75);
            }

            byte Darker(byte number)
            {
                if (number - 75 < 0)
                {
                    return 0;
                }
                return (byte)(number - 75);
            }
        }

        public void Unhover()
        {
            bitmap.WritePixel(
                hoveredPixel.x,
                hoveredPixel.y,
                hoveredPixel.color);
            hoveredPixel = null;
        }
    }
}

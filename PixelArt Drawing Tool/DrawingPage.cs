﻿using System;
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

        /// <summary>
        ///  Draws a pixel on the specified
        ///  location in the page.
        /// </summary>
        public void Draw(int x, int y, Color color)
        {
            bitmap.WritePixel(x, y, color);
        }
    }
}

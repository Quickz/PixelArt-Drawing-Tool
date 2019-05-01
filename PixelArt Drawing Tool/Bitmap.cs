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
    class Bitmap
    {
        public WriteableBitmap Source { get; private set; }
        public int Stride { get; private set; }

        public Bitmap(int width, int height, PixelFormat format)
        {
            Source = CreateWritableBitmap(width, height, format);
        }

        public void WritePixel(int x, int y, Color color)
        {
            byte[] colorData = { color.B, color.G, color.R, color.A };
            Int32Rect rect = new Int32Rect(x, y, 1, 1);
            Source.WritePixels(rect, colorData, Stride, 0);
        }

        public Color PixelColorAt(int x, int y)
        {
            int size = Source.PixelHeight * Stride;
            byte[] pixels = new byte[size];
            Source.CopyPixels(pixels, Stride, 0);

            int index = y * Stride + 4 * x;
            byte red = pixels[index];
            byte green = pixels[index + 1];
            byte blue = pixels[index + 2];
            byte alpha = pixels[index + 3];

            return Color.FromArgb(alpha, red, green, blue);
        }

        /// <returns>
        ///  2D array
        ///  [x-index, y-index]
        /// </returns>
        public Color[,] AllPixelColors()
        {
            int size = Source.PixelHeight * Stride;
            byte[] pixels = new byte[size];
            Source.CopyPixels(pixels, Stride, 0);
            Color[,] colors = new Color[Source.PixelHeight, Source.PixelWidth];

            for (int y = 0; y < Source.PixelHeight; y++)
            {
                for (int x = 0; x < Source.PixelWidth; x++)
                {
                    int index = y * Stride + 4 * x;
                    byte red = pixels[index];
                    byte green = pixels[index + 1];
                    byte blue = pixels[index + 2];
                    byte alpha = pixels[index + 3];

                    colors[x, y] = Color.FromArgb(alpha, red, green, blue);
                }
            }
            return colors;
        }

        private WriteableBitmap CreateWritableBitmap(
            int width,
            int height,
            PixelFormat format)
        {
            // defining parameters
            Stride = (width * format.BitsPerPixel + 7) / 8;
            byte[] rawImage = new byte[Stride * height];

            // creating a bitmap
            BitmapSource bitmap = BitmapSource.Create(
                width,
                height,
                96,
                96,
                format,
                null,
                rawImage,
                Stride);

            return new WriteableBitmap(bitmap);
        }
    }
}

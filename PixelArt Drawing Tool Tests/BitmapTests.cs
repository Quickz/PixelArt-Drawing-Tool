using System;
using System.Windows.Media;
using NUnit.Framework;
using Quickz.PixelArt.App;

namespace Quickz.PixelArt.Tests
{
    public class BitmapTests
    {
        private PixelFormat format = PixelFormats.Bgra32;

        [Test]
        public void ValidStride()
        {
            var image = new Bitmap(16, 16, format);
            Assert.AreEqual(
                image.Stride,
                (16 * format.BitsPerPixel + 7) / 8);
        }

        [Test]
        public void ValidSize()
        {
            var image = new Bitmap(16, 16, format);
            Assert.AreEqual(image.Source.PixelWidth, 16);
            Assert.AreEqual(image.Source.PixelHeight, 16);
        }

        [Test]
        public void IsTransparentByDefault()
        {
            var image = new Bitmap(16, 16, format);
            Color[,] colors = image.AllPixelColors();
            bool allColorsTransparent = true;
            for (int i = 0; i < colors.GetLength(0); i++)
            {
                for (int j = 0; j < colors.GetLength(1); j++)
                {
                    if (colors[i, j].A != 0)
                    {
                        allColorsTransparent = false;
                        break;
                    }
                }
            }
            Assert.AreEqual(true, allColorsTransparent);
        }

        [Test]
        public void ReadAndWriteSinglePixel()
        {
            var image = new Bitmap(32, 32, format);
            image.WritePixel(25, 25, Colors.Black);
            Color color = image.PixelColorAt(25, 25);
            Assert.AreEqual(Colors.Black, color);
        }
    }
}

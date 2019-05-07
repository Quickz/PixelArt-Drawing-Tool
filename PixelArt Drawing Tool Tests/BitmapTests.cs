using System;
using System.Windows.Media;
using NUnit.Framework;
using PixelArt_Drawing_Tool;

namespace PixelArt_Drawing_Tool_Tests
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
    }
}

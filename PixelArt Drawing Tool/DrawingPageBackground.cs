using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PixelArt_Drawing_Tool
{
    static class DrawingPageBackground
    {
        public static Bitmap CreateBackground(int width, int height)
        {
            Scale(ref width, ref height, 48);

            var background = new Bitmap(width, height, PixelFormats.Bgra32);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color color;
                    if ((i + j) % 2 == 0)
                    {
                        color = Color.FromRgb(0x8c, 0x8c, 0x8c);
                    }
                    else
                    {
                        color = Color.FromRgb(0x73, 0x73, 0x73);
                    }
                    background.WritePixel(i, j, color);
                }
            }
            return background;
        }

        private static void Scale(
            ref int width,
            ref int height,
            int targetValue)
        {
            if (width < height)
            {
                width = (int)Math.Floor(targetValue * (width / (double)height));
                height = targetValue;
            }
            else
            {
                height = (int)Math.Floor(targetValue * (height / (double)width));
                width = targetValue;
            }
        }
    }
}

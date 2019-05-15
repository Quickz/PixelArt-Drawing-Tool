using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Quickz.PixelArt.App
{
    class Pixel
    {
        public readonly int x;
        public readonly int y;
        public Color color;

        public Pixel(int x, int y, Color color)
        {
            this.x = x;
            this.y = y;
            this.color = color;
        }
    }
}

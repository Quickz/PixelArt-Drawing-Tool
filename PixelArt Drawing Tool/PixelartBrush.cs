using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PixelArt_Drawing_Tool
{
    class PixelartBrush
    {
        public event EventHandler OpacityChanged;

        public Color Color
        {
            get
            {
                Color color = rawColor;
                color.A = (byte)(2.55 * Opacity);

                return color;
            }
        }

        /// <summary>
        ///  How transparent the color is.
        /// </summary>
        public double Opacity
        {
            get
            {
                return opacity;
            }
            set
            {
                opacity = value;
                OnOpacityChanged();
            }
        }

        private double opacity;

        private Color rawColor;

        public PixelartBrush()
        {
            rawColor = Colors.Black;
            opacity = 100;
        }

        /// <summary>
        ///  Modifies the raw color
        ///  of the brush.
        /// </summary>
        public void ChangeColor(byte red, byte green, byte blue)
        {
            rawColor.R = red;
            rawColor.G = green;
            rawColor.B = blue;
        }

        private void OnOpacityChanged()
        {
            OpacityChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

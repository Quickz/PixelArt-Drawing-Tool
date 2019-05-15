using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Quickz.PixelArt.App
{
    class PixelartBrush
    {
        public event EventHandler OpacityChanged;
        public event EventHandler BrightnessChanged;

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

        public double Brightness
        {
            get
            {
                return brightness;
            }
            set
            {
                brightness = value;
                OnBrightnessChanged();
            }
        }

        private double opacity;
        private double brightness;

        private Color rawColor;

        public PixelartBrush()
        {
            rawColor = Colors.Black;
            opacity = 100;
            brightness = 0;
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
            Brightness = ((double)red + green + blue) / 765 * 100;
        }

        private void OnOpacityChanged()
        {
            OpacityChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnBrightnessChanged()
        {
            BrightnessChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

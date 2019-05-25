using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Quickz.PixelArt.App
{
    class ColorPalette
    {
        private static Color[] colors = new Color[]
        {
            Colors.White,
            Colors.Black,
            Colors.Red,
            Colors.Green,
            Colors.Blue
        };

        public static void Create(
            Panel parent,
            RoutedEventHandler paletteEntryClickCallback)
        {
            foreach (Color color in colors)
            {
                Button button = CreatePaletteColorEntry(color, parent);
                button.Click += paletteEntryClickCallback;
            }
        }

        private static Button CreatePaletteColorEntry(
            Color color,
            Panel parent)
        {
            var button = new Button();
            button.Width = 30;
            button.Height = 30;
            button.Background = new SolidColorBrush(color);
            parent.Children.Add(button);
            return button;
        }
    }
}

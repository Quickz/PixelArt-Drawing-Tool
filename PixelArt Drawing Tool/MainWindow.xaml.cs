using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace PixelArt_Drawing_Tool
{
    /// <summary>
    ///  Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DrawingPage page;
        private ShortcutManager shortcutManager = new ShortcutManager();
        private VectorInt defaultSize = new VectorInt(16, 16);
        private Color brushColor = Colors.Black;

        // last valid color text box value that was assigned
        private string SavedColorText = "#000000";

        public MainWindow()
        {
            InitializeComponent();
            page = new DrawingPage(defaultSize.x, defaultSize.y);
            PageContainer.Source = page.Source;
            page.PageSourceChanged += OnPageSourceChanged;

            LoadShortcuts();
        }

        /// <summary>
        ///  Called when image Source
        ///  is deleted or overwritten
        ///  from resizing or something else.
        /// </summary>
        private void OnPageSourceChanged(object sender, EventArgs args)
        {
            PageContainer.Source = page.Source;
        }

        /// <summary>
        ///  Draws a square at
        ///  the specified position.
        /// </summary>
        private void Draw(int x, int y, Color color)
        {
            // position outside grid
            if (x >= page.Width ||
                y >= page.Height ||
                x < 0 ||
                y < 0)
            {
                return;
            }

            page.Draw(x, y, color);
        }

        /// <summary>
        ///  Takes the pointer position and rounds it down
        ///  to a position of a pixel on the page.
        /// </summary>
        private VectorInt GetPixelPosition(Point position)
        {
            // size of single pixel
            double pixelWidth = PageContainer.ActualWidth / page.Width;
            double pixelHeight = PageContainer.ActualHeight / page.Height;

            int column = (int)Math.Floor(position.X / pixelWidth);
            int row = (int)Math.Floor(position.Y / pixelHeight);

            return new VectorInt(column, row);
        }

        private void LoadShortcuts()
        {
            shortcutManager.Add(Key.S, true, Save);
        }

        private void ProcessDrawingInput(MouseEventArgs e)
        {
            Point cursorPosition = e.GetPosition(PageContainer);
            VectorInt pixelPosition = GetPixelPosition(cursorPosition);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Draw(pixelPosition.x, pixelPosition.y, brushColor);
            }

            if (e.RightButton == MouseButtonState.Pressed)
            {
                Draw(pixelPosition.x, pixelPosition.y, Colors.Transparent);
            }
        }

        private void PageContainer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ProcessDrawingInput(e);
        }

        private void PageContainer_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed ||
                e.RightButton == MouseButtonState.Pressed)
            {
                ProcessDrawingInput(e);
            }
        }

        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Save()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(
                Environment.SpecialFolder.Desktop);

            dialog.FileName = "untitled";
            dialog.DefaultExt = ".png";
            dialog.Filter = "PNG (*.png)|*.png";

            if (dialog.ShowDialog() == true)
            {
                page.Save(dialog.FileName);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            shortcutManager.HandleKeyDown(e.Key);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            shortcutManager.HandleKeyUp(e.Key);
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonResize_Click(object sender, RoutedEventArgs e)
        {
            // resizing page if values can be converted into numbers
            if (int.TryParse(TextBoxPageWidth.Text, out int width) &&
                int.TryParse(TextBoxPageHeight.Text, out int height))
            {
                page.ResizeTo(width, height);
            }
        }

        private void ButtonChangeColor_Click(object sender, RoutedEventArgs e)
        {
            UpdateTextBoxColorContent();
            ChangeColor(TextBoxColor.Text);
            RectangleColor.Fill = new SolidColorBrush(brushColor);
        }

        /// <summary>
        ///  Changes brush color the
        ///  the specified one.
        /// </summary>
        /// <param name="colorHexString">
        ///  Hexadecimal string (Ex. #000000)
        ///  that represents a color.
        /// </param>
        private void ChangeColor(string colorHexString)
        {
            string colorHex = colorHexString.Substring(1);

            string redHex = colorHex.Substring(0, 2);
            string greenHex = colorHex.Substring(2, 2);
            string blueHex = colorHex.Substring(4, 2);

            byte red = Convert.ToByte(redHex, 16);
            byte green = Convert.ToByte(greenHex, 16);
            byte blue = Convert.ToByte(blueHex, 16);

            Color newColor = Color.FromRgb(red, green, blue);
            brushColor = newColor;
            UpdateBrushOpacity();
        }

        private void TextBoxColor_LostFocus(object sender, RoutedEventArgs e)
        {
            ButtonChangeColor.IsDefault = false;
            UpdateTextBoxColorContent();
        }

        private void TextBoxColor_GotFocus(object sender, RoutedEventArgs e)
        {
            ButtonChangeColor.IsDefault = true;
            SavedColorText = TextBoxColor.Text;
        }

        /// <summary>
        ///  Filters out any invalid content
        ///  out of color text box content.
        /// </summary>
        private void UpdateTextBoxColorContent()
        {
            string hexValue = Regex.Replace(
                TextBoxColor.Text,
                @"[^0-9a-fA-F]",
                string.Empty);

            // short version of hex value
            if (hexValue.Length == 3)
            {
                hexValue = string.Format(
                    "{0}{0}{1}{1}{2}{2}",
                    hexValue[0],
                    hexValue[1],
                    hexValue[2]);
            }
            // invalid length
            else if (hexValue.Length != 6)
            {
                TextBoxColor.Text = SavedColorText;
                return;
            }

            TextBoxColor.Text = "#" + hexValue;
        }

        private void SliderColorOpacity_ValueChanged(
            object sender,
            RoutedPropertyChangedEventArgs<double> e)
        {
            if (RectangleColor == null)
            {
                return;
            }

            brushColor.A = (byte)(2.5 * e.NewValue);
            RectangleColor.Fill = new SolidColorBrush(brushColor);
        }

        private void UpdateBrushOpacity()
        {
            brushColor.A = (byte)(2.5 * SliderColorOpacity.Value);
            RectangleColor.Fill = new SolidColorBrush(brushColor);
        }
    }
}

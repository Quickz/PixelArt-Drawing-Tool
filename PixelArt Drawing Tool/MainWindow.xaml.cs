using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public MainWindow()
        {
            InitializeComponent();
            page = new DrawingPage(16, 16);
            PageContainer.Source = page.Source;
            LoadShortcuts();
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
                Draw(pixelPosition.x, pixelPosition.y, Colors.Red);
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
    }
}

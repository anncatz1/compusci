using GraphControl;
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

namespace Interference
{
    /// <summary>
    /// Simple visualizer that holds the PictureDisplay
    /// </summary>
    public partial class InterferenceVisualizer : Window
    {
        public PictureDisplay Display;

        public InterferenceVisualizer(int rows, int columns)
        {
            InitializeComponent();

            Display = new PictureDisplay(rows, columns);
            MainSpot.Content = Display;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)ActualWidth, (int)ActualHeight, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(MainSpot);
            Clipboard.SetImage(bitmap);
        }
    }
}

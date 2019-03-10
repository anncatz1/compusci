using DongUtility;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GraphControl
{
    /// <summary>
    /// A class that displays a picture on a screen, using a grid system
    /// </summary>
    public partial class PictureDisplay : UserControl
    {
        /// <summary>
        /// The grid of colors that is used to create the display
        /// </summary>
        private Color[,] grid;
        /// <summary>
        /// The bitmap that is sent to the screen
        /// </summary>
        private WriteableBitmap bitmap;

        /// <param name="width">The width of the display, as a number of cells</param>
        /// <param name="height">The height of the display, as a number of cells</param>
        public PictureDisplay(int width, int height)
        {
            InitializeComponent();

            grid = new Color[width, height];
        }

        /// <summary>
        /// Set the color of a specific cell
        /// </summary>
        public void SetCell(int xCoord, int yCoord, Color color)
        {
            grid[xCoord, yCoord] = color; 
        }

        /// <summary>
        /// Sets the color and intensity of a cell by wavelength
        /// </summary>
        /// <param name="wavelength">Wavelength in meters</param>
        /// <param name="intensity">Intensity, as a value from 0 to 1</param>
        public void SetCell(int xCoord, int yCoord, double wavelength, double intensity)
        {
            if (intensity < 0 || intensity > 1)
                throw new ArgumentException("Invalid intensity passed to SetCell()");
            var color = LightFunctions.ConvertWavelengthToColor(wavelength);
            color.A = (byte)Math.Round(intensity * 255);
            SetCell(xCoord, yCoord, color);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            
            // Initialize the bitmap
            bitmap = new WriteableBitmap(grid.GetLength(0), grid.GetLength(1), 96, 96, PixelFormats.Bgra32, null);

            // Ready to draw
            bitmap.Lock();

            // Calculate stride length and create array from grid
            var rect = new Int32Rect(0, 0, grid.GetLength(0), grid.GetLength(1));
            int stride = bitmap.PixelWidth * ((bitmap.Format.BitsPerPixel + 7) / 8);
            var tempArray = new byte[stride * bitmap.PixelHeight];

            // Transfer grid colors to flat array
            int counter = 0;      
            // Loop over y first so we go by rows, then columns
            for (int iy = 0; iy < grid.GetLength(1); ++iy)
                for (int ix = 0; ix < grid.GetLength(0); ++ix)
                {
                    var pixel = grid[ix, iy];
                    tempArray[counter++] = pixel.B;
                    tempArray[counter++] = pixel.G;
                    tempArray[counter++] = pixel.R;
                    tempArray[counter++] = pixel.A;
                }
            bitmap.WritePixels(rect, tempArray, stride, 0);

            // Done drawing
            bitmap.Unlock();

            // Display the bitmap
            MainImage.Source = bitmap;
        }
    }
}

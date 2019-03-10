using DongUtility;
using GraphControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisualizerControl;
using VisualizerControl.Visualizations;

namespace Visualizer
{
    /// <summary>
    /// Interaction logic for FullVisualizer.xaml
    /// </summary>
    public partial class FullVisualizer : Window
    {
        private IVisualization viz;
        public VisualizerControl.Visualizer Visualizer { get; }

        public FullVisualizer(IVisualization viz)
        {
            InitializeComponent();

            this.viz = viz;
            Visualizer = new VisualizerControl.Visualizer(viz);
            Viewport.Content = Visualizer;
            timer.Elapsed += OnTick;
        }

        private bool timeRunning = false;
        private Timer timer = new Timer(1);
        private double time = 0;
        private double timeIncrement = .01;

        public delegate Vector3D VectorFunc();

        public void Add3DGraph(string name, Timeline.GetValue funcX, VectorFunc funcY, string xTitle, string yTitle)
        {
            GraphUnderlying graphU = new GraphUnderlying(xTitle, yTitle);
            graphU.AddTimeline(new Timeline("x " + name, funcX, (() => funcY().X), Colors.Red));
            graphU.AddTimeline(new Timeline("y " + name, funcX, (() => funcY().Y), Colors.Green));
            graphU.AddTimeline(new Timeline("z " + name, funcX, (() => funcY().Z), Colors.Blue));
            Graphs.AddGraph(new Graph(graphU));
        }

        protected override void OnClosed(EventArgs e)
        {
            timer.Stop();
            base.OnClosed(e);
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            if (timeRunning)
            {
                timeRunning = false;
                timer.Stop();
                Start_Button.Content = "Start!";
            }
            else
            {
                timeRunning = true;
                timer.Start();
                Start_Button.Content = "Pause";
            }
        }

        private void TimeIncrement_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Double.TryParse(TimeIncrementSlider.Text, out double result))
            {
                timeIncrement = result;
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            //adjustCamera();
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            bool needToRestart = false;
            if (timeRunning)
            {
                timer.Stop();
                needToRestart = true;
            }
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "Screenshot",
                DefaultExt = ".jpg"
            };

            if (dlg.ShowDialog() == true)
            {
                string filename = dlg.FileName;

                RenderTargetBitmap bitmap = new RenderTargetBitmap((int)ActualWidth, (int)ActualHeight, 96, 96, PixelFormats.Pbgra32);
                bitmap.Render(this);
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                using (Stream fileStream = File.Create(filename))
                {
                    encoder.Save(fileStream);
                }
            }

            if (needToRestart)
            {
                timer.Start();
            }
        }

        private void MaxPointsSlider_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Double.TryParse(MaxPointsSlider.Text, out double result))
            {
                GraphControl.Timeline.MaximumPoints = result;
            }
        }

        private void AutoCameraCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (AutoCameraCheck.IsChecked == true)
            {
                Visualizer.AdjustCamera();
            }
        }

        private void OnTick(object sender, ElapsedEventArgs e)
        {
            if (timeRunning)
            {
                // Stop the timer to give it time to render
                timer.Stop();
                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        bool result = viz.Tick(time += timeIncrement);

                        Graphs.Update();

                        if (!result)
                        {
                            Start_Button_Click(null, null);
                            return;
                        }
                        
                        if (AutoCameraCheck.IsChecked == true)
                        {
                            Visualizer.AdjustCamera();
                        }

                        // Now restart the timer once it is done computing
                        if (timeRunning)
                        {
                            timer.Start();
                        }
                    });
                }
                catch (Exception)
                {
                    throw;
                    // Do nothing
                }
            }
        }

        public delegate double Function();
        public Function SoundFunction { get; set; }
        public Function TimeFunction { get; set; }
        public IEngine EngineForSound { get; set; }
        public double SoundLength { get; set; } = 1;
        public double Amplification { get; set; } = 1;

        private void Play_Button_Click(object sender, RoutedEventArgs e)
        {
            if (SoundFunction == null)
                return;

            Run_Button.Content = "Playing...";
            var list = new List<Tuple<double, double>>();
            double countTime = 0;
            while (countTime < SoundLength)
            {
                EngineForSound.Tick(countTime + timeIncrement);
                countTime = TimeFunction();
                list.Add(new Tuple<double, double>(countTime, SoundFunction()));
            }

            var writer = new WavFileWriter
            {
                Amplification = Amplification
            };
            writer.CreateInterpolatedSamples(list);
            writer.WriteFile("temp.wav");

            SoundPlayer player = new SoundPlayer("temp.wav");

            player.PlaySync();
            Run_Button.Content = "Play Sound";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Interference
{
    class SoundWave
    {
        public double Amplitude { get; set; }
        public double Intensity { get; set; }
        public double PathLength { get; set; }
        public Vector StartPosition { get; set; }
        public double AngleVertical { get; set; }
        public double AngleHorizontal { get; set; }
        const int speed = 343;
        const int frequency = 200;
        const double wavelength = speed / frequency;

        public SoundWave(Vector startPosition, double angleVertical, double angleHorizontal)
        {
            StartPosition = startPosition;
            AngleVertical = angleVertical;
            AngleHorizontal = angleHorizontal;
        }

        public static double GetAmplitudeMultipleWaves(List<SoundWave> waves)
        {
            return GetAmplitude(waves);
        }


        //finds farthest spacing between bright spots
        static private double FindMax(List<double> list)
        {
            double max = 0;
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i] > max)
                {
                    max = list[i];
                }
            }
            return max;
        }

        static private double GetAmplitude(List<SoundWave> waves)
        {
            List<double> amplitudeSums = new List<double>();
            double k = 2 * Math.PI / wavelength;
            double sum = 0; double amp = 0; double maxAmp = 0;
            for (double t = 0; t < wavelength / speed; t += (wavelength / speed) / 50)
            {
                sum = 0;
                for (int i = 0; i < waves.Count(); i++)
                {
                    amp = Math.Cos(speed * k * t - k * waves[i].PathLength);
                    sum += amp;
                }
                amplitudeSums.Add(sum);
            }
            maxAmp = FindMax(amplitudeSums);
            return maxAmp;
        }      
    }
}

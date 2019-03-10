using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Interference
{
    static internal class InterferenceDriver
    {
        static Dictionary<double, double> PosAndIntensity = new Dictionary<double, double>(); //stores col # (position) and intensity of all points on screen
        static List<double> BrightestSpotsPos = new List<double>(); //stores position of all bright spots
        static List<double> Spacing = new List<double>(); //stores the spacing between all bright spots
        const int numColumns = 10000;
        const double distBtwnSlits = 0.000125;
        const double center = .5;
        const int rows = 1;
        const double distToScreen = 10;

        static public void Run()
        {
            //RunLevelOne();
            //RunLevelTwo();
            VizualizeLevel3();
        }

        //visualizes double slit interference
        static public void RunLevelOne()
        {
            // Create the visualizer
            // Use integers for the number of columns and rows
            var viz = new InterferenceVisualizer(numColumns, rows);

            // Do your work here
            double wavelength = .000000525;
            const double amp = 0.5;
            double k = 2 * Math.PI / wavelength;
            double d1 = 0;
            double d2 = 0;
            double intensity = 0;
            // Use this to set the color and intensity of a cell
            // Intensity must be between zero and one
            using (StreamWriter writer = File.CreateText(@"c:\annie\LevelOne.txt"))
            {
                for (int i = 0; i < numColumns; i++)
                {
                    d1 = Math.Sqrt(Math.Pow((center - distBtwnSlits / 2) - (double)i / numColumns, 2) + Math.Pow(distToScreen, 2));
                    d2 = Math.Sqrt(Math.Pow((center + distBtwnSlits / 2) - (double)i / numColumns, 2) + Math.Pow(distToScreen, 2));
                    intensity = Math.Pow(2 * amp * Math.Cos(0.5 * k * (d2 - d1)), 2);
                    PosAndIntensity.Add(i, intensity);
                    writer.WriteLine(i + " " + intensity);
                    viz.Display.SetCell(i, 0, wavelength, intensity);
                }
            }

            //See it!
            viz.Show();
        }

        // goes through all wavelength/distBtwnSlit/distToScreen choices and outputs max and min spacing
        static public void RunLevelTwo()
        {
            // Do your work here
            double wavelength = .000000525;
            const double amp = 0.5;
            double k = 2 * Math.PI / wavelength;
            double d1 = 0; double d2 = 0; double intensity = 0;
            double length = 10;
            List<double> Wavelengths = new List<double>();
            double[] arrayWave = new double[] { .00000008, .0000003, .000000525, .0000008, .000001, .0000012,
             .0000015, .0000018, .000002, .0000025};
            Wavelengths.AddRange(arrayWave);

            List<double> Distances = new List<double>();
            double[] arrayDistances = new double[] { .00002, .00004, .00007, 0.000125, .00035, .0005, .0008,
                .001, 0.0013, 0.0016};
            Distances.AddRange(arrayDistances);

            List<double> LengthsToScreen = new List<double>();
            double[] arrayLengths = new double[] { 2, 4, 6, 8, 10, 11, 12, 14, 16, 17 };
            LengthsToScreen.AddRange(arrayLengths);
            using (StreamWriter writer3 = File.CreateText(@"c:\annie\Level2.txt"))
            {
                for (int m = 0; m < LengthsToScreen.Count(); m++)
                {
                    //wavelength = Wavelengths[m]; 
                    //k = 2 * Math.PI / Wavelengths[m];
                    //distbtwnSlits = Distances[m];
                    length = LengthsToScreen[m];

                    for (int i = 0; i < numColumns; i++)
                    {
                        d1 = Math.Sqrt(Math.Pow((center - distBtwnSlits / 2) - (double)i / numColumns, 2) + Math.Pow(length, 2));
                        d2 = Math.Sqrt(Math.Pow((center + distBtwnSlits / 2) - (double)i / numColumns, 2) + Math.Pow(length, 2));
                        intensity = Math.Pow(2 * amp * Math.Cos(0.5 * k * (d2 - d1)), 2);
                        PosAndIntensity.Add(i, intensity);
                    }

                    FindBrightestSpots();
                    FindSpacing();
                    double max = FindMax(Spacing);
                    double min = FindMin(Spacing);
                    writer3.WriteLine(Wavelengths[m] + "\t" + max + "\t" + min);
                    writer3.WriteLine(Distances[m] + "\t" + max + "\t" + min);
                    writer3.WriteLine(length + "\t" + max + "\t" + min);

                    PosAndIntensity.Clear();
                    BrightestSpotsPos.Clear();
                    Spacing.Clear();
                }
            }
        }

        //fills in list of positions of brightest spots on screen
        static public void FindBrightestSpots()
        {
            for (int col = 1; col < PosAndIntensity.Count() - 1; col++)
            {
                if (PosAndIntensity[col] > PosAndIntensity[col + 1] && PosAndIntensity[col] > PosAndIntensity[col - 1])
                {
                    BrightestSpotsPos.Add((double)col / numColumns);
                }
            }
        }

        //fills in list of the spacing between brightest spots on screen
        static public void FindSpacing()
        {
            for (int i = 0; i < BrightestSpotsPos.Count() - 1; i++)
            {
                Spacing.Add((double)BrightestSpotsPos[i + 1] - BrightestSpotsPos[i]);
            }
        }

        //finds closest spacing between bright spots
        static public double FindMin(List<double> list)
        {
            double minSpacing = double.MaxValue;
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i] < minSpacing)
                {
                    minSpacing = list[i];
                }
            }
            return minSpacing;
        }

        //finds farthest spacing between bright spots
        static public double FindMax(List<double> list)
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

        //level 3 visualizer
        static public void VizualizeLevel3()
        {
            double wavelength = .000000525;
            double intensity = 0;
            var viz = new InterferenceVisualizer(numColumns, rows);
            List<double> listIntensities = new List<double>();
            using (StreamWriter writer = File.CreateText(@"c:\annie\Level3Part2.txt"))
            {
                for (int i = 0; i < numColumns; i++)
                {
                    intensity = Math.Pow(GetAmplitude(i), 2);
                    listIntensities.Add(intensity);
                }

                //finds max intensity of the list and divides all intensities by it, so the intensities are scaled down to work with visualizer
                double maxIntensity = FindMax(listIntensities);
                for (int i = 0; i < listIntensities.Count(); i++)
                {
                    listIntensities[i] = listIntensities[i] / maxIntensity;
                    writer.WriteLine(i + "\t" + listIntensities[i]);
                    viz.Display.SetCell(i, 0, wavelength, listIntensities[i]);
                }
            }
            viz.Show();
        }

        static public double GetAmplitude(double position)
        {
            double slitWidth = 0.00001;
            //part 1 slit points are for the triple slit
            List<double> slitPointsPart1 = new List<double>() { center, center - distBtwnSlits, center + distBtwnSlits };
            //part 2 slit points are for the double slit with fixed width, and are the positions of the left side of the slit
            List<double> slitPointsPart2 = new List<double>() { center - distBtwnSlits / 2 - slitWidth, center + distBtwnSlits / 2 };
            bool width = true; //this boolean dictates whether there is a finite slit width or not - true for finite slit width, false for slit is just a point
            position = position / numColumns;
            List<double> listDistances = GetListDistances(slitPointsPart2, width, position);
            List<double> amplitudeSums = new List<double>();
            const int speedOfLight = 299792458;
            double wavelength = .000000525;
            double k = 2 * Math.PI / wavelength;
            double sum = 0;
            double amp = 0;
            double maxAmp = 0;
            for (double t = 0; t < wavelength / speedOfLight; t += (wavelength / speedOfLight) / 50)
            {
                sum = 0;
                for (int i = 0; i < listDistances.Count(); i++)
                {
                    amp = Math.Cos(speedOfLight * k * t - k * listDistances[i]);
                    sum += amp;
                }
                amplitudeSums.Add(sum);
            }
            maxAmp = FindMax(amplitudeSums);
            return maxAmp;
        }

        //gets distance between a slit and a spot on the screen
        static public double GetDistance(double slitPosition, double positionOnScreen)
        {
            return Math.Sqrt(Math.Pow(positionOnScreen - slitPosition, 2) + Math.Pow(distToScreen, 2));
        }

        //gets all the distances for all the points of multiple slits to one specific position on the screen
        static public List<double> GetListDistances(List<double> slits, bool width, double position)
        {
            List<double> listDistances = new List<double>();
            double slitWidth = 0.00001;
            double slitPos = 0;
            if (width == false)
            {
                for (int m = 0; m < slits.Count(); m++)
                {
                    listDistances.Add(GetDistance(slits[m], position));
                }
            }
            else
            {
                for (int m = 0; m < slits.Count(); m++)
                {
                    for (double step = 0; step < slitWidth; step += 0.00001)
                    {
                        slitPos = slits[m] + step;
                        listDistances.Add(GetDistance(slitPos, position));
                    }
                }
            }
            return listDistances;
        }
    }
}

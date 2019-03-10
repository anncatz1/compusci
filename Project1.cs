using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Interference
{
    class Project1
    {
        const int numColumns = 10;
        const int numRows = 10;
        const double wallLength = 14.82;
        const double wallWidth = 6.071;
        const int speed = 343;
        const int frequency = 200;
        const double wavelength = speed / frequency;
        const double speakerHeight = 2.85;
        const double leftSpeakerY = 0.65;
        const double rightSpeakerY = 4.19;
        const double planeHeight = 2; //plane at height 2m is where we're checking sound volume
        static List<List<double>> IntensityArray = new List<List<double>>();
        static List<SoundWave> waves = new List<SoundWave>();

        static public void Run()
        {
            ProjectOne();
        }

        static public void ProjectOne()
        {
            AddWaves();
            Vector wallLeft = new Vector(0, 0, 2);
            Vector wallRight = new Vector(6.07, 0, 2);
            Vector wallFront = new Vector(0, 0, 2);
            using (StreamWriter writer = File.CreateText(@"C:\annie\compusciDocs\ProjOneIntensityArray.txt"))
            {
                List<SoundWave> wavesAtPoint = new List<SoundWave>();
                double intensity = 0;
                for (int i = 0; i < numColumns; i++)
                {
                    IntensityArray.Add(new List<double>());
                    for (int j = 0; j<numRows; j++)
                    {
                        for (int m = 0; m<waves.Count(); m++)
                        {
                            if (Math.Atan((speakerHeight-planeHeight)/ ((wallWidth / numColumns) * i)) != waves[m].AngleVertical &&
                                Math.Atan((((wallLength/numRows)*j)-leftSpeakerY)/((wallWidth/numColumns)*i)) != waves[m].AngleHorizontal)
                            {
                                waves[m].PathLength = GetDistance(waves[m].StartPosition, new Vector(i, j, 2));
                                wavesAtPoint.Add(waves[m]);
                                intensity = Math.Pow(SoundWave.GetAmplitudeMultipleWaves(wavesAtPoint), 2);
                                IntensityArray[i].Add(intensity);
                            }
                        }              
                        /*for (int k = 0; k<waves.Count(); k++)
                        {
                            writer.WriteLine("(" + (wallWidth / numColumns) * i + ", " + (wallLength / numRows) * j + ")" + ". Path Length: " + waves[k].PathLength);
                        }*/
                    }
                }

                double maxIntensity = FindMax(IntensityArray);
                for (int i = 0; i < numColumns; i++)
                {
                    for (int j = 0; j < numRows; j++)
                    {
                        IntensityArray[i][j] /= maxIntensity;
                        writer.WriteLine(IntensityArray[i][j]);
                    }
                }
            }
            FindMin(IntensityArray);
            Visualize();
        }

        static public void AddWaves()
        {
            int numAngles = 4;
            double angleDifference = 360 / numAngles;
            for (int i = 0; i < numAngles; i++)
            {
                for (int j = 0; j < numAngles; j++)
                {
                    waves.Add(new SoundWave(new Utilities.Vector(0, leftSpeakerY, speakerHeight), angleDifference * i, angleDifference * j));
                }
            }

            for (int i = 0; i < numAngles; i++)
            {
                for (int j = 0; j < numAngles; j++)
                {
                    waves.Add(new SoundWave(new Utilities.Vector(0, rightSpeakerY, speakerHeight), angleDifference * i, angleDifference * j));
                }
            }
        }
        
        static public double GetDistance(Vector positionOne, Vector positionTwo)
        {
            return (positionOne - positionTwo).Magnitude;
        }

        //finds minimum value of a list
        static public void FindMin(List<List<double>> list)
        {
            double min = double.MaxValue;
            double minX = 0;
            double minY = 0;
            for (int i = 0; i < list.Count(); i++)
            {
                for (int j = 0; j < list[i].Count(); j++)
                {
                    if (list[i][j] < min)
                    {
                        min = list[i][j];
                        minX = (wallWidth / numColumns) * i;
                        minY = (wallLength / numRows) * j;
                    }
                }
            }
            using (StreamWriter writer = File.CreateText(@"C:\annie\compusciDocs\ProjectOne.txt"))
            {
                writer.WriteLine("Min Volume: " + min + " at (" + minX + ", " + minY + ", 2)");
            }
        }

        //finds maximum value in a list
        static public double FindMax(List<List<double>> list)
        {
            double max = 0;
            for (int i = 0; i < list.Count(); i++)
            {
                for (int j = 0; j < list[i].Count(); j++)
                {
                    if (list[i][j] > max)
                    {
                        max = list[i][j];
                    }
                }
            }
            return max;
        }

        //level 3 visualizer
        static void Visualize()
        {
            var viz = new InterferenceVisualizer(numColumns, numRows);
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j<numColumns; j++)
                {
                    viz.Display.SetCell(i, j, wavelength, IntensityArray[i][j]);
                }
            }            
            viz.Show();
        }       
    }
}

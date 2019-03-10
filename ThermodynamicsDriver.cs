using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Thermodynamics;
using VisualizerControl;
using VisualizerControl.Shapes;

namespace Visualizer.Thermodynamics
{
    class ThermodynamicsDriver
    {
        static internal void Run()
        {
            const double containerSize = 50;
            const double minSpeed = 1;
            const double maxSpeed = 10;
            const double mass = 1;
            Color color = Colors.Aquamarine;
            const int nParticles = 1000;
            const double deltaTime = .01;
            const string name = "Molecule";

            var cont = new ParticleContainer(containerSize, containerSize, containerSize);
            var generator = new FlatGenerator(cont, minSpeed, maxSpeed);
            cont.Dictionary.AddParticle(new ParticleInfo(name, mass, color));
            cont.AddRandomParticles(generator, name, nParticles);

            var visualization = new ThermodynamicsVisualization(cont);
            
            var viz = new FullVisualizer(visualization);

            viz.Visualizer.TimeIncrement = deltaTime;
            viz.Visualizer.TimeScale = 1;

//            viz.AddGraph("Temperature", () => visualization.Time, cont.GetTemperature, "Time (s)", "Temperature (K)", Colors.CornflowerBlue);

            viz.Graphs.AddHist(() => cont.GetParticlePropertyList((Particle part) => part.Velocity.Magnitude), 50, Colors.BlueViolet, "Speed (m/s)");
//            viz.Graphs.AddText("Temperature (K)", () => cont.GetTemperature(), Colors.CadetBlue);

            viz.Visualizer.AutoCamera = true;
            viz.Visualizer.AutoCamera = false;
            viz.Show();
        }
    }
}

using DongUtility;
using GraphControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Thermodynamics;

namespace Visualizer.Aerodynamics
{
    static class AerodynamicsDriver
    {
        static public void Run()
        {
            const double containerSize = 50;
            const double mass = 1;
            Color color = Colors.Aquamarine;
            const int nParticles = 1000;
            const double deltaTime = .1;
            const string name = "Molecule";
            const double temperature = 1;
            const double reactionRadius = 1;
            const double gridSize = 1;

            var cont = new InteractingParticleContainer(containerSize, containerSize, containerSize, gridSize, gridSize, gridSize, reactionRadius);
            var info = new ParticleInfo(name, mass, color);
            var generator = new BoltzmannGenerator(cont, temperature, info, nParticles);
            cont.Dictionary.AddParticle(info);
            cont.AddRandomParticles(generator, name, nParticles);

            var visualization = new AerodynamicsVisualization(cont);

            var viz = new FullVisualizer(visualization);

            viz.Visualizer.TimeIncrement = deltaTime;
            viz.Visualizer.TimeScale = 1;

            //viz.AddGraph("Temperature", () => visualization.Time, cont.GetTemperature, "Time (s)", "Temperature (K)", Colors.CornflowerBlue);

            // Here is how you can make your own multiple graphs based on a function you can call in the container
            // Here it is named ParticleDensity()
            //var gu = new GraphUnderlying("Time (s)", "Density (particles/m^3)");
            //gu.AddTimeline(new Timeline("Lower", () => visualization.Time, () => cont.ParticleDensity(0, cont.Size.Z / 3), Colors.Red));
            //gu.AddTimeline(new Timeline("Middle", () => visualization.Time, () => cont.ParticleDensity(cont.Size.Z / 3, 2 * cont.Size.Z / 3), Colors.Green));
            //gu.AddTimeline(new Timeline("Upper", () => visualization.Time, () => cont.ParticleDensity(2 * cont.Size.Z / 3, cont.Size.Z), Colors.Blue));
            //viz.Graphs.AddGraph(new Graph(gu));

            viz.Graphs.AddHist(() => cont.GetParticlePropertyList((Particle part) => part.Velocity.Magnitude), 50, Colors.BlueViolet, "Speed (m/s)");
            //viz.Graphs.AddText("Temperature (K)", () => cont.GetTemperature(), Colors.CadetBlue);

            viz.Visualizer.AutoCamera = true;
            viz.Visualizer.AutoCamera = false;
            viz.Show();
        }
    }
}

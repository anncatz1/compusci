using DongUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using VisualizerControl.Shapes;
using VisualizerControl.Visualizations;
using Projectile_Motion;
using Utilities;
using System.IO;

namespace Visualizer
{
    static internal class KinematicsDriver
    {
        static Vector3D ConvertToVector3D(Utilities.Vector vector)
        {
            return new Vector3D(vector.X, vector.Y, vector.Z);
        }

        static internal void RunKinematics()
        {
            //Create an engine, for example:
            Utilities.Vector gravity = new Utilities.Vector(0, 0, -9.8);
            World engine = new World(0, gravity);        
            
            const double mass1 = 5;
            const double drag_coeff = 0.3;
            Projectile proj1 = new Projectile(mass1, new Utilities.Vector(1, 1, 1), new Utilities.Vector(-2, 1, 3), new Utilities.Vector(0, 0, 0), drag_coeff, true, true, true);
            engine.AddProjectile(proj1);
            Spring spring1 = new Spring(proj1);
            engine.AddSpring(spring1);

            const double mass2 = 3;
            Projectile proj2 = new Projectile(mass2, new Utilities.Vector(2, 2, 2), new Utilities.Vector(1, -3, 2), new Utilities.Vector(0, 0, 0), drag_coeff, true, true, false);
            engine.AddProjectile(proj2);
            Spring spring2 = new Spring(proj1,proj2);
            engine.AddSpring(spring2);

            // Once you have created an adapter class for engine, you can create an instance
            var adapter = new EngineAdapter(engine);

            Sphere3D.NSegments = 40;

            // Create a visualization
            var visualization = new SingleParticleVisualization(adapter);

            // Make a visualizer
            var fullViz = new FullVisualizer(visualization);

            /*
            // Add graphs
            fullViz.Add3DGraph("Position", () => engine.Time, () => ConvertToVector3D(engine.projectiles[0].Pos), "Time (s)", "Position (m)");
            fullViz.Add3DGraph("Velocity", () => engine.Time, () => ConvertToVector3D(engine.projectiles[0].Vel), "Time (s)", "Velocity (m/s)");
            fullViz.Add3DGraph("Acceleration", () => engine.Time, () => ConvertToVector3D(engine.projectiles[0].Accel), "Time (s)", "Acceleration (m/s^2)");

            fullViz.Graphs.AddSingleGraph("Speed", () => engine.Time, (() => engine.projectiles[0].Vel.Magnitude),
            Colors.Teal, "Time (s)", "Speed (m/s)");*/

            // Add graphs
            fullViz.Add3DGraph("Position", () => engine.Time, () => ConvertToVector3D(engine.CoMPosition), "Time (s)", "Position of Center of Mass (m)");
            fullViz.Add3DGraph("Velocity", () => engine.Time, () => ConvertToVector3D(engine.CoMVelocity), "Time (s)", "Velocity of Center of Mass (m/s)");
            fullViz.Add3DGraph("Acceleration", () => engine.Time, () => ConvertToVector3D(engine.CoMAccel), "Time (s)", "Acceleration of Center of Mass (m/s^2)");

            fullViz.Graphs.AddSingleGraph("Distance between Projectiles", () => engine.Time, (() => engine.Projectiles[0].Dist(engine.Projectiles[0], engine.Projectiles[1])),
            Colors.Teal, "Time (s)", "Speed (m/s)");

            // Run it!
            fullViz.Show();
        }

    }
}

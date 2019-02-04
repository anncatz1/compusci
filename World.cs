using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Projectile_Motion
{
    public class World
    {
        public double Time { get; set; }
        public Vector Gravity { get; set; } 
        public List<Projectile> projectiles { get; set; } = new List<Projectile>();
        //private StreamWriter sw = File.CreateText(@"C:\Users\student\Dropbox\source\repos\CompuSci\Projectile Motion\SeparateClassesLevel3.txt");

        public World(double time, Vector gravity)
        {
            Time = time;
            Gravity = gravity;
        }

        public void Tick(double deltaTime)
        {
            //Time += deltaTime;
            List<Vector> forces = AddForces();
            foreach (var proj in projectiles)
            {
                proj.Move(deltaTime, forces);
                //sw.WriteLine(Time + "\t" + proj);
            }
        }

        public void Run(double totalTime, double deltaTime)
        {             
            while (Time < totalTime)
            {
                Tick(deltaTime);
            }
            //sw.Close();
            //Console.Read();
        }

        public void AddProjectile(Projectile proj)
        {
            projectiles.Add(proj);
        }

        public List<Vector> AddForces()
        {
            List<Vector> forces = new List<Vector>();
            foreach (var proj in projectiles)
            {
                forces.Add(proj.CalcGravityForce(Gravity));
                if (proj.HasAirR)
                {
                    forces.Add(proj.CalcAirResistance());
                }
                if (proj.HasSpringF)
                {
                    forces.Add(proj.CalcSpringForce());
                }
            }
            return forces;
        }
    }
}

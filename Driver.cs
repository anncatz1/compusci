using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Projectile_Motion
{
    class Driver
    {
        static void Main(string[] args)
        {
            //LevelOne();
            //LevelTwo();
            LevelThree();
        }

        static void LevelOne()
        {
            double time = 0;
            Vector gravity = new Vector(0, 0, -9.8);
            const double mass = 5;
            const double drag_coeff = 0.3;
            double totalTime = 10;
            double deltaTime = 0.01;
            double initialvelmag = 10;
            Vector vel = new Vector(initialvelmag * Math.Cos(Math.PI / 4), 0, initialvelmag * Math.Sin(Math.PI / 4));
            Projectile proj = new Projectile(mass, new Vector(0, 0, 0), vel, new Vector(0, 0, 0), drag_coeff, false, false);
            //using (StreamWriter writer = File.CreateText(@"c:\Users\student\source\repos\CompuSci\Projectile Motion\SeparateClassesLevel1.txt"))
            //{
                World world = new World(time, gravity);
                world.AddProjectile(proj);
                //writer.WriteLine("Time (s) \t x \t y \t z \t Distance \t vx \t vy \t vz \t Speed \t ax \t ay \t az \t Acceleration"); //header
                world.Run(totalTime, deltaTime);
            //}
        }

        static void LevelTwo()
        {
            double time = 0;
            Vector gravity = new Vector(0, 0, -9.8);
            const double mass = 5;
            const double drag_coeff = 0.3;
            double totalTime = 10;
            double deltaTime = 0.01;
            double initialvelmag = 10;
            Vector vel = new Vector(initialvelmag * Math.Cos(Math.PI / 4), 0, initialvelmag * Math.Sin(Math.PI / 4));
            Projectile proj = new Projectile(mass, new Vector(0, 0, 0), vel, new Vector(0, 0, 0), drag_coeff, true, false);
            //using (StreamWriter writer = File.CreateText(@"c:\Users\student\source\repos\CompuSci\Projectile Motion\SeparateClassesLevel2.txt"))
            //{
                World world = new World(time, gravity);
                world.AddProjectile(proj);
                //writer.WriteLine("Time (s) \t x \t y \t z \t Distance \t vx \t vy \t vz \t Speed \t ax \t ay \t az \t Acceleration"); //header
                world.Run(totalTime, deltaTime);
            //}
        }

        static void LevelThree()
        {
            double time = 0;
            Vector gravity = new Vector(0, 0, -9.8);
            const double mass = 5;
            const double drag_coeff = 0.3;
            double totalTime = 50;
            double deltaTime = 0.01;
            Projectile proj = new Projectile(mass, new Vector(1, 1, 1), new Vector(-2, 1, 3), new Vector(0, 0, 0), drag_coeff, true, true);
            //using (StreamWriter writer = File.CreateText(@"c:\Users\student\source\repos\CompuSci\Projectile Motion\SeparateClassesLevel3.txt"))
            //{
                World world = new World(time, gravity);
                world.AddProjectile(proj);
                //writer.WriteLine("Time (s) \t x \t y \t z \t Distance \t vx \t vy \t vz \t Speed \t ax \t ay \t az \t Acceleration"); //header
                world.Run(totalTime, deltaTime);
            //}
        }
    }    
}

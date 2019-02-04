using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Projectile_Motion
{
    public class Projectile
    {
        public double Mass { get; set; }
        public Vector Vel { get; set; }
        public Vector Pos { get; set; } 
        public Vector Accel { get; set; }
        public double Drag_coeff { get; } = 0.3;
        public bool HasAirR { get; set; }
        public bool HasSpringF { get; set; }

        public Projectile(double mass, Vector pos, Vector vel, Vector accel, double drag_coeff, bool hasAirR, bool hasSpringF)
        {
            Mass = mass;
            Pos = pos;
            Vel = vel;
            Accel = accel;
            Drag_coeff = drag_coeff;
            HasAirR = hasAirR;
            HasSpringF = hasSpringF;
        }

        public Vector CalcGravityForce(Vector gravity)
        {
            return Mass * gravity;
        }

        public Vector CalcAirResistance()
        {
            return new Vector(-Drag_coeff * Vel.X * Vel.Magnitude, -Drag_coeff * Vel.Y * Vel.Magnitude, -Drag_coeff * Vel.Z * Vel.Magnitude); 
        }

        public Vector CalcSpringForce()
        {
            Vector SpringForce = new Vector(0, 0, 0);
            SpringForce.X = CalcSpringComponent(Pos.X);
            SpringForce.Y = CalcSpringComponent(Pos.Y);
            SpringForce.Z = CalcSpringComponent(Pos.Z);
            return SpringForce;
        }

        //calculates force of the spring for each direction component, using directional cosine
        double CalcSpringComponent(double wantComponent)
        {
            const double springForceConst = 2;
            double fSpringComp = -springForceConst * (Pos.Magnitude - 1) * wantComponent / Math.Sqrt(Math.Pow(Pos.X, 2) + Math.Pow(Pos.Y, 2) + Math.Pow(Pos.Z, 2));
            return fSpringComp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <param name="forces"></param>
        public void Move(double deltaTime, List<Vector> forces)
        {
            Vector TotalForce = new Vector(0, 0, 0);
            foreach (var force in forces)
            {
                TotalForce = TotalForce + force;
            }
            //Console.WriteLine(TotalForce);            
            Accel = TotalForce / Mass;
            Vel = Vel + Accel * deltaTime; 
            Pos = Pos + Vel * deltaTime;
        }        

        public override string ToString()
        {
            return Pos.X + "\t" + Pos.Y + "\t" + Pos.Z + "\t" + Pos.Magnitude + "\t" + Vel.X + "\t" + Vel.Y + "\t" +
                        Vel.Z + "\t" + Vel.Magnitude + "\t" + Accel.X + "\t" + Accel.Y + "\t" + Accel.Z + "\t" + Accel.Magnitude;
        }
    }    
}

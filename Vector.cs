using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public struct Vector
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public double Magnitude => Math.Sqrt(X * X + Y * Y + Z * Z);
        
        //constructor
        public Vector (double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z; 
        }

        static public Vector operator+(Vector v1, Vector v2)
        {
            Vector addedVector = new Vector(
                v1.X + v2.X,
                v1.Y + v2.Y,
                v1.Z + v2.Z
                );
            return addedVector;
        }

        static public Vector operator-(Vector v1, Vector v2)
        {
            return new Vector(
                v1.X - v2.X,
                v1.Y - v2.Y,
                v1.Z - v2.Z
                );
        }

        static public Vector operator*(Vector v1, double scalar)
        {
            return new Vector(
                v1.X * scalar,
                v1.Y * scalar,
                v1.Z * scalar
                );            
        }

        static public Vector operator /(Vector v1, double scalar)
        {
            return new Vector(
                v1.X / scalar,
                v1.Y / scalar,
                v1.Z / scalar
                );
        }

        static public Vector operator*(double scalar, Vector v1)
        {
            return v1 * scalar;
        }

        public bool EqualsV(Vector v2)
        {
            if (X == v2.X && Y == v2.Y && Z == v2.Z)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //will allow you to print the vector
        //we can just write out Console.WriteLine(v1) after this!
        public override string ToString()
        {
            return "(" + X + ", " + Y + ", " + Z + ")"; 
        }

        public Vector UnitVector(Vector v1)
        {
            Double mag = v1.Magnitude;
            Vector unitV = v1 / mag;
            return unitV;
        }

        public Double DotProduct(Vector v1, Vector v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        public Vector CrossProduct(Vector v1, Vector v2)
        {
            Vector crossV = new Vector(v1.Y * v2.Z - v1.Z * v2.Y, v1.Z*v2.X-v1.X*v2.Z, v1.X*v2.Y-v1.Y*v2.X);
            return crossV;
        }
    }
}

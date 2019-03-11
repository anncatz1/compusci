using DongUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thermodynamics
{
    /// <summary>
    /// A ParticleContainer in which the Particles interact with one another
    /// </summary>
    public class InteractingParticleContainer : SimpleParticleContainerGrid
    {
        private readonly double reactionRad;

        public InteractingParticleContainer(double xsize, double ysize, double zsize, double gridSizeX, double gridSizeY, double gridSizeZ, double reactionRad) :
            base(xsize, ysize, zsize, NDiv(xsize, gridSizeX), NDiv(ysize, gridSizeY), NDiv(zsize, gridSizeZ))
        {
            this.reactionRad = reactionRad;
        }
        
        /// <summary>
        /// Calculates the number of divisions based on the grid size
        /// </summary>
        static private int NDiv(double size, double gridSize)
        {
            return (int)Math.Round(size / gridSize);
        }

        /// <summary>
        /// A list of particles that have already been interacted with, to avoid double-counting
        /// </summary>
        private List<Particle> alreadyInteracted = new List<Particle>();

        protected override void Setup()
        {
            alreadyInteracted.Clear();
        }

        protected override void ParticleUpdate(Particle part)
        {
             foreach (var other in Nearby(part, reactionRad, alreadyInteracted))
             {
                Collide(part, other);
             }

            alreadyInteracted.Add(part);
        }

        /// <summary>
        /// Calculates what happens when two particles are within an interaction radius of each other
        /// </summary>
        private void Collide(Particle p1, Particle p2)
        {
            // Do your calculations here
            double changeInMomentum = Vector.Dot((p2.Momentum - p1.Momentum), (p2.Position - p1.Position));
            Vector changeMomentum = new Vector(changeInMomentum, changeInMomentum, changeInMomentum);
            p1.Momentum += changeMomentum;
            p2.Momentum += changeMomentum;
        }
    }
}

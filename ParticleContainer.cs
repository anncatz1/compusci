using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DongUtility;

namespace Thermodynamics
{
    /// <summary>
    /// A container class for holding gas particles in a cubical container
    /// </summary>
    public class ParticleContainer
    {
        /// <summary>
        /// The size of the container
        /// Each component of the vector is the size in that dimension
        /// </summary>
        public Vector Size { get; set; }
        /// <summary>
        /// All the particles in the container
        /// </summary>
        public List<Particle> Particles { get; } = new List<Particle>();

        /// <summary>
        /// A dictionary to keep track of all the particle types
        /// </summary>
        public ParticleDictionary Dictionary { get; } = new ParticleDictionary();

        /// <summary>
        /// Access to the random generator
        /// </summary>
        static protected Random Random { get { return RandomGenerator.RandomGen; } }

        public ParticleContainer(double xSize, double ySize, double zSize)
        {
            Size = new Vector(xSize, ySize, zSize);
        }

        /// <summary>
        /// Adds a particle directly
        /// Not for general use; hence, it is protected
        /// </summary>
        protected virtual void AddParticleDirectly(Particle part)
        {
            Particles.Add(part);
        }

        /// <summary>
        /// Gets the total number of particles of a given name
        /// </summary>
        public int GetNParticles(string name)
        {
            int total = 0;

            foreach (var part in Particles)
            {
                if (part.Info.Name == name)
                {
                    ++total;
                }
            }

            return total;
        }

        /// <summary>
        /// Adds a number of particles at random
        /// </summary>
        /// <param name="generator">The random generator to use</param>
        /// <param name="name">The name of the particle type</param>
        /// <param name="number">The number of particles to add</param>
        public void AddRandomParticles(RandomGenerator generator, string name, int number)
        {
            for (int i = 0; i < number; ++i)
            {
                AddParticleDirectly(generator.GetRandomParticle(name));
            }
        }

        /// <summary>
        /// Updates all particles for a given time increment
        /// </summary>
        public virtual void Update(double deltaTime)
        {
            foreach (Particle proj in Particles)
            {
                proj.Update(deltaTime);
                CheckParticle(proj);
            }
        }

        /// <summary>
        /// A function that extracts a specific property of a particle
        /// </summary>
        public delegate double ParticleFunction(Particle part);

        /// <summary>
        /// Get a list of values for a given property of all particles
        /// Useful for histograms
        /// </summary>
        public List<double> GetParticlePropertyList(ParticleFunction func)
        {
            var response = new List<double>();

            foreach (var part in Particles)
            {
                response.Add(func(part));
            }

            return response;
        }

        /// <summary>
        /// Make sure the particle lies within the bounds of the box
        /// Reflect it back if it is not
        /// </summary>
        private void CheckParticle(Particle particle)
        {
            Vector newVec = particle.Position;
            if (particle.Position.X < 0 || particle.Position.X > Size.X)
            {
                particle.Velocity = new Vector(-particle.Velocity.X, particle.Velocity.Y, particle.Velocity.Z);
                if (particle.Position.X < 0)
                {
                    newVec.X = 0;
                }
                if (particle.Position.X > Size.X)
                {
                    newVec.X = Size.X;
                }
            }
            if (particle.Position.Y < 0 || particle.Position.Y > Size.Y)
            {
                particle.Velocity = new Vector(particle.Velocity.X, -particle.Velocity.Y, particle.Velocity.Z);
                if (particle.Position.Y < 0)
                {
                    newVec.Y = 0;
                }
                if (particle.Position.Y > Size.Y)
                {
                    newVec.Y = Size.Y;
                }
            }
            if (particle.Position.Z < 0 || particle.Position.Z > Size.Z)
            {
                particle.Velocity = new Vector(particle.Velocity.X, particle.Velocity.Y, -particle.Velocity.Z);
                if (particle.Position.Z < 0)
                {
                    newVec.Z = 0;
                }
                if (particle.Position.Z > Size.Z)
                {
                    newVec.Z = Size.Z;
                }
            }
            particle.Position = newVec;          
        }
    }
}

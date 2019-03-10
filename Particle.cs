﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using DongUtility;

namespace Thermodynamics
{
    /// <summary>
    /// A single particle in the simulation
    /// </summary>
    public class Particle
    {
        public Vector Position { get; set; }
        public Vector Velocity { get; set; }
        /// <summary>
        /// Information about the particle type that is the same for all particles of that type
        /// </summary>
        public ParticleInfo Info { get; set; }

        public Particle(Vector position, Vector velocity, ParticleInfo info)
        {
            Position = position;
            Velocity = velocity;
            Info = info;
        }

        /// <summary>
        /// Change the particle position for a given time step
        /// </summary>
        virtual public void Update(double timeIncrement)
        {
            Position += Velocity * timeIncrement;
        }

    }
}

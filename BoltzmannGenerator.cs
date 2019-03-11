using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DongUtility;

namespace Thermodynamics
{
    public class BoltzmannGenerator : FunctionGenerator
    {
        private readonly double temperature;
        private readonly double mass;

        public BoltzmannGenerator(ParticleContainer cont, double temperature, ParticleInfo info,
            int projectedCalls = 10000, int nDivisions = 10000) :
            base(cont)
        {
            this.temperature = temperature;
            mass = info.Mass;
            Setup();
        }

        protected override double Function(double speed)
        {
            return 4 * Math.PI * Math.Pow(mass / (2 * Math.PI * Constants.BoltzmannConstant * temperature), 1.5)
                * speed * speed
                * Math.Exp(-mass * speed * speed / (2 * Constants.BoltzmannConstant * temperature));
        }
    }
}

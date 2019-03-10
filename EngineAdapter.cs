using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualizerControl;
using Projectile_Motion;
using System.IO;
using Utilities;

namespace Visualizer
{
    public class EngineAdapter : IEngine
    {
        private World engine = new World(0, new Vector(0,0,-9.8));

        public EngineAdapter(World engine)
        {
            this.engine = engine;
        }

        bool IEngine.Tick(double newTime)
        {
            double deltaTime = newTime - engine.Time;
            engine.Time += deltaTime;
            double totalTime = 40;
            engine.Tick(deltaTime);
            //if (engine.projectiles[0].Pos.Z >0)
            if (totalTime != newTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IList<IProjectile> GetProjectiles()
        {
            List<IProjectile> projectiles = new List<IProjectile>();
            foreach (var projectile in engine.Projectiles)
            {
                ProjectileAdapter proj = new ProjectileAdapter(projectile);
                projectiles.Add(proj);
            }
            return projectiles;
        }
    }
}

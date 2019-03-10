using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualizerControl;
using Projectile_Motion;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using VisualizerControl.Shapes;

namespace Visualizer
{
    public class ProjectileAdapter : IProjectile
    {
        private Projectile projectile;
        public ProjectileAdapter(Projectile projectile)
        {
            this.projectile = projectile;
        }

        public Vector3D Position => new Vector3D(projectile.Pos.X, projectile.Pos.Y, projectile.Pos.Z);

        public Color Color => Colors.AliceBlue;

        public Shape3D Shape => new Sphere3D();

        public double Size => 1;

    }
}

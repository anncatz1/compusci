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
using Utilities;

namespace Visualizer
{
    public class VectorAdapter
    {
        private Vector3D vector;
        public VectorAdapter(Vector vector)
        {
            this.vector = new Vector3D(vector.X, vector.Y, vector.Z);
        }
    }
}

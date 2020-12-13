using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharperMC.Core.Utils
{
    public class Ray2
    {
        /// Epsilon Yes very much epsilon lmfao
        public static readonly double EPSILON = 0.5;

        /// Offset
        public static readonly double OFFSET = 0.0001;

        /// Origin of the Ray
        public Vector3 x;

        /// Direction of the Ray
        public Vector3 d;

        /// Normal vector of intersection
        public Vector3 n = new Vector3();

        /// tNear variable
        public double tNear;

        /// t variable / parameter of ray
        public double t;

        /// Texture coordinate
        public double u;

        /// Texture coordinate
        public double v;
    }
}
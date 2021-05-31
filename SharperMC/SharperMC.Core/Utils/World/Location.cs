using System;

namespace SharperMC.Core.Utils.World
{
    public class Location
    {
        public readonly double X;
        public readonly double Y;
        public readonly double Z;

        public readonly float Yaw;
        public readonly float Pitch;
        
        public Location(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        
        public Location(double x, double y, double z, float yaw, float pitch)
        {
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
        }
        
        public Location(float yaw, float pitch)
        {
            Yaw = yaw;
            Pitch = pitch;
        }

        public double GetDistance(Location location)
        {
            return Math.Sqrt(Math.Pow(location.X, 2) - Math.Pow(X, 2) + Math.Pow(location.Y, 2) - Math.Pow(Y, 2) + Math.Pow(location.Z, 2) - Math.Pow(Z, 2));
        }
    }
}
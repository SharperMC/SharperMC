using System;

namespace SharperMC.Core.Utils.World.Vectors
{
    public struct Vector3D : IEquatable<Vector3D>
	{
		public readonly double X;
		public readonly double Y;
		public double Z;

		public Vector3D(double value)
		{
			X = Y = Z = value;
		}

		public Vector3D(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public Vector3D(Vector3D v)
		{
			X = v.X;
			Y = v.Y;
			Z = v.Z;
		}

		/// <summary>
		///     Finds the distance of this vector from Vector3D.Zero
		/// </summary>
		public double Distance
		{
			get { return DistanceTo(Zero); }
		}

		public bool Equals(Vector3D other)
		{
			return other.X.Equals(X) && other.Y.Equals(Y) && other.Z.Equals(Z);
		}

		/// <summary>
		///     Truncates the decimal component of each part of this Vector3D.
		/// </summary>
		public Vector3D Floor()
		{
			return new (Math.Floor(X), Math.Floor(Y), Math.Floor(Z));
		}

		public Vector3D Normalize()
		{
			return new (X/Distance, Y/Distance, Z/Distance);
		}

		/// <summary>
		///     Calculates the distance between two Vector3D objects.
		/// </summary>
		public double DistanceTo(Vector3D other)
		{
			return Math.Sqrt(Square(other.X - X) +
			                 Square(other.Y - Y) +
			                 Square(other.Z - Z));
		}

		/// <summary>
		///     Calculates the square of a num.
		/// </summary>
		private double Square(double num)
		{
			return num*num;
		}

		public static Vector3D Min(Vector3D value1, Vector3D value2)
		{
			return new (
				Math.Min(value1.X, value2.X),
				Math.Min(value1.Y, value2.Y),
				Math.Min(value1.Z, value2.Z)
				);
		}

		public static Vector3D Max(Vector3D value1, Vector3D value2)
		{
			return new (
				Math.Max(value1.X, value2.X),
				Math.Max(value1.Y, value2.Y),
				Math.Max(value1.Z, value2.Z)
				);
		}

		public static bool operator !=(Vector3D a, Vector3D b)
		{
			return !a.Equals(b);
		}

		public static bool operator ==(Vector3D a, Vector3D b)
		{
			return a.Equals(b);
		}

		public static Vector3D operator +(Vector3D a, Vector3D b)
		{
			return new (
				a.X + b.X,
				a.Y + b.Y,
				a.Z + b.Z);
		}

		public static Vector3D operator -(Vector3D a, Vector3D b)
		{
			return new (
				a.X - b.X,
				a.Y - b.Y,
				a.Z - b.Z);
		}

		public static Vector3D operator -(Vector3D a)
		{
			return new (
				-a.X,
				-a.Y,
				-a.Z);
		}

		public static Vector3D operator *(Vector3D a, Vector3D b)
		{
			return new (
				a.X*b.X,
				a.Y*b.Y,
				a.Z*b.Z);
		}

		public static Vector3D operator /(Vector3D a, Vector3D b)
		{
			return new (
				a.X/b.X,
				a.Y/b.Y,
				a.Z/b.Z);
		}

		public static Vector3D operator %(Vector3D a, Vector3D b)
		{
			return new (a.X%b.X, a.Y%b.Y, a.Z%b.Z);
		}

		public static Vector3D operator +(Vector3D a, double b)
		{
			return new (
				a.X + b,
				a.Y + b,
				a.Z + b);
		}

		public static Vector3D operator -(Vector3D a, double b)
		{
			return new (
				a.X - b,
				a.Y - b,
				a.Z - b);
		}

		public static Vector3D operator *(Vector3D a, double b)
		{
			return new (
				a.X*b,
				a.Y*b,
				a.Z*b);
		}

		public static Vector3D operator /(Vector3D a, double b)
		{
			return new (
				a.X/b,
				a.Y/b,
				a.Z/b);
		}

		public static Vector3D operator %(Vector3D a, double b)
		{
			return new (a.X%b, a.Y%b, a.Y%b);
		}

		public static Vector3D operator +(double a, Vector3D b)
		{
			return new (
				a + b.X,
				a + b.Y,
				a + b.Z);
		}

		public static Vector3D operator -(double a, Vector3D b)
		{
			return new (
				a - b.X,
				a - b.Y,
				a - b.Z);
		}

		public static Vector3D operator *(double a, Vector3D b)
		{
			return new (
				a*b.X,
				a*b.Y,
				a*b.Z);
		}

		public static Vector3D operator /(double a, Vector3D b)
		{
			return new (
				a/b.X,
				a/b.Y,
				a/b.Z);
		}

		public static Vector3D operator %(double a, Vector3D b)
		{
			return new (a%b.X, a%b.Y, a%b.Y);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (obj.GetType() != typeof (Vector3D)) return false;
			return Equals((Vector3D) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var result = X.GetHashCode();
				result = (result*397) ^ Y.GetHashCode();
				result = (result*397) ^ Z.GetHashCode();
				return result;
			}
		}

		public Location ToLocation()
		{
			return new(X, Y, Z);
		}

		public static readonly Vector3D Zero = new(0);
		public static readonly Vector3D One = new(1);

		public static readonly Vector3D Up = new(0, 1, 0);
		public static readonly Vector3D Down = new(0, -1, 0);
		public static readonly Vector3D Left = new(-1, 0, 0);
		public static readonly Vector3D Right = new(1, 0, 0);
		public static readonly Vector3D Backwards = new(0, 0, -1);
		public static readonly Vector3D Forwards = new(0, 0, 1);

		public static readonly Vector3D East = new(1, 0, 0);
		public static readonly Vector3D West = new(-1, 0, 0);
		public static readonly Vector3D North = new(0, 0, -1);
		public static readonly Vector3D South = new(0, 0, 1);
	}
}
// Distrubuted under the MIT license
// ===================================================
// SharperMC uses the permissive MIT license.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the “Software”), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software
// 
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// ©Copyright SharperMC - 2020

using System;

namespace SharperMC.Core.Utils
{
	public struct Ray : IEquatable<Ray>
	{
		#region Public Constructors

		public Ray(Vector3 position, Vector3 direction)
		{
			Position = position;
			Direction = direction;
		}

		#endregion

		#region Public Fields

		public readonly Vector3 Direction;
		public readonly Vector3 Position;

		#endregion

		#region Public Methods

		public override bool Equals(object obj)
		{
			return (obj is Ray) && Equals((Ray) obj);
		}


		public bool Equals(Ray other)
		{
			return Position.Equals(other.Position) && Direction.Equals(other.Direction);
		}


		public override int GetHashCode()
		{
			return Position.GetHashCode() ^ Direction.GetHashCode();
		}

		public double? Intersects(BoundingBox box)
		{
			//first test if start in box
			if (Position.X >= box.Min.X
			    && Position.X <= box.Max.X
			    && Position.Y >= box.Min.Y
			    && Position.Y <= box.Max.Y
			    && Position.Z >= box.Min.Z
			    && Position.Z <= box.Max.Z)
				return 0.0f; // here we concidere cube is full and origine is in cube so intersect at origine

			//Second we check each face
			var maxT = new Vector3(-1.0f);

			//calcul intersection with each faces
			if (Position.X < box.Min.X && Direction.X != 0.0f)
				maxT.X = (box.Min.X - Position.X)/Direction.X;
			else if (Position.X > box.Max.X && Direction.X != 0.0f)
				maxT.X = (box.Max.X - Position.X)/Direction.X;
			if (Position.Y < box.Min.Y && Direction.Y != 0.0f)
				maxT.Y = (box.Min.Y - Position.Y)/Direction.Y;
			else if (Position.Y > box.Max.Y && Direction.Y != 0.0f)
				maxT.Y = (box.Max.Y - Position.Y)/Direction.Y;
			if (Position.Z < box.Min.Z && Direction.Z != 0.0f)
				maxT.Z = (box.Min.Z - Position.Z)/Direction.Z;
			else if (Position.Z > box.Max.Z && Direction.Z != 0.0f)
				maxT.Z = (box.Max.Z - Position.Z)/Direction.Z;

			//get the maximum maxT
			if (maxT.X > maxT.Y && maxT.X > maxT.Z)
			{
				if (maxT.X < 0.0f)
					return null; // ray go on opposite of face
				//coordonate of hit point of face of cube
				var coord = Position.Z + maxT.X*Direction.Z;
				// if hit point coord ( intersect face with ray) is out of other plane coord it miss
				if (coord < box.Min.Z || coord > box.Max.Z)
					return null;
				coord = Position.Y + maxT.X*Direction.Y;
				if (coord < box.Min.Y || coord > box.Max.Y)
					return null;
				return maxT.X;
			}
			if (maxT.Y > maxT.X && maxT.Y > maxT.Z)
			{
				if (maxT.Y < 0.0f)
					return null; // ray go on opposite of face
				//coordonate of hit point of face of cube
				var coord = Position.Z + maxT.Y*Direction.Z;
				// if hit point coord ( intersect face with ray) is out of other plane coord it miss
				if (coord < box.Min.Z || coord > box.Max.Z)
					return null;
				coord = Position.X + maxT.Y*Direction.X;
				if (coord < box.Min.X || coord > box.Max.X)
					return null;
				return maxT.Y;
			}
			else //Z
			{
				if (maxT.Z < 0.0f)
					return null; // ray go on opposite of face
				//coordonate of hit point of face of cube
				var coord = Position.X + maxT.Z*Direction.X;
				// if hit point coord ( intersect face with ray) is out of other plane coord it miss
				if (coord < box.Min.X || coord > box.Max.X)
					return null;
				coord = Position.Y + maxT.Z*Direction.Y;
				if (coord < box.Min.Y || coord > box.Max.Y)
					return null;
				return maxT.Z;
			}
		}

		public static bool operator !=(Ray a, Ray b)
		{
			return !a.Equals(b);
		}

		public static bool operator ==(Ray a, Ray b)
		{
			return a.Equals(b);
		}

		public override string ToString()
		{
			return string.Format("{{Position:{0} Direction:{1}}}", Position, Direction);
		}

		#endregion
	}
}
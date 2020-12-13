﻿// Distrubuted under the MIT license
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
using System.Collections.Generic;

namespace SharperMC.Core.Utils
{
	public enum ContainmentType
	{
		Disjoint,
		Contains,
		Intersects
	}

	public struct BoundingBox : IEquatable<BoundingBox>
	{
		public const int CornerCount = 8;
		public Vector3 Max;
		public Vector3 Min;

		public BoundingBox(Vector3 min, Vector3 max)
		{
			Min = min;
			Max = max;
		}

		public BoundingBox(BoundingBox box)
		{
			Min = new Vector3(box.Min);
			Max = new Vector3(box.Max);
		}

		public double Height => Max.Y - Min.Y;

		public double Width => Max.X - Min.X;

		public double Depth => Max.Z - Min.Z;

		public bool Equals(BoundingBox other)
		{
			return (Min == other.Min) && (Max == other.Max);
		}

		public ContainmentType Contains(BoundingBox box)
		{
			//test if all corner is in the same side of a face by just checking min and max
			if (box.Max.X < Min.X
			    || box.Min.X > Max.X
			    || box.Max.Y < Min.Y
			    || box.Min.Y > Max.Y
			    || box.Max.Z < Min.Z
			    || box.Min.Z > Max.Z)
				return ContainmentType.Disjoint;


			if (box.Min.X >= Min.X
			    && box.Max.X <= Max.X
			    && box.Min.Y >= Min.Y
			    && box.Max.Y <= Max.Y
			    && box.Min.Z >= Min.Z
			    && box.Max.Z <= Max.Z)
				return ContainmentType.Contains;

			return ContainmentType.Intersects;
		}

		public bool Contains(Vector3 vec)
		{
			return Min.X <= vec.X && vec.X <= Max.X &&
			       Min.Y <= vec.Y && vec.Y <= Max.Y &&
			       Min.Z <= vec.Z && vec.Z <= Max.Z;
		}

		public static BoundingBox CreateFromPoints(IEnumerable<Vector3> points)
		{
			if (points == null)
				throw new ArgumentNullException();

			var empty = true;
			var vector2 = new Vector3(float.MaxValue);
			var vector1 = new Vector3(float.MinValue);
			foreach (var vector3 in points)
			{
				vector2 = Vector3.Min(vector2, vector3);
				vector1 = Vector3.Max(vector1, vector3);
				empty = false;
			}
			if (empty)
				throw new ArgumentException();

			return new BoundingBox(vector2, vector1);
		}

		public BoundingBox OffsetBy(Vector3 offset)
		{
			return new BoundingBox(Min + offset, Max + offset);
		}

		public Vector3[] GetCorners()
		{
			return new[]
			{
				new Vector3(Min.X, Max.Y, Max.Z),
				new Vector3(Max.X, Max.Y, Max.Z),
				new Vector3(Max.X, Min.Y, Max.Z),
				new Vector3(Min.X, Min.Y, Max.Z),
				new Vector3(Min.X, Max.Y, Min.Z),
				new Vector3(Max.X, Max.Y, Min.Z),
				new Vector3(Max.X, Min.Y, Min.Z),
				new Vector3(Min.X, Min.Y, Min.Z)
			};
		}

		public override bool Equals(object obj)
		{
			return (obj is BoundingBox) && Equals((BoundingBox) obj);
		}

		public override int GetHashCode()
		{
			return Min.GetHashCode() + Max.GetHashCode();
		}

		public bool Intersects(BoundingBox box)
		{
			Intersects(ref box, out var result);
			return result;
		}

		public void Intersects(ref BoundingBox box, out bool result)
		{
			if ((Max.X >= box.Min.X) && (Min.X <= box.Max.X))
			{
				if ((Max.Y < box.Min.Y) || (Min.Y > box.Max.Y))
				{
					result = false;
					return;
				}

				result = (Max.Z >= box.Min.Z) && (Min.Z <= box.Max.Z);
				return;
			}

			result = false;
		}

		public static BoundingBox operator +(BoundingBox a, double b)
		{
			return new BoundingBox(a.Min - b, a.Max + b);
		}

		public static bool operator ==(BoundingBox a, BoundingBox b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(BoundingBox a, BoundingBox b)
		{
			return !a.Equals(b);
		}

		public override string ToString()
		{
			return $"{{Min:{Min} Max:{Max}}}";
		}
	}
}
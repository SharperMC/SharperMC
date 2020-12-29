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
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SharperMC.Core.Permissions
{
    public class Permission
    {
        public static Type PermissibleType = typeof(PermissibleBase);

        public static IPermissible NewPermissible(Dictionary<string, string> dict)
        {
            var method = PermissibleType.GetMethod("Serialize");
            if (method == null) throw new Exception("No \"Serialize\" method in " + PermissibleType.Name);
            return (IPermissible) method.Invoke(null, new object[] {dict});
        }
        
        public string Name;
        public PermissionType Type;

        public Permission(string name, PermissionType type = PermissionType.Op)
        {
            Name = name.ToLower();
            Type = type;
        }

        public bool MatchesPermission(string name)
        {
            return Regex.IsMatch(Name, name);
        }

        public bool CheckForOp()
        {
            switch (Type)
            {
                case PermissionType.True:
                case PermissionType.False:
                    return false;
                case PermissionType.Op:
                case PermissionType.NotOp:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public bool GetValue(bool op)
        {
            switch (Type)
            {
                case PermissionType.True:
                    return true;
                case PermissionType.False:
                    return false;
                case PermissionType.Op:
                    return op;
                case PermissionType.NotOp:
                    return !op;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private sealed class NameTypeEqualityComparer : IEqualityComparer<Permission>
        {
            public bool Equals(Permission x, Permission y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Name == y.Name && x.Type == y.Type;
            }

            public int GetHashCode(Permission obj)
            {
                unchecked
                {
                    return ((obj.Name != null ? obj.Name.GetHashCode() : 0) * 397) ^ (int) obj.Type;
                }
            }
        }

        public static IEqualityComparer<Permission> NameTypeComparer { get; } = new NameTypeEqualityComparer();

        protected bool Equals(Permission other)
        {
            return Name == other.Name && Type == other.Type;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Permission) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (int) Type;
            }
        }

        public Permission Copy()
        {
            return new Permission(Name, Type);
        }
    }

    public enum PermissionType
    {
        True,
        False,
        Op,
        NotOp,
    }
}
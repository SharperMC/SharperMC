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
using System.Linq;

namespace SharperMC.Core.Permissions
{
    public class PermissibleBase : IPermissible
    {
        public static void Test()
        {
            var a = typeof(PermissibleBase);
            Activator.CreateInstance(a);
            var perm = new PermissibleBase();
            perm.SetPermission("no");
            Console.WriteLine(perm.HasPermission("no.*")); // true
            Console.WriteLine(perm.HasPermission("no u.*")); // false
            perm.SetPermission("no u haha");
            Console.WriteLine(perm.HasPermission("no u.*")); // true
            Console.WriteLine(perm.HasPermission("no me.*")); // false
            perm.SetOp(true);
            Console.WriteLine(perm.HasPermission("no me.*")); // true; op
            Console.WriteLine(perm.HasPermission(new Permission("no me.*", PermissionType.False))); // false
        }
        
        public List<Permission> Permissions = new List<Permission>();
        public bool Op;
        public virtual void SetOp(bool op)
        {
            Op = op;
        }

        public virtual bool IsOp()
        {
            return Op;
        }

        public virtual void RemovePermission(string permissionName)
        {
            Permissions.RemoveAll((perm) => perm.Name.Equals(permissionName, StringComparison.OrdinalIgnoreCase));
        }

        public virtual void RemovePermission(Permission permission)
        {
            permission.Name = permission.Name.ToLower(); 
            Permissions.RemoveAll((perm) => perm.Equals(permission));
        }

        public virtual void SetPermission(string permissionName)
        {
            SetPermission(new Permission(permissionName, PermissionType.True));
        }

        public virtual void SetPermission(Permission permission)
        {
            Permissions.Add(permission);
        }

        public virtual bool HasPermission(string permissionName)
        {
            return HasPermission(new Permission(permissionName));
        }

        public virtual bool HasPermission(Permission permission)
        { // todo: improve
            permission.Name = permission.Name.ToLower();
            var first = Permissions.FirstOrDefault(p => p.MatchesPermission(permission.Name));
            return first != null && first.GetValue(Op) || first == null && permission.GetValue(Op);
        }

        public virtual bool HasPermissionSet(string permissionName)
        {
            return Permissions.Any(p => p.Name.Equals(permissionName, StringComparison.OrdinalIgnoreCase));
        }

        public virtual bool HasPermissionSet(Permission permission)
        {
            permission.Name = permission.Name.ToLower();
            return Permissions.Any(p => p.Equals(permission));
        }

        public virtual bool AnyPermissionMatches(string permissionName)
        {
            permissionName = permissionName.ToLower();
            return Permissions.Any(p => p.MatchesPermission(permissionName));
        }

        public virtual Permission GetPermission(string permissionName)
        {
            return Permissions.FirstOrDefault(p => p.Name.Equals(permissionName, StringComparison.OrdinalIgnoreCase));
        }

        public virtual IEnumerable<Permission> GetMatchingPermissions(string match)
        {
            match = match.ToLower();
            return Permissions.FindAll(p => p.MatchesPermission(match));
        }

        public virtual IEnumerable<Permission> GetPermissions()
        {
            return Permissions.Select(perm => perm.Copy());
        }

        public Dictionary<string, string> Serialize()
        {
            return Permissions.ToDictionary(p => p.Name, p => p.Type.ToString());
        }

        static IPermissible Serialize(Dictionary<string, string> dict)
        {
            var p = new PermissibleBase();
            foreach (var pair in dict)
            {
                Enum.TryParse(pair.Value, true, out PermissionType type);
                p.SetPermission(new Permission(pair.Key, type));
            }

            return p;
        }
    }
}
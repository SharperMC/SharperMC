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

using System.Collections.Generic;

namespace SharperMC.Core.Permissions
{
    // Every implementation must have a 
    // static IPermissible Serialize(Dictionary<string, string> dict)
    public interface IPermissible
    {
        void SetOp(bool op);
        bool IsOp();
        void RemovePermission(string permissionName);
        void RemovePermission(Permission permission);
        void SetPermission(string permissionName);
        void SetPermission(Permission permission);
        bool HasPermission(string permissionName);
        bool HasPermission(Permission permission);
        bool HasPermissionSet(string permissionName);
        bool HasPermissionSet(Permission permission);
        bool AnyPermissionMatches(string permissionName);
        Permission GetPermission(string permissionName);
        IEnumerable<Permission> GetMatchingPermissions(string match);
        IEnumerable<Permission> GetPermissions();
        Dictionary<string, string> Serialize();
    }
}
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

namespace SharperMC.Core.Plugins
{
    public interface IPlugin
    {
        /// <summary>
        /// This will be called when the server first loads the plugin. It is only called once, even during reloads.
        ///
        /// You want to register your events here.
        /// </summary>
        void Load();
        /// <summary>
        /// This will be called every time the plugin gets enabled.
        ///
        /// You want to register your event listeners here.
        /// </summary>
        void Enable();
        /// <summary>
        /// This will be called every time the plugin gets disabled.
        /// </summary>
        void Disable();
        string GetName();
        string GetVersion();
        string GetAuthor();
        string GetDescription();
    }
}
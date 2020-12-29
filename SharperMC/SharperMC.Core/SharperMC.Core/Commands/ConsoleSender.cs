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
using SharperMC.Core.Enums;
using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Console;
using SharperMC.Core.Utils.Packets;
using SharperMC.Core.Utils.Text;

namespace SharperMC.Core.Commands
{
    public class ConsoleSender : ICommandSender
    { //todo: Move this in a better location & add color support
        public string GetName()
        {
            return "CONSOLE";
        }

        public bool IsPlayer()
        {
            return false;
        }

        public void SendChat(string message, params object[] args)
        {
            ConsoleFunctions.Write(TextUtils.ToChatText(TextUtils.Format(message, args)));
        }

        public void SendChat(ChatText message)
        {
            ConsoleFunctions.Write(message);
        }

        public void SendChat(string message, TextAttribute color)
        {
            ConsoleFunctions.WriteLine(message, color);
        }
    }
}
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

using Newtonsoft.Json;
using SharperMC.Core.Commands;
using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Client;
using SharperMC.Core.Utils.Console;
using SharperMC.Core.Utils.Misc;

namespace SharperMC.Core.Networking.Packets.Play
{
    public class TabComplete : Package<TabComplete>
    {
        // private string _text;
        // private bool _hasPosition;
        // private PlayerPosition _lookedAtBlock;
        public TabComplete(ClientWrapper client) : base(client)
        {
            ReadId = 0x14;
            SendId = 0x3A;
        }

        public TabComplete(ClientWrapper client, DataBuffer buffer) : base(client, buffer)
        {
            ReadId = 0x14;
            SendId = 0x3A;
        }

        /*
         * TODO: Tab completion but this is not very important at the moment
         */
        public override void Read()
        {
            var message = Buffer.ReadString();
            var hasPosition = Buffer.ReadBool();    
            // ConsoleFunctions.WriteInfoLine(""+message + " " + hasPosition);
            
            if (CommandManager.ShouldProcess(message))
            {
                CommandManager.ParseTab(Client.Player, message);
                return;
            }//else { player name list thingy UwU}
        }

        public override void Write()
        {
            // ConsoleFunctions.WriteInfoLine("a");
            //var message = JsonConvert.SerializeObject();
            Buffer.WriteVarInt(SendId);
            Buffer.WriteString("test");
            //Buffer.WriteString(message);
            Buffer.FlushData();
        }
    }
}
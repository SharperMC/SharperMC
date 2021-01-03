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
using SharperMC.Core.Enums;
using SharperMC.Core.Events;
using SharperMC.Core.Events.DefaultEvents;
using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Client;
using SharperMC.Core.Utils.Console;
using SharperMC.Core.Utils.Misc;
using SharperMC.Core.Utils.Packets;
using SharperMC.Core.Utils.Text;

namespace SharperMC.Core.Networking.Packets.Play
{
    public class ChatMessage : Package<ChatMessage>
    {
        public ChatText Message;
        public ChatMessageType MessageType = ChatMessageType.ChatBox;

        public ChatMessage(ClientWrapper client) : base(client)
        {
            ReadId = 0x01;
            SendId = 0x02;
        }

        public ChatMessage(ClientWrapper client, DataBuffer buffer) : base(client, buffer)
        {
            ReadId = 0x01;
            SendId = 0x02;
        }

        public override void Read()
        {
            var message = Buffer.ReadString();

            var preChatEvent = new PreChatEvent(message, CommandManager.ShouldProcess(message), Client.Player);
            EventManager.CallEvent(preChatEvent);
            if (preChatEvent.Cancelled) return;

            message = preChatEvent.Message;

            if (preChatEvent.ProcessAsCommand)
            {
                CommandManager.ParseCommand(Client.Player, message);
                return;
            }

            var chatEvent = new ChatEvent(message, "<{0}> {1}", Client.Player);
            EventManager.CallEvent(chatEvent);
            if (chatEvent.Cancelled) return;
            message = chatEvent.Message;

            var msg = string.Format(chatEvent.Format, Client.Player.GetName(), message); // Todo: Make this customizable

            Globals.ChatManager.BroadcastChat(msg);
            ConsoleFunctions.WriteInfoLine(msg);
        }

        public override void Write()
        {
            if (Buffer == null) return;
            var message = Message.Serialize();

            Buffer.WriteVarInt(SendId);
            //Buffer.WriteString("{ \"text\": \"" + Message + "\" }");
            Buffer.WriteString(message);
            Buffer.WriteByte((byte) MessageType);
            Buffer.FlushData();
        }
    }
}
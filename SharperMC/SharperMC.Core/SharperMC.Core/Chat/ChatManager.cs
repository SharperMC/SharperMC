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
using SharperMC.Core.Entity;
using SharperMC.Core.Enums;
using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Packets;
using SharperMC.Core.Utils.Text;

namespace SharperMC.Core.Chat
{
    public class ChatManager
    {
        public char Prefix { get; set; }
        public ChatManager()
        {
            Prefix = '/';
        }
        /// <summary>
        /// Prepares the message for chat.
        /// </summary>
        /// <param name="source">The player that send the message.</param>
        /// <param name="message">The message.</param>
        /// <returns>Formated chat message</returns>
        public virtual string FormatMessage(Player source, string message)
        {
            return $"<{source.Username}> {message}";
        }
        
        public void BroadcastChat(string message, Player sender = null)
        {
            BroadcastChat(new ChatText(message, TextColor.Reset), ChatMessageType.ChatBox, sender);
        }

        public void BroadcastChat(ChatText message, ChatMessageType type, Player sender)
        {
            foreach (var lvl in Globals.LevelManager.GetLevels())
            {
                lvl.BroadcastChat(message, type, sender);
            }
            Globals.LevelManager.MainLevel.BroadcastChat(message, type, sender);
        }
    }
}
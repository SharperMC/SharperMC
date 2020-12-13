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

using SharperMC.Core.Utils;

namespace SharperMC.Core.Networking.Packets.Play.Server
{
	internal class ClientSettings : Package<ClientSettings>
	{
		public ClientSettings(ClientWrapper client) : base(client)
		{
			ReadId = 0x15; //0x16
		}

		public ClientSettings(ClientWrapper client, DataBuffer buffer) : base(client, buffer)
		{
			ReadId = 0x15; //0x16
		}

		public override void Read()
		{
			var locale = Buffer.ReadString();
			var viewDistance = (byte) Buffer.ReadByte();
			var chatFlags = Buffer.ReadVarInt();
			var chatColours = Buffer.ReadBool();
			var skinParts = (byte) Buffer.ReadByte();
			// var mainHand = Buffer.ReadVarInt();
			
			Client.Player.Locale = locale;
			Client.Player.ViewDistance = viewDistance;
			Client.Player.ChatColours = chatColours;
			Client.Player.ChatFlags = chatFlags;
			Client.Player.SkinParts = skinParts;
			// Client.Player.MainHand = mainHand;
		}
	}
}
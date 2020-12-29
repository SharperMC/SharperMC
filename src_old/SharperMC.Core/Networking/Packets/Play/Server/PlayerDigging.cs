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

using SharperMC.Core.Blocks;
using SharperMC.Core.Entity;
using SharperMC.Core.Enums;
using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Client;
using SharperMC.Core.Utils.Entities.Player;
using SharperMC.Core.Utils.Misc;

namespace SharperMC.Core.Networking.Packets.Play.Server
{
	internal class PlayerDigging : Package<PlayerDigging>
	{
		public PlayerDigging(ClientWrapper client) : base(client)
		{
			ReadId = 0x07;
		}

		public PlayerDigging(ClientWrapper client, DataBuffer buffer) : base(client, buffer)
		{
			ReadId = 0x07;
		}

		public override void Read()
		{
			if (Buffer != null)
			{
				var status = Buffer.ReadByte();
				var position = Buffer.ReadPosition();
				var face = Buffer.ReadByte();

				switch (status)
				{
					case 0:
						Client.Player.Digging = true;
						break;
					case 1:
						Client.Player.Digging = false;
						break;
					case 2:
						Block block = Client.Player.Level.GetBlock(position);
						block.BreakBlock(Client.Player.Level);
						Client.Player.Digging = false;
						if (Client.Player.Gamemode != Gamemode.Creative)
						{
							foreach (var its in block.Drops)
							{
								new ItemEntity(Client.Player.Level, its)
								{
									KnownPosition = new PlayerLocation(position.X, position.Y, position.Z)
								}.SpawnEntity();
							}
						}
						break;
					case 3:
						Client.Player.Inventory.DropCurrentItemStack();
						break;
					case 4:
						Client.Player.Inventory.DropCurrentItem();
						break;
					case 5:
						/*
						 * TODO: Shoot Arrow / Finish eating https://wiki.vg/index.php?title=Protocol&oldid=7368#Player_Digging 
						 */
						break;
				}
			}
		}
	}
}
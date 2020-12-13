﻿using System;
using System.Linq;
using System.Net;
using System.Text;
using SharperMC.Core.Entity;
using SharperMC.Core.Networking.Packets.Login.Client;
using SharperMC.Core.Networking.Packets.Play.Client;
using SharperMC.Core.Utils;

namespace SharperMC.Core.Networking.Packets.Login.Server
{
	public class LoginStart : Package<LoginStart>
	{
		public LoginStart(ClientWrapper client) : base(client)
		{
			ReadId = 0x00;
		}

		public LoginStart(ClientWrapper client, DataBuffer buffer) : base(client, buffer)
		{
			ReadId = 0x00;
		}

		public override void Read()
		{
			if (Buffer != null)
			{
				var usernameRaw = Buffer.ReadString();
				var username =
					new string(usernameRaw.Where(c => char.IsLetter(c) || char.IsPunctuation(c) || char.IsDigit(c)).ToArray());
				
				var uuid = GetUuid(username);
				if (ServerSettings.OnlineMode)
				{
					if (ServerSettings.EncryptionEnabled)
					{
						Client.PacketMode = PacketMode.Login;
						Client.Username = username;
						new EncryptionRequest(Client)
						{
							PublicKey = PacketCryptography.PublicKeyToAsn1(Globals.ServerKey),
							VerificationToken = PacketCryptography.GetRandomToken()
						}.Write();
						return;
					}
					if (!Client.Player.IsAuthenticated())
					{
						new Disconnect(Client) { Reason = new McChatMessage("§fAuthentication failed!") }.Write();
						return;
					}
				}

				if (Encoding.UTF8.GetBytes(username).Length == 0)
				{
					new Disconnect(Client) { Reason = new McChatMessage("§fSomething went wrong while decoding your username!") }.Write();
					return;
				}

				//Protocol check!
				if (Client.Protocol < Globals.ProtocolVersion) //Protocol to old?
				{
					new Disconnect(Client) { Reason = new McChatMessage("§fYour Minecraft version is too old!\nPlease update in order to play!") }
						.Write();
					return;
				}

				if (Client.Protocol > Globals.ProtocolVersion) //Protocol to new?
				{
					new Disconnect(Client)
					{
						Reason =
							new McChatMessage("§fThis server is not yet updated for your version of Minecraft!\nIn order to play you have to use " +
							Globals.OfficialProtocolName)
					}.Write();
					return;
				}

				new LoginSucces(Client) { Username = username, Uuid = uuid }.Write();

				Client.Player = new Player(Globals.LevelManager.MainLevel)
				{
					Uuid = uuid,
					Username = username,
					Wrapper = Client,
					Gamemode = Globals.LevelManager.MainLevel.DefaultGamemode
				};
				Client.PacketMode = PacketMode.Play;

				new SetCompression(Client)
				{
					CompressionLevel = ServerSettings.UseCompression ? ServerSettings.CompressionThreshold : -1
				}.Write();

				new JoinGame(Client) { Player = Client.Player }.Write();
				new SpawnPosition(Client).Write();
				Client.Player.InitializePlayer();
			}
		}

		private string GetUuid(string username)
		{
			try
			{
				var wc = new WebClient();
				var result = wc.DownloadString("https://api.mojang.com/users/profiles/minecraft/" + username);
				var _result = result.Split('"');
				if (_result.Length > 1)
				{
					var uuid = _result[3];
					return new Guid(uuid).ToString();
				}
				return Guid.NewGuid().ToString();
			}
			catch
			{
				return Guid.NewGuid().ToString();
			}
		}
	}
}

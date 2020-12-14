﻿using System;
using System.Collections.Generic;
using SharperMC.Core.Utils;

namespace SharperMC.Core.PluginChannel
{
	public class MessageFactory
	{
		private Dictionary<string, PluginMessage> Messages { get; set; }

		public MessageFactory()
		{
			Messages = new Dictionary<string, PluginMessage>()
			{
				{"MC", new BrandMessage() }
			};
		}

		public bool AddMessage(PluginMessage pm, string channel)
		{
			if (Messages.ContainsKey(channel)) return false;
			Messages.Add(channel, pm);
			return true;
		}

		public bool HandleMessage(ClientWrapper client, DataBuffer buffer)
		{
			string raw = buffer.ReadString();
			Console.WriteLine(raw);
			string channel = raw.Split('|')[0];
			string command = raw.Split('|')[1];

			foreach (var msg in Messages)
			{
				if (msg.Key == channel)
				{
					if (msg.Value.Command == command)
					{
						msg.Value.HandleData(client, buffer);
					}
					return true;
				}
			}
			return false;
		}
	}
}

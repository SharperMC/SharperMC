namespace SharperMC.Core.Utils.Packets
{
	public class StatusRequestMessage
	{
		public StatusRequestMessage(string version, int protocol, int maxPlayers, int onlinePlayers, string description)
		{
			this.version = new StatusVersionClass(version, protocol);
			this.players = new StatusPlayersClass(maxPlayers, onlinePlayers);
			this.description = new StatusDescriptionClass(description);
		}

		public StatusVersionClass version;
		public StatusPlayersClass players;
		public StatusDescriptionClass description;
	}

	public class StatusVersionClass
	{
		public StatusVersionClass(string name, int protocol)
		{
			this.name = name;
			this.protocol = protocol;
		}
		public string name;
		public int protocol = 0;
	}

	public class StatusPlayersClass
	{
		public StatusPlayersClass(int max, int online)
		{
			this.max = max;
			this.online = online;
		}
		public int max;
		public int online;
	}
	
	public class StatusDescriptionClass
	{
		public StatusDescriptionClass(string text)
		{
			this.text = text;
		}
		public string text;
	}
}

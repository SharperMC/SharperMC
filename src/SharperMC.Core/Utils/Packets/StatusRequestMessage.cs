namespace SharperMC.Core.Utils
{
	public class StatusRequestMessage
	{
		public StatusRequestMessage(string version, int protocol, int maxPlayers, int onlinePlayers, string description)
		{
			Version = new StatusVersionClass(version, protocol);
			Players = new StatusPlayersClass(maxPlayers, onlinePlayers);
			Description = new McChatMessage(description);
		}

		public StatusVersionClass Version;
		public StatusPlayersClass Players;
		public McChatMessage Description;
	}

	public class StatusVersionClass
	{
		public StatusVersionClass(string name, int protocol)
		{
			Name = name;
			Protocol = protocol;
		}

		public string Name;
		public int Protocol = 0;
	}

	public class StatusPlayersClass
	{
		public StatusPlayersClass(int max, int online)
		{
			Max = max;
			Online = online;
		}

		public int Max;
		public int Online;
	}
}

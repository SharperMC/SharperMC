using SharperMC.Core.Enums;

namespace SharperMC.Core.Config
{
	/// <summary>
	/// Server settings
	/// </summary>
	public class ServerSettings
	{
		public int Port = 25565;
		
		public bool UseCompression = false;
		public int CompressionThreshold = 64;
		
		public string Seed = "SharpMC";
		public string WorldType = "standard";
		public string WorldName = "world";
		
		public string Motd = "A Minecraft server!";
		public bool OnlineMode = false;
		public bool EncryptionEnabled = true;
		public int MaxPlayers = 10;

		public Gamemode Gamemode = Gamemode.Survival;
		
		public bool DisplayPacketErrors = false;
		public bool Debug = false;
		
		internal bool ReportExceptionsToClient = true;
	}
}

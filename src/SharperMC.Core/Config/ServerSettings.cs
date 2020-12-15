namespace SharperMC.Core.Config
{
	/// <summary>
	/// Server settings
	/// </summary>
	public class ServerSettings
	{
		internal static string Seed = "default";
		
		internal static bool UseCompression = false;
		internal static int CompressionThreshold = 64; // [Default Int: 999999999 ORIGINAL SHARPMC VALUE] (This should not be 999... causes extreme issues should be 512, 128, 64, -1
		
		internal static bool OnlineMode = true;
		
		internal static bool EncryptionEnabled = true; //Should be enabled & unchangeable!
		internal static int MaxPlayers = 10;
		
		public static bool DisplayPacketErrors = true;
		public static bool Debug = true;
		public static string Motd = "";
		public static bool ReportExceptionsToClient = true;
	}
}

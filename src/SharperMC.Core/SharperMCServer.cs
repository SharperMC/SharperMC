using System;
using System.IO;
using System.Threading;
using SharperMC.Core.Chat;
using SharperMC.Core.Networking;
using SharperMC.Core.PluginChannel;
using SharperMC.Core.Utils;
using SharperMC.Core.Worlds;
using SharperMC.Core.Worlds.Anvil;
using SharperMC.Core.Worlds.Flatland;
using SharperMC.Core.Worlds.Nether;
using SharperMC.Core.Worlds.Standard;

namespace SharperMC.Core
{
	/*
	 * TODO: Cry
	 */
	public class SharperMCServer
	{
		private bool _initiated = false;
		public SharperMCServer()
		{
			ConsoleFunctions.WriteInfoLine(string.Format("Initiating {0}", Globals.ProtocolName));

			ConsoleFunctions.WriteInfoLine("Enabling global error handling...");
			var currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException += UnhandledException;

			ConsoleFunctions.WriteInfoLine("Loading settings...");
			LoadSettings();

			ConsoleFunctions.WriteInfoLine("Loading variables...");
			InitiateVariables();

			ConsoleFunctions.WriteInfoLine("Checking files and directories...");
			CheckDirectoriesAndFiles();

			_initiated = true;
		}

		public void StartServer()
		{
			if (!_initiated) throw new Exception("Server not initated!");

			Console.CancelKeyPress += ConsoleOnCancelKeyPress;

			try
			{
				new Thread(() => Globals.ServerListener.StartListening()).Start();
				new Thread(() => ConsoleCommandHandler.WaitForCommand()).Start();
			}
			catch (Exception ex)
			{
				UnhandledException(this, new UnhandledExceptionEventArgs(ex, false));
			}
		}

		/*
		 * TODO: Change entirely
		 */
		private void LoadSettings()
		{
			Config.ConfigFile = "server.properties";
			Config.InitialValue = new[]
			{
				"#DO NOT REMOVE THIS LINE - SharpMC Config",
				"Port=25515",
				"MaxPlayers=10",
				"LevelType=standard",
				"WorldName=world",
				"Online-mode=false",
				"Seed=SharpMC",
				"Motd=A SharpMC Powered Server"
			};
			Config.Check();
		}

		private void InitiateVariables()
		{
			Globals.Rand = new Random();
			Console.Title = Globals.ProtocolName;
			ServerSettings.Debug = Config.GetProperty("debug", false);
			ServerSettings.DisplayPacketErrors = Config.GetProperty("ShowNetworkErrors", false);
			ServerSettings.MaxPlayers = Config.GetProperty("MaxPlayers", 10);
			ServerSettings.Seed = Config.GetProperty("Seed", "SharpMC");
			ServerSettings.Motd = Config.GetProperty("motd", "A SharpMC Powered Server");

			Globals.LevelManager = new LevelManager(LoadLevel());
			Globals.LevelManager.AddLevel("nether", new NetherLevel("nether"));
			ServerSettings.OnlineMode = Config.GetProperty("Online-mode", false);

			Globals.ChatManager = new ChatManager();
			
			Globals.ServerKey = PacketCryptography.GenerateKeyPair();

			Globals.ClientManager = new ClientManager();

			Globals.MessageFactory = new MessageFactory();

			Globals.ServerListener = new BasicListener();
		}

		private Level LoadLevel()
		{
			var lvltype = Config.GetProperty("LevelType", "standard");
			Level lvl;
			switch (lvltype.ToLower())
			{
				case "flatland":
					lvl = new FlatLandLevel(Config.GetProperty("WorldName", "world"));
					break;
				case "anvil":
					lvl = new AnvilLevel(Config.GetProperty("WorldName", "world"));
					break;
				default:
					lvl = new StandardLevel(Config.GetProperty("WorldName", "world"));
					break;
			}
			return lvl;
		}

		private void CheckDirectoriesAndFiles()
		{
			if (!Directory.Exists(Globals.LevelManager.MainLevel.LvlName))
				Directory.CreateDirectory(Globals.LevelManager.MainLevel.LvlName);
			
			if (!Directory.Exists("Players"))
				Directory.CreateDirectory("Players");
		}

		private static void UnhandledException(object sender, UnhandledExceptionEventArgs args)
		{
			var e = (Exception)args.ExceptionObject;
			ConsoleFunctions.WriteErrorLine("An unhandled exception occured! Error message: " + e.Message);
		}

		private void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs consoleCancelEventArgs)
		{
			Globals.StopServer();
		}
	}
}

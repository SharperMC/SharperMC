using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using SharperMC.Core.Chat;
using SharperMC.Core.Config;
using SharperMC.Core.Networking;
using SharperMC.Core.PluginChannel;
using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Console;
using SharperMC.Core.Utils.Security;
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
		public string CurrentDirectory;
		public static DateTime StartTime;
		
		public SharperMCServer()
		{
			ConsoleFunctions.ClearConsole();
			ConsoleFunctions.WriteInfoLine("Initiating server on {0}", true, Globals.ProtocolName);
			CurrentDirectory = Directory.GetCurrentDirectory();
			var currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException += UnhandledException;
			ConsoleFunctions.WriteInfoLine("Enabling global error handling... ", false);
			ConsoleFunctions.WriteLine("Enabled.", ConsoleColor.Green);
			ConsoleFunctions.WriteInfoLine("Checking if server properties exist... ", false);
			ConsoleFunctions.WriteLine(LoadSettings() ? "Loading." : "Created.", ConsoleColor.Green);
			ConsoleFunctions.WriteInfoLine("Loading server variables... ", false);
			ConsoleFunctions.WriteLine("Loaded.", ConsoleColor.Green);
			InitiateVariables();
			
			ConsoleFunctions.WriteInfoLine("Checking files and directories... ", false);
			CheckDirectoriesAndFiles();
			ConsoleFunctions.WriteLine("Files are good :)", ConsoleColor.Green);
			_initiated = true;
		}

		
		public void StartServer()
		{
			if (!_initiated) throw new Exception("Server not initiated!");
			Console.CancelKeyPress += ConsoleOnCancelKeyPress;
			StartTime = DateTime.UtcNow;
		
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
		
		private bool LoadSettings()
		{
			ConfigManager.ConfigFile = "server.properties";
			ConfigManager.InitialValue = new[]
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
			if (ConfigManager.Check())
				return true;
			return false;
		}
		
		
		private void InitiateVariables()
		{
			Globals.Rand = new Random();
			Console.Title = Globals.ProtocolName;
			ServerSettings.Debug = ConfigManager.GetProperty("debug", false);
			ServerSettings.DisplayPacketErrors = ConfigManager.GetProperty("ShowNetworkErrors", false);
			ServerSettings.MaxPlayers = ConfigManager.GetProperty("MaxPlayers", 10);
			ServerSettings.Seed = ConfigManager.GetProperty("Seed", "SharpMC");
			ServerSettings.Motd = ConfigManager.GetProperty("motd", "A SharpMC Powered Server");
		
			Globals.LevelManager = new LevelManager(LoadLevel());
			Globals.LevelManager.AddLevel("nether", new NetherLevel("nether"));
			ServerSettings.OnlineMode = ConfigManager.GetProperty("Online-mode", false);
		
			Globals.ChatManager = new ChatManager();
			Globals.ServerKey = PacketCryptography.GenerateKeyPair();
			Globals.ClientManager = new ClientManager();
			Globals.MessageFactory = new MessageFactory();
			Globals.ServerListener = new BasicListener();
		}
		
		private Level LoadLevel()
		{
			var lvltype = ConfigManager.GetProperty("LevelType", "standard");
			Level lvl;
			switch (lvltype.ToLower())
			{
				case "flatland":
					lvl = new FlatLandLevel(ConfigManager.GetProperty("WorldName", "world"));
					break;
				case "anvil":
					lvl = new AnvilLevel(ConfigManager.GetProperty("WorldName", "world"));
					break;
				default:
					lvl = new StandardLevel(ConfigManager.GetProperty("WorldName", "world"));
					break;
			}
			return lvl;
		}
		
		private void CheckDirectoriesAndFiles()
		{
			if (!Directory.Exists("Logs"))
				Directory.CreateDirectory("Logs");
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

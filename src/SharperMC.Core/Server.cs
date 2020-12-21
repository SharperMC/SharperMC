using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using SharperMC.Core.Chat;
using SharperMC.Core.Config;
using SharperMC.Core.Entity;
using SharperMC.Core.Enums;
using SharperMC.Core.Networking;
using SharperMC.Core.Networking.Packets.Login.Client;
using SharperMC.Core.PluginChannel;
using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Console;
using SharperMC.Core.Utils.Packets;
using SharperMC.Core.Utils.Security;
using SharperMC.Core.Worlds;
using SharperMC.Core.Worlds.Anvil;
using SharperMC.Core.Worlds.Flatland;
using SharperMC.Core.Worlds.Nether;
using SharperMC.Core.Worlds.Standard;

namespace SharperMC.Core
{
	public class Server
	{
		private readonly bool _initiated;
		public static string CurrentDirectory;
		public static DateTime StartTime;
		public static ConfigManager ConfigManager;
		public static ServerSettings ServerSettings;

		public Server()
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
			
			ConsoleFunctions.WriteInfoLine("Checking files and directories... ", false);
			CheckDirectoriesAndFiles();
			ConsoleFunctions.WriteLine("Files are good hopefully.", ConsoleColor.Green);
			_initiated = true;
		}

		
		public void StartServer()
		{
			if (!_initiated) throw new Exception("Server not initiated!");
			Console.CancelKeyPress += ConsoleOnCancelKeyPress;
			StartTime = DateTime.UtcNow;
		
			try
			{
				new Thread(Globals.ServerListener.StartListening).Start();
				new Thread(ConsoleCommandHandler.WaitForCommand).Start();
			}
			catch (Exception ex)
			{
				UnhandledException(this, new UnhandledExceptionEventArgs(ex, false));
			}
		}
		
		public static void StopServer(string stopMsg = "Server shutting down...")
		{
			ConsoleFunctions.WriteInfoLine("Shutting down...");
			Globals.BroadcastPacket(new Disconnect(null) { Reason = new McChatMessage("§f" + stopMsg) });
			ConsoleFunctions.WriteInfoLine("Saving all player data...");
			foreach (Player player in Globals.LevelManager.GetAllPlayers())
			{
				player.SavePlayer();
			}
			ConsoleFunctions.WriteInfoLine("Saving chunks...");
			Globals.LevelManager.SaveAllChunks();
			Globals.ServerListener.StopListenening();
			Environment.Exit(0);
		}
		
		private bool LoadSettings()
		{
			ServerSettings = new ServerSettings();
			ConfigManager = new ConfigManager("server.properties");
			LoadServerSettingsFromFile();
			InitiateVariables();
			if (ConfigManager.ConfigExists())
				return true;
			return false;
		}

		public static void LoadServerSettingsFromFile()
		{
			ServerSettings.Seed = ConfigManager.GetProperty("Seed", "SharpMC");
			ServerSettings.Port = ConfigManager.GetProperty("port", 25565);
			
			ServerSettings.UseCompression = ConfigManager.GetProperty("UseCompression", false);
			ServerSettings.CompressionThreshold = ConfigManager.GetProperty("CompressionThreshold", 64);

			ServerSettings.WorldType = ConfigManager.GetProperty("WorldType", "standard");
			ServerSettings.WorldName = ConfigManager.GetProperty("WorldName", "world");

			ServerSettings.Motd = ConfigManager.GetProperty("Motd", "A SharpMC Powered Server");
			ServerSettings.OnlineMode = ConfigManager.GetProperty("OnlineMode", false);
			ServerSettings.EncryptionEnabled = ConfigManager.GetProperty("EncryptionEnabled", true);
			ServerSettings.MaxPlayers = ConfigManager.GetProperty("MaxPlayers", 10);
			ServerSettings.Gamemode = ConfigManager.GetProperty("Gamemode", Gamemode.Survival);
			ServerSettings.DisplayPacketErrors = ConfigManager.GetProperty("DisplayPacketErrors", false);
			ServerSettings.Debug = ConfigManager.GetProperty("Debug", false);
		}

		private void InitiateVariables()
		{
			Console.Title = Globals.ProtocolName;
		
			Globals.LevelManager = new LevelManager(LoadLevel());
			Globals.LevelManager.AddLevel("nether", new NetherLevel("nether"));
			Globals.ChatManager = new ChatManager();
			Globals.ServerKey = PacketCryptography.GenerateKeyPair();
			Globals.ClientManager = new ClientManager();
			Globals.MessageFactory = new MessageFactory();
			Globals.ServerListener = new ClientListener();
		}
		
		private Level LoadLevel()
		{
			switch (ServerSettings.WorldType.ToLower())
			{
				case "flatland":
					return new FlatLandLevel(ConfigManager.GetProperty("WorldName", "world"));
				case "anvil":
					return new AnvilLevel(ConfigManager.GetProperty("WorldName", "world"));
				default:
					return new StandardLevel(ConfigManager.GetProperty("WorldName", "world"));
			}
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
			StopServer();
		}
	}
}

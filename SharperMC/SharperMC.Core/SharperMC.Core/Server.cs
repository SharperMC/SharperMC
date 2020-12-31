using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using SharperMC.Core.Chat;
using SharperMC.Core.Config;
using SharperMC.Core.Entity;
using SharperMC.Core.Enums;
using SharperMC.Core.Networking;
using SharperMC.Core.Networking.Packets.Login.Client;
using SharperMC.Core.Networking.Packets.Play.Client;
using SharperMC.Core.PluginChannel;
using SharperMC.Core.Plugins;
using SharperMC.Core.Utils.Console;
using SharperMC.Core.Utils.Packets;
using SharperMC.Core.Utils.Security;
using SharperMC.Core.Utils.Text;
using SharperMC.Core.Worlds;
using SharperMC.Core.Worlds.Anvil;
using SharperMC.Core.Worlds.Flatland;
using SharperMC.Core.Worlds.Nether;
using SharperMC.Core.Worlds.Standard;

namespace SharperMC.Core
{
    public class Server
    {
        public static bool Initiated { get; private set; }
        public static string CurrentDirectory;
        public static DateTime StartTime;
        public static ConfigManager ConfigManager;
        public static ServerSettings ServerSettings;

        public Server(string[] args)
        {
            GuiApp.Setup(args);
            ConsoleFunctions.ClearConsole();

            if (!args.Contains("--minimal"))
                for (var i = 0; i < 10; i++)
                    ConsoleFunctions.Write(
                        TextUtils.ToChatText(
                            "\u00A74\u00A7l[ETHO]\u00A7r \u00A7l\u00A7nUSE\u00A77 --minimal\u00A7r \u00A7l\u00A7nIN PROGRAM ARGUMENTS TO FIX ASYNC WRITING OR DEAL W/ IT")
                    );
            ConsoleFunctions.WriteInfoLine("For some reason, writing async in non-minimal console doesn't work.");
            ConsoleFunctions.WriteInfoLine("For some reason, the server doesn't close all threads. Just kill it for now.");
            ConsoleFunctions.WriteInfoLine("Loading plugins...");
            PluginManager.RegisterPlugins();

            ConsoleFunctions.WriteInfoLine("Initiating server on {0}", Globals.ProtocolName);
            CurrentDirectory = Directory.GetCurrentDirectory();
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += UnhandledException;

            ConsoleFunctions.Pause();
            ConsoleFunctions.WriteInfoLine("Enabling global error handling... ");
            ConsoleFunctions.WriteLine("Enabled.", ConsoleColor.Green);
            ConsoleFunctions.Continue();

            ConsoleFunctions.Pause();
            ConsoleFunctions.WriteInfoLine("Checking if server properties exist... ");
            ConsoleFunctions.WriteLine(LoadSettings() ? "Loading." : "Created.", ConsoleColor.Green);
            ConsoleFunctions.Continue();

            ConsoleFunctions.Pause();
            ConsoleFunctions.WriteInfoLine("Loading server variables... ");
            ConsoleFunctions.WriteLine("Loaded.", ConsoleColor.Green);
            ConsoleFunctions.Continue();

            ConsoleFunctions.Pause();
            ConsoleFunctions.WriteInfoLine("Checking files and directories... ");
            CheckDirectoriesAndFiles();
            ConsoleFunctions.WriteLine("Files are good hopefully.", ConsoleColor.Green);
            ConsoleFunctions.Continue();

            Initiated = true;
        }

        public void StartServer()
        {
            if (!Initiated) throw new Exception("Server not initiated!");
            Console.CancelKeyPress += ConsoleOnCancelKeyPress;
            StartTime = DateTime.UtcNow;

            ConsoleFunctions.WriteInfoLine("Enabling plugins...");
            PluginManager.EnableAllPlugins();
            ConsoleFunctions.WriteInfoLine("Enabled plugins.");

            try
            {
                new Thread(Globals.ServerListener.StartListening).Start();
                new Thread(GuiApp.Start).Start();
            }
            catch (Exception ex)
            {
                UnhandledException(this, new UnhandledExceptionEventArgs(ex, false));
            }
        }

        private static bool ShutDown;
        
        public static void StopServer(string stopMsg = "Server shutting down...")
        {
            if (ShutDown) return;
            ShutDown = true;
            ConsoleFunctions.WriteInfoLine("Shutting down...");
            Globals.BroadcastPacket(new Disconnect(null) {Reason = new ChatText(stopMsg, TextColor.Reset)});

            ConsoleFunctions.WriteInfoLine("Disabling all plugins...");
            PluginManager.DisableAllPlugins();

            ConsoleFunctions.WriteInfoLine("Saving all player data...");
            foreach (Player player in Globals.LevelManager.GetAllPlayers())
            {
                player.SavePlayer();
            }

            ConsoleFunctions.WriteInfoLine("Saving chunks...");
            Globals.LevelManager.SaveAllChunks();
            ConsoleFunctions.WriteInfoLine("Stopping listening...");
            //todo:
            ConsoleFunctions.WriteInfoLine("For some reason, the app decides to keep existing. Press Ctrl+C after a second. This will be fixed soon (hopefully).");
            Globals.ServerListener.StopListenening();
            Environment.Exit(0);
        }

        private bool LoadSettings()
        {
            ServerSettings = new ServerSettings();
            ConfigManager = new ConfigManager("server.properties");
            LoadServerSettingsFromFile();
            InitiateVariables();
            return ConfigManager.ConfigExists();
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
            ServerSettings.ReducedDebugInfo = ConfigManager.GetProperty("ReducedDebugInfo", false);
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
            var e = (Exception) args.ExceptionObject;
            ConsoleFunctions.WriteErrorLine("An unhandled exception occured! Error message: " + e.Message);
        }

        private void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs consoleCancelEventArgs)
        {
            StopServer();
        }
    }
}
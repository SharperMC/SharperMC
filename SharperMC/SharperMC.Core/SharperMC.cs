using System;
using System.Linq;

namespace SharperMC.Core
{
    public class SharperMC
    {
        public static SharperMC Instance;
        public Server Server { get; }

        public SharperMC(string[] args)
        {
            Instance = this;
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            Console.CancelKeyPress += ConsoleOnCancelKeyPress;
            Server = new Server();

            Console.Title = Server.ServerSettings.ProtocolName;
            
            Console.WriteLine("Initiating {0} Server! | " + (Server.ServerSettings.ProtocolVersions.Length == 0 ? "All Supported Protocols! " + $"({String.Join(", ", Server.ServerSettings.SupportedVersions().ToArray())})" + " " : "Allowed Protocol" + (Server.ServerSettings.ProtocolVersions.Length > 1 ? "s" : String.Empty) + $" ({string.Join(", ", Server.ServerSettings.ProtocolVersions)})"), Server.ServerSettings.ProtocolName);
        }

        public static void UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Console.WriteLine("An unhandled exception occured! Error message: " + ((Exception)args.ExceptionObject).Message);
        }
        
        private void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs consoleCancelEventArgs)
        {
            if(Server != null)
                Server.StopServer();
            else
                Environment.Exit(0);
        }
    }
}
using System;
using System.Threading;
using SharperMC.Core.Networking;

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

            Console.Title = Server.ProtocolName;
            Console.WriteLine("Initiating server on {0}, Protocol {1}", Server.ProtocolName, Server.ProtocolVersion);
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
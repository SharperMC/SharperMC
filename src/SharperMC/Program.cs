using SharperMC.Core;

namespace SharperMC
{
	internal static class Program
	{
		private static SharperMCServer _server;

		private static void Main(string[] args)
		{
			_server = new SharperMCServer();
			_server.StartServer(args);
		}
	}
}

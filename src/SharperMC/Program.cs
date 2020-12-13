using SharperMC.Core;

namespace SharperMC
{
	class Program
	{
		private static SharperMCServer _server;
		static void Main(string[] args)
		{
			_server= new SharperMCServer();
			_server.StartServer();
		}
	}
}

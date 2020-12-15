using SharperMC.Core.Commands;

namespace SharperMC.Core.Utils.Console
{
	internal static class ConsoleCommandHandler
	{
		public static void WaitForCommand()
		{
			while (true)
			{
				var input = System.Console.ReadLine();
				if (!string.IsNullOrEmpty(input))
				{
					CommandManager.ParseCommand(Globals.ConsoleSender, input);
				}
			}
		}
	}
}

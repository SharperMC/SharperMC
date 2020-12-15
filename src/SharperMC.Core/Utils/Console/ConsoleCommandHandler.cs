using SharperMC.Core.Commands;

namespace SharperMC.Core.Utils.Console
{
	internal static class ConsoleCommandHandler
	{
		public static void WaitForCommand()
		{
			while (true)
			{
				var input = Globals.ColoredConsole.GetNextString();
				ConsoleFunctions.WriteInfoLine("Input: " + input);
				if (!string.IsNullOrEmpty(input))
				{
					CommandManager.ParseCommand(Globals.ConsoleSender, input);
				}
			}
		}
	}
}

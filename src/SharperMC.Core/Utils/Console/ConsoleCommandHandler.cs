using System;
using SharperMC.Core.Commands;

namespace SharperMC.Core
{
	internal static class ConsoleCommandHandler
	{
		public static void WaitForCommand()
		{
			while (true)
			{
				var input = Console.ReadLine();
				if (!string.IsNullOrEmpty(input))
				{
					CommandManager.ParseCommand(Globals.ConsoleSender, input);
				}
			}
		}
	}
}

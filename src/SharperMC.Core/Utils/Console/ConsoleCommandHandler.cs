using System;

namespace SharperMC.Core
{
	internal static class ConsoleCommandHandler
	{
		public static void WaitForCommand()
		{
			while (true)
			{
				var input = Console.ReadLine();
				if (!String.IsNullOrEmpty(input))
				{
					Globals.ChatManager.HandleCommand(input, null);
				}
			}
		}
	}
}

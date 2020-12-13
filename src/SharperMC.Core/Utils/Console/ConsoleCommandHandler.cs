using System;

namespace SharperMC.Core
{
	internal class ConsoleCommandHandler
	{
		public void WaitForCommand()
		{
			while (true)
			{
				string input = Console.ReadLine();
				if (!String.IsNullOrEmpty(input))
				{
					Globals.ChatManager.HandleCommand(input, null);
				}
			}
		}
	}
}

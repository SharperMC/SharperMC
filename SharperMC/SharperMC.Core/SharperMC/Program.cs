﻿using SharperMC.Core;

namespace SharperMC
{
	internal static class Program
	{
		private static void Main(string[] args) => new Core.SharperMC(args).Server?.StartServer();
	}
}

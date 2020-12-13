namespace SharperMC.Core.Utils
{
	public class McChatMessage
	{
		/*
		 * Todo: Make this better lol
		 */
		public McChatMessage(string message)
		{
			Text = message;
		}

		public readonly string Text;
		public bool Bold = false;
		public bool Italic = false;
		public bool Underlined = false;
		public bool Strikethrough = false;
		public bool Obfuscated = false;
	}
}

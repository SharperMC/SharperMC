namespace SharperMC.Core.Utils.Packets
{
	public class McChatMessage
	{
		/*
		 * Todo: Make this better lol
		 */
		public McChatMessage(string message)
		{
			text = message;
		}

		// DO NOT CHANGE NAME FOR NOW!!!!
		public readonly string text;
		public bool bold = false;
		public bool italic = false;
		public bool underlined = false;
		public bool strikethrough = false;
		public bool obfuscated = false;
	}
}

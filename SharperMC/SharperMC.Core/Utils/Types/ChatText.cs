using System;

namespace SharperMC.Core.Utils.Types
{
    public class ChatText
    {
        public string text;

        public ChatText(string message)
        {
            text = message;
        }
        
        public ChatText(string message, params object[] objs)
        {
            text = String.Format(message, objs);
        }
        
        public bool bold;
        public bool italic;
        public bool underlined;
        public bool strikethrough;
        public bool obfuscated;
    }
}
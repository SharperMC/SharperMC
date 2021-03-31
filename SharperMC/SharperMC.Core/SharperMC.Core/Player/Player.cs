using System;

namespace SharperMC.Core
{
    public class Player
    {
        public String Username;
        public String Uuid;

        public bool Cracked;
        public int Protocol;

        public Player()
        {
            Protocol = SharperMC.Instance.Server.ProtocolVersion;
        }
    }
}
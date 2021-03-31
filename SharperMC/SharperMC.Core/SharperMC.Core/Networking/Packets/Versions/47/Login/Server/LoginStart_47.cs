using System;
using System.Linq;
using System.Net;
using System.Text;
using SharperMC.Core.Networking.Packets.Type;
using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Enums;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Networking.Packets.Versions._47.Login.Server
{
    public class LoginStart_47 : ReadablePacket
    {
        public LoginStart_47(ClientWrapper clientWrapper) : base(clientWrapper)
        {
            Protocol = 47;
            PacketId = (int) Protocol47.LoginStart;
            Status = PacketStatus.Login;
        }

        public override void Read(DataBuffer dataBuffer)
        {
            base.Read(dataBuffer);
            if(dataBuffer == null) return;
            
            var usernameRaw = dataBuffer.ReadString();
            var username = new string(usernameRaw.Where(c => char.IsLetter(c) || char.IsPunctuation(c) || char.IsDigit(c)).ToArray());
            string uuid = null;
            // if (SharperMC.Instance.Server.ProtocolOnline)
                // uuid = GetUuid(username);
            
            if (Encoding.UTF8.GetBytes(username).Length == 0)
                return;
            if (username.Length > 16)
                return;
            
            // ClientWrapper.Player.Equals("a");
            
            Console.WriteLine("Username: " + username + " UUID: " + uuid);
            
            ClientWrapper.Status = PacketStatus.Play;
        }
        
        private string GetUuid(string username)
        {
            try
            {
                var wc = new WebClient();
                var result = wc.DownloadString("https://api.mojang.com/users/profiles/minecraft/" + username);
                var _result = result.Split('"');
                if (_result.Length <= 1) return Guid.NewGuid().ToString();
                var uuid = _result[3];
                return new Guid(uuid).ToString();
            }
            catch
            {
                return Guid.NewGuid().ToString();
            }
        }
    }
}
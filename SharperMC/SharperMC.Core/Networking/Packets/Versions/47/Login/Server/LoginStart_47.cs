using System;
using System.Linq;
using System.Net;
using System.Text;
using SharperMC.Core.Entities.Player;
using SharperMC.Core.Networking.Packets.Type;
using SharperMC.Core.Networking.Packets.Versions._47.Login.Client;
using SharperMC.Core.Networking.Packets.Versions._47.Play.Client;
using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Enums;
using SharperMC.Core.Utils.Misc;
using SharperMC.Core.Utils.Types;
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
            
            var usernameRaw = dataBuffer.ReadString();
            var username = new string(usernameRaw.Where(c => char.IsLetter(c) || char.IsPunctuation(c) || char.IsDigit(c)).ToArray());

            if (String.IsNullOrWhiteSpace(username) || username.Trim().Length == 0)
                return;

            var uuid = UuidUtil.GetUuid(username);
            
            if(!SharperMC.Instance.Server.ServerSettings.AllProtocols() && !SharperMC.Instance.Server.ServerSettings.ProtocolVersions.Contains(ClientWrapper.Protocol))
                new Disconnect_47(ClientWrapper, new ChatText("Minecraft version not allowed! ({0})", ClientWrapper.Protocol)).Write();
            
            if(SharperMC.Instance.Server.ServerSettings.AllProtocols() && !SharperMC.Instance.Server.ServerSettings.IsProtocolSupported(ClientWrapper.Protocol))
                new Disconnect_47(ClientWrapper, new ChatText("Minecraft version not supported! ({0})", ClientWrapper.Protocol)).Write();

            Console.WriteLine(username + " is attempting to connect...");
            
            //TODO: Do encryption process atm idk how.

            new LoginSuccess_47(ClientWrapper, username, uuid.ToString()).Write();
            ClientWrapper.Player = new(ClientWrapper)
            {
                            Username = username,
                            Uuid = uuid
            };
            ClientWrapper.Status = PacketStatus.Play;
            
            //TODO: Compression
            new SetCompression_47(ClientWrapper, -1).Write();
            
            new JoinGame_47(ClientWrapper).Write();
            new SpawnPosition_47(ClientWrapper).Write();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using SharperMC.Core.Networking.Packets.Type;
using SharperMC.Core.Networking.Packets.Versions._47.Login.Server;
using SharperMC.Core.Networking.Packets.Versions.Global.Handshake.Server;
using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Enums;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Networking.Packets
{
    public class PacketHandler
    {
        private readonly List<ReadablePacket> _serverPackets = new List<ReadablePacket>();

        private readonly ClientWrapper _clientWrapper;
        
        public PacketHandler(ClientWrapper clientWrapper)
        {
            _clientWrapper = clientWrapper;
            
            _serverPackets.Add(new Handshake_G(_clientWrapper));

            if (SharperMC.Instance.Server.ProtocolVersion == 47)
            {
                _serverPackets.Add(new LoginStart_47(_clientWrapper));
            }
        }

        public void HandlePacket(DataBuffer dataBuffer, int id)
        {
            foreach (var packet in _serverPackets)
            {
                if(_clientWrapper.Status != packet.Status || (packet.Protocol != _clientWrapper.Player.Protocol) && _clientWrapper.Status != PacketStatus.Handshake)
                    continue;
                    
                if (packet.PacketId != id)
                {
                    if (_serverPackets.All(p => p.PacketId != id))
                    {
                        Console.WriteLine("Unknown packet received! (0x{0:X2} Raw ID: {0}) : (Packet: {1} Packet ID: {2})", id, packet.GetType().Name, packet.PacketId);
                        SharperMC.Instance.Server.ClientHandler.DisconnectClient(_clientWrapper);
                        return;
                    }
                    continue;
                }
                
                Console.WriteLine("Packet received! 0x{0:X2} Raw ID: {0} Client Status: {1} Packet Status: {2}", id, _clientWrapper.Status, packet.Status);
                packet.Read(dataBuffer);
            }
        }
    }
}
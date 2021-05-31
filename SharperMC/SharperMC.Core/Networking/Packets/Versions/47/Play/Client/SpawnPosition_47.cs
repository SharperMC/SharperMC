using SharperMC.Core.Networking.Packets.Type;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Networking.Packets.Versions._47.Play.Client
{
    public class SpawnPosition_47 : SendablePacket
    {
        public SpawnPosition_47(ClientWrapper clientWrapper) : base(clientWrapper)
        {
            Protocol = 47;
            PacketId = (int) Protocol47.SpawnPosition;
        }


        public override void Write()
        {
            double x = 0;
            double y = 70;
            double z = 0;
            
            //var d = Globals.LevelManager.MainLevel.Generator.GetSpawnPoint();
            var data = (((long) x & 0x3FFFFFF) << 38) | (((long) y & 0xFFF) << 26) | ((long) z & 0x3FFFFFF);
            DataBuffer.WriteVarInt(PacketId);
            DataBuffer.WriteLong(data);
            base.Write();
        }
    }
}
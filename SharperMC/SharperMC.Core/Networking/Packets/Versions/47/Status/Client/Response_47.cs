using System.Linq;
using Newtonsoft.Json;
using SharperMC.Core.Networking.Packets.Type;
using SharperMC.Core.Utils.Json;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Networking.Packets.Versions._47.Status.Client
{
    public class Response_47 : SendablePacket
    {
        public Response_47(ClientWrapper clientWrapper) : base(clientWrapper)
        {
            Protocol = 47;
            PacketId = (int) Protocol47.Response;
        }

        public override void Write()
        {
            DataBuffer.WriteVarInt(PacketId);
            StatusRequestVersion statusRequestVersion = new StatusRequestVersion(SharperMC.Instance.Server.ServerSettings.AllProtocols() ? string.Join(", ",SharperMC.Instance.Server.ServerSettings.SupportedVersions().ToArray()) : string.Join(", ", SharperMC.Instance.Server.ServerSettings), SharperMC.Instance.Server.ServerSettings.AllProtocols() ? SharperMC.Instance.Server.ServerSettings.SupportedVersions().ToArray()[0] : SharperMC.Instance.Server.ServerSettings.ProtocolVersions[0]);
            StatusRequestPlayers statusRequestPlayers = new StatusRequestPlayers(SharperMC.Instance.Server.ServerSettings.MaxPlayers, SharperMC.Instance.Server.GetPlayers().Count);
            StatusRequestDescription statusRequestDescription = new StatusRequestDescription(SharperMC.Instance.Server.ServerSettings.ServerDescription);
            DataBuffer.WriteString(JsonConvert.SerializeObject(new StatusRequest(statusRequestVersion, statusRequestPlayers, statusRequestDescription)));
            base.Write();
        }
    }
}
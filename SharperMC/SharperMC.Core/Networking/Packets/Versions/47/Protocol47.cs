namespace SharperMC.Core.Networking.Packets.Versions._47
{
    public enum Protocol47
    {
        // Status
            Response = 0x00,
            Pong = 0x01,
            
            Request = 0x00,
            Ping = 0x01,
        // Login
            LoginStart = 0x00,
            EncryptionResponse = 0x01,
        
            Disconnect = 0x00,
            LoginSuccess = 0x02,
            SetCompression = 0x46,
        // Play
            JoinGame = 0x01,
            SpawnPosition = 0x05,
            PlayerListItem = 0x38,
            SpawnPlayer = 0x0C,
            PlayerPositionAndLook = 0x08,
            MapChunkBulk = 0x26
    }
}
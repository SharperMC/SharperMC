namespace SharperMC.Core.Networking.Packets.Versions._47
{
    public enum Protocol47
    {
        // Status
            Response = 0x00,
            
            Request = 0x00,
        // Login
            LoginStart = 0x00,
            EncryptionResponse = 0x01,
        
            Disconnect = 0x00,
            LoginSuccess = 0x02,
            SetCompression = 0x46,
        // Play
            JoinGame = 0x01,
            SpawnPosition = 0x05
    }
}
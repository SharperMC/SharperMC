namespace SharperMC.Core.Utils.Json
{
    public class StatusRequest
    {
        public StatusRequest(StatusRequestVersion statusRequestVersion, StatusRequestPlayers statusRequestPlayers, StatusRequestDescription statusRequestDescription)
        {
            version = statusRequestVersion;
            players = statusRequestPlayers;
            description = statusRequestDescription;
        }
        
        public StatusRequestVersion version;
        public StatusRequestPlayers players;
        public StatusRequestDescription description;
    }
    
    public class StatusRequestVersion
    {
        public StatusRequestVersion(string name, int protocol)
        {
            this.name = name;
            this.protocol = protocol;
        }
        
        public string name;
        public int protocol;
    }
    
    public class StatusRequestPlayers
    {
        public StatusRequestPlayers(int max, int online)
        {
            this.max = max;
            this.online = online;
        }
        //TODO: Sample https://wiki.vg/Server_List_Ping#Response
        
        public int max;
        public int online;
    }
    
    public class StatusRequestDescription
    {
        public StatusRequestDescription(string text)
        {
            this.text = text;
        }
        
        public string text;
    }
    
    public class StatusRequestFavicon
    {
        public StatusRequestFavicon(string favicon)
        {
            this.favicon = favicon;
        }
        public string favicon;
    }
}
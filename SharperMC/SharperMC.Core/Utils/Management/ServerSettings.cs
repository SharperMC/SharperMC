using System;
using System.Collections.Generic;
using System.Linq;
using SharperMC.Core.Utils.Enums;

namespace SharperMC.Core.Utils.Management
{
    public class ServerSettings
    {
        /// <summary>
        /// Client versions that are allowed to connect
        /// </summary>
        public readonly int[] ProtocolVersions = {(int)Enums.ProtocolVersions._1_8};
        
        /// <summary>
        /// The server protocol name
        /// </summary>
        public readonly string ProtocolName = "SharperMC";
        
        /// <summary>
        /// Max allowed players on the server at a time.
        /// </summary>
        public readonly int MaxPlayers = 32;
        
        /// <summary>
        /// Server description in the server list
        /// </summary>
        public readonly string ServerDescription = "SharperMC Server!!";
        
        /// <summary>
        /// Only authenticated Mojang accounts are authorised to connect
        /// </summary>
        public readonly bool ProtocolOnline = true;
        
        /// <summary>
        /// Reports exceptions / server errors to the console
        /// </summary>
        public readonly bool ReportFatalExceptions = true;
        
        /// <summary>
        /// Starts core functionality on multiple threads.
        /// </summary>
        public readonly bool ServerThreading = false;
        
        /// <summary>
        /// Default gamemode the player will be when joining for the first time
        /// </summary>
        public readonly Gamemode DefaultGamemode = Gamemode.Survival;

        public bool AllProtocols()
        {
            if (ProtocolVersions.Length == 0)
                return true;
            return false;
        }

        public bool IsProtocolSupported(int protocol)
        {
            if (ProtocolVersions.Length == 0 && SupportedVersions().ToArray().Contains(protocol))
                return true;
            return false;
        }

        public IEnumerable<int> SupportedVersions()
        {
            return Enum.GetValues(typeof(ProtocolVersions)).Cast<int>();
        }
    }
}
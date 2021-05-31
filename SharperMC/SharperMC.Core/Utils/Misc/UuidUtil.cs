using System;
using System.Net;

namespace SharperMC.Core.Utils.Misc
{
    public static class UuidUtil
    {
        public static Guid GetUuid(string username)
        {
            try
            {
                var result = new WebClient().DownloadString("https://api.mojang.com/users/profiles/minecraft/" + username).Split('"');
                if (result.Length <= 1) 
                    return Guid.NewGuid();
                return new Guid(result[3]);
            }
            catch
            {
                return Guid.NewGuid();
            }
        }
    }
}
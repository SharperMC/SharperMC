using System;
using System.Collections.Generic;
using System.Timers;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Networking
{
    public class ClientHandler
    {
        public List<ClientWrapper> ClientWrappers { get; }

        private Timer TickTimer;

        public ClientHandler()
        {
            ClientWrappers = new List<ClientWrapper>();

            TickTimer = new Timer();
            TickTimer.Elapsed += DoTick;
            TickTimer.Interval = 1000;
            TickTimer.Start();
        }

        internal void AddClient(ref ClientWrapper clientWrapper)
        {
            ClientWrappers.Add(clientWrapper);
        }

        private void RemoveClient(ClientWrapper clientWrapper)
        {
            ClientWrappers.Remove(clientWrapper);
            clientWrapper.Close();
            GC.Collect();
        }

        public void DisconnectClient(ClientWrapper clientWrapper)
        {
            if (!ClientWrappers.Contains(clientWrapper))
                return;
            RemoveClient(clientWrapper);
        }

        public void DoTick(object obj, ElapsedEventArgs elapsedEventArgs)
        {
            //TODO: Do a tick
        }

        public int ClientWrapperSize()
        {
            return ClientWrappers.Count;
        }
        
        
    }
}
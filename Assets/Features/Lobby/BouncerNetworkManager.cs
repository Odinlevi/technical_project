using System;
using Mirror;
using Mirror.Discovery;

namespace Features.Lobby
{
    public class BouncerNetworkManager : NetworkRoomManager
    {
        public event Action OnClientConnected;
        public event Action OnClientDisconnected;
        public event Action OnSomeoneConnected;
        public event Action OnSomeoneDisconnected;

        public override void Start()
        {
            base.Start();
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            
            OnClientConnected?.Invoke();
        }
        
        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
            OnClientDisconnected?.Invoke();
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            OnSomeoneConnected?.Invoke();
        }
        
        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
            OnSomeoneDisconnected?.Invoke();
        }
        
        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            base.OnServerConnect(conn);
            
            if (numPlayers >= maxConnections)
            {
                conn.Disconnect();
            }
            
            OnSomeoneConnected?.Invoke();
        }
    }
}

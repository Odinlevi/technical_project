using System;
using System.Collections.Generic;
using Mirror;
using Mirror.Discovery;
using UnityEngine;

namespace Features.Lobby
{
    public class BouncerNetworkManager : NetworkRoomManager
    {
        public event Action OnClientConnected;
        public event Action OnClientDisconnected;
        public event Action OnSomeoneConnected;
        public event Action OnSomeoneDisconnected;

        private readonly List<int> _usedSpawnPoints = new List<int>();

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
        
        public override Transform GetStartPosition()
        {
            // first remove any dead transforms
            startPositions.RemoveAll(t => t == null);

            if (startPositions.Count == 0)
                return null;

            if (playerSpawnMethod == PlayerSpawnMethod.Random)
            {
                if (_usedSpawnPoints.Count >= startPositions.Count)
                {
                    _usedSpawnPoints.Clear();
                }

                int startPosIndex;
                
                do
                {
                    startPosIndex = UnityEngine.Random.Range(0, startPositions.Count);
                } while (_usedSpawnPoints.Contains(startPosIndex));
                
                _usedSpawnPoints.Add(startPosIndex);
                
                return startPositions[startPosIndex];
            }
            else
            {
                Transform startPosition = startPositions[startPositionIndex];
                startPositionIndex = (startPositionIndex + 1) % startPositions.Count;
                return startPosition;
            }
        }
    }
}

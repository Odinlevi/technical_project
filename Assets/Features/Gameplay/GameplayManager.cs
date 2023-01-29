using System;
using System.Collections;
using System.Collections.Generic;
using Features.Lobby;
using Features.Player.Controller;
using Features.UI;
using Features.Utility;
using Mirror;
using UnityEngine;

namespace Features.Gameplay
{
    public class GameplayManager : Singleton<GameplayManager>
    {
        public float timeToRestart;
        public bool isRestarting;
        private float _timer;

        private List<PlayerController> _players = new List<PlayerController>();
        
        public void OnPlayerHasWinScore(string winnerName, int winnerScore)
        {
            GameplayUIManager.Instance.ShowWinScreen(winnerName, winnerScore);
        }
        
        [ServerCallback]
        public void ServerOnPlayerHasWinScore()
        {
            _timer = timeToRestart;
            isRestarting = true;
        }

        private void FixedUpdate()
        {
            if (!isRestarting) return;

            if (_timer >= 0)
            {
                _timer -= Time.deltaTime;
                return;
            }

            isRestarting = false;

            ServerRestartGame();
        }
        
        private void ServerRestartGame()
        {
            _players.Clear();
            _players.AddRange(FindObjectsOfType<PlayerController>());
            
            foreach (var player in _players)
            {
                player.ServerResetPlayer();
            }
        }

        
    }
}
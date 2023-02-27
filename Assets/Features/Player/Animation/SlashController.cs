using System;
using Mirror;
using UnityEngine;

namespace Features.Player.Animation
{
    [RequireComponent(typeof(NetworkAnimator))]
    public class SlashController : NetworkBehaviour
    {
        [SerializeField] private NetworkAnimator _networkAnimator;
        [SerializeField] private string isSlashingTrigger = "isSlashing";

        private void OnValidate()
        {
            if (_networkAnimator == null)
                _networkAnimator = GetComponent<NetworkAnimator>();
        }

        public void OnUpdate(bool isSlashing)
        {
            if (!isSlashing)
                return;
            
            _networkAnimator.SetTrigger(isSlashingTrigger);
        }
    }
}
using Mirror;
using UnityEngine;

namespace Features.Player.Health
{
    public class HealthController : NetworkBehaviour
    {
        public float invincibilityTime = 3f;
        [SyncVar]
        public bool isInvincible;

        private bool _isCooldown;
        private float _cooldownTimeLeft;

        [ServerCallback]
        public void ChangeInvincibilityState(bool state)
        {
            isInvincible = state;
        }

        [Command]
        public void AskServerToChangeInvincibilityState(bool state)
        {
            ChangeInvincibilityState(state);
        }

        public void StartCooldown()
        {
            _isCooldown = true;
            _cooldownTimeLeft = invincibilityTime;
        }

        public void OnUpdate()
        {
            if (isInvincible && _isCooldown)
            {
                if (_cooldownTimeLeft > 0f)
                {
                    _cooldownTimeLeft -= Time.fixedDeltaTime;
                    return;
                }

                AskServerToChangeInvincibilityState(false);
                _isCooldown = false;

            }
        }
        
        
    }
}
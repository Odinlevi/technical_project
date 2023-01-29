using System;
using Mirror;
using UnityEngine;

namespace Features.Player.Dash
{
    public class DashController : NetworkBehaviour
    {
        [Header("Dash settings")]
        public float dashDistance = 15f;
        public float dashSpeed = 20f;
        public float dashCooldown = 1f;

        // private Vector3 _dashDirection;
        private float _dashDistanceLeft;
        private float _dashCooldownLeft;

        public bool dash;
        public bool isDashing;
        public bool isDashCooldown;

        private void Update()
        {
            dash = Input.GetMouseButton(0);
        }

        public void OnUpdate(Vector3 direction, CharacterController characterController)
        {
            if (isDashCooldown)
            {
                _dashCooldownLeft -= Time.fixedDeltaTime;

                if (_dashCooldownLeft < 0f)
                    isDashCooldown = false;
            }
            
            if (dash && !isDashing && !isDashCooldown)
            {
                isDashing = true;
                // _dashDirection = direction;
                _dashDistanceLeft = dashDistance;
                isDashCooldown = true;
                _dashCooldownLeft = dashCooldown;
            }

            if (isDashing)
            {
                if (_dashDistanceLeft > 0f)
                {
                    var movement = direction.normalized * (dashSpeed * Time.fixedDeltaTime);
                    characterController.Move(movement);
                    _dashDistanceLeft -= movement.magnitude;
                    
                    return;
                }
                
                isDashing = false;
            }
        }
    }
}
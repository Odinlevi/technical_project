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

        private Vector3 _dashDirection;
        private float _dashDistanceLeft;

        public bool dash;
        public bool isDashing;

        private void Update()
        {
            dash = Input.GetMouseButton(0);
        }

        public void OnUpdate(Vector3 direction, CharacterController characterController)
        {
            if (dash && !isDashing)
            {
                isDashing = true;
                _dashDirection = direction;
                _dashDistanceLeft = dashDistance;
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
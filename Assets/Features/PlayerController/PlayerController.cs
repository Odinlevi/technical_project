using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.PlayerController
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(NetworkTransform))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : NetworkBehaviour
    {
        public CharacterController characterController;
        public Transform cam;

        [Header("Movement Settings")]
        public float moveSpeed = 8f;
        public float turnSmoothTime = 0.1f;
        
        private float _turnSmoothVelocity;

        [Header("Dash settings")]
        public float dashDistance = 15f;
        public float dashSpeed = 20f;
        public float dashCooldown = 1f;

        private Vector3 _dashDirection;
        private float _dashDistanceLeft;
        
        [Header("Diagnostics")]
        public float horizontal;
        public float vertical;
        public bool dash;
        public bool isDashing;

        // void OnValidate()
        // {
        //     if (characterController == null)
        //         characterController = GetComponent<CharacterController>();
        //
        //     characterController.enabled = false;
        //     GetComponent<Rigidbody>().isKinematic = true;
        //     GetComponent<NetworkTransform>().syncDirection = SyncDirection.ClientToServer;
        // }

        public override void OnStartLocalPlayer()
        {
            cam = Camera.main.transform;
            characterController.enabled = true;
        }

        private void Update()
        {
            if (!isLocalPlayer || characterController == null || !characterController.enabled)
                return;

            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            dash = Input.GetMouseButton(0);

        }

        private void FixedUpdate()
        {
            if (!isLocalPlayer || characterController == null || !characterController.enabled)
                return;

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude < 0.1f && !isDashing)
            {
                characterController.Move(Vector3.zero);
                return;
            }

            if (dash)
            {
                isDashing = true;
                _dashDirection = direction;
                _dashDistanceLeft = dashDistance;
                
            }
            
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            if (isDashing)
            {
                if (_dashDistanceLeft > 0f)
                {
                    var movement = moveDirection.normalized * (dashSpeed * Time.fixedDeltaTime);
                    characterController.Move(movement);
                    _dashDistanceLeft -= movement.magnitude;
                    
                    return;
                }
                
                isDashing = false;
            }

            characterController.Move(moveDirection.normalized * (moveSpeed * Time.fixedDeltaTime));
        }
    }
}
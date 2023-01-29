using Mirror;
using UnityEngine;

namespace Features.Player.Direction
{
    public class DirectionController : NetworkBehaviour
    {
        private Transform _cameraTransform;
        
        [Header("Diagnostics")]
        public float horizontal;
        public float vertical;
        
        public float turnSmoothTime = 0.1f;
        private float _turnSmoothVelocity;

        public override void OnStartLocalPlayer()
        {
            var camera = UnityEngine.Camera.main;
            _cameraTransform = camera.transform;
        }
        
        private void Update()
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }

        public Vector3 GetInputDirection()
        {
            return new Vector3(horizontal, 0f, vertical).normalized;
        }

        public Vector3 GetMovementDirection(Vector3 direction)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            
            return moveDirection;
        }
    }
}
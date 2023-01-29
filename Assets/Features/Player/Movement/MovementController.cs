using Mirror;
using UnityEngine;

namespace Features.Player.Movement
{
    public class MovementController : NetworkBehaviour
    {
        private Transform _camera;
        
        [Header("Movement Settings")]
        public float moveSpeed = 8f;

        public void OnUpdate(Vector3 inputDirection, Vector3 movementDirection, CharacterController characterController)
        {
            if (inputDirection.magnitude < 0.1f)
            {
                characterController.Move(Vector3.zero);
                return;
            }
            
            characterController.Move(movementDirection.normalized * (moveSpeed * Time.fixedDeltaTime));

        }
    }
}
using Mirror;
using UnityEngine;

namespace Features.Player.Gravity
{
    public class GravityController: NetworkBehaviour
    {
        [Header("Gravity Settings")]
        public float gravityForce = 50f;

        public Vector3 gravityDirection = new Vector3(0, -1, 0);
        
        public void OnUpdate(CharacterController characterController)
        {
            characterController.Move(gravityDirection * (gravityForce * Time.fixedDeltaTime));
        }
    }
}
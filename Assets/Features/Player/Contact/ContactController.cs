using Features.Player.Controller;
using Mirror;
using UnityEngine;

namespace Features.Player.Contact
{
    public class ContactController : NetworkBehaviour
    {
        [SerializeField] private PlayerController _playerController;
        
        [ServerCallback]
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                
            }
        }

        [ClientRpc]
        private void RpcOnCollision()
        {
            
            
            
        }
    }
}
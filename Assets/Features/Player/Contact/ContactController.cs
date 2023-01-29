using System;
using Features.Player.Controller;
using Features.Player.Dash;
using Features.Player.Health;
using Mirror;
using UnityEngine;

namespace Features.Player.Contact
{
    [RequireComponent(typeof(DashController))]
    [RequireComponent(typeof(HealthController))]
    public class ContactController : NetworkBehaviour
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private DashController _dashController;
        [SerializeField] private HealthController _healthController;

        private void OnValidate()
        {
            if (_dashController == null)
                _dashController = GetComponent<DashController>();
            if (_healthController == null)
                _healthController = GetComponent<HealthController>();
            if (_playerController == null)
                _playerController = GetComponent<PlayerController>();
        }

        [ServerCallback]
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.GetComponent<DashController>().isDashing && !_healthController.isInvincible)
                {
                    _playerController.ServerOnPlayerGotHit();
                    RpcOnCollisionGet();
                }
                if (_dashController.isDashing && !other.GetComponent<HealthController>().isInvincible)
                    RpcOnCollisionScored();
            }
        }

        [ClientRpc]
        private void RpcOnCollisionScored()
        {
            Debug.Log($"Collision scored by local = {netId}");
        }

        [ClientRpc]
        private void RpcOnCollisionGet()
        {
            Debug.Log($"Collision got by local = {netId}");
            _playerController.RpcOnPlayerGotHit();
        }
    }
}
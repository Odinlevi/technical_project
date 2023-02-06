using System;
using System.Collections.Generic;
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
        
        private List<Collider> _pendingCollidersList = new List<Collider>();

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
                Debug.Log(" ------- ");
                Debug.Log(this.name);
                Debug.Log(other.name);
                Debug.Log(" ------- end ");
                var otherHealthController = other.GetComponent<HealthController>();
                
                var otherIsDashing = other.GetComponent<DashController>().isDashing;
                var otherIsInvincible = otherHealthController.isInvincible;


                var thisIsDashing = _dashController.isDashing;
                var thisIsInvincible = _healthController.isInvincible;
                
                // Debug.Log($"netId: {netId}, isLocal: {isLocalPlayer}, otherIsDashing: {otherIsDashing}, " +
                //           $"otherIsInvincible: {otherIsInvincible}, otherWaitingForHitAuthor: {otherHealthController.waitingForHitAuthor}, " +
                //           $"thisIsDashing: {thisIsDashing}, thisIsInvincible: {thisIsInvincible}, thisWaitingForHitAuthor: {_healthController.waitingForHitAuthor}");
                
                if (thisIsDashing && (!otherIsInvincible || otherHealthController.waitingForHitAuthor))
                {
                    _playerController.ServerOnPlayerHit();
                    RpcOnCollisionScored();

                    otherHealthController.waitingForHitAuthor = !otherHealthController.waitingForHitAuthor;
                }
                if (otherIsDashing && !thisIsInvincible)
                {
                    _healthController.waitingForHitAuthor = !_healthController.waitingForHitAuthor;

                    _playerController.ServerOnPlayerGotHit();
                    RpcOnCollisionGet();
                }
                
            }
        }

        [ClientRpc]
        private void RpcOnCollisionScored()
        {
            Debug.Log($"Collision scored by local = {netId}");
            _playerController.RpcOnPlayerHit();
        }

        [ClientRpc]
        private void RpcOnCollisionGet()
        {
            Debug.Log($"Collision got by local = {netId}");
            _playerController.RpcOnPlayerGotHit();
        }
    }
}
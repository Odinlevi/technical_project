using System;
using Features.Player.Camera;
using Features.Player.Color;
using Features.Player.Contact;
using Features.Player.Dash;
using Features.Player.Direction;
using Features.Player.Movement;
using Mirror;
using UnityEngine;

namespace Features.Player.Controller
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(MovementController))]
    [RequireComponent(typeof(DashController))]
    [RequireComponent(typeof(DirectionController))]
    [RequireComponent(typeof(NetworkTransform))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private DirectionController _directionController;
        [SerializeField] private DashController _dashController;
        [SerializeField] private MovementController _movementController;
        
        private void OnValidate()
        {
            if (_characterController == null)
                _characterController = GetComponent<CharacterController>();
            if (_directionController == null)
                _directionController = GetComponent<DirectionController>();
            if (_dashController == null)
                _dashController = GetComponent<DashController>();
            if (_movementController == null)
                _movementController = GetComponent<MovementController>();
        
            _characterController.enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<NetworkTransform>().syncDirection = SyncDirection.ClientToServer;
        }

        public override void OnStartLocalPlayer()
        {
            if (isLocalPlayer)
            {
                var orbitCamera = UnityEngine.Camera.main.GetComponent<OrbitCamera>();

                orbitCamera.focus = transform;
                orbitCamera.enabled = true;
                
                _characterController.enabled = true;
            }
        }

        private void FixedUpdate()
        {
            if (!isLocalPlayer || _characterController == null || !_characterController.enabled)
                return;
            var inputDirection = _directionController.GetInputDirection();
            var moveDirection = _directionController.GetMovementDirection(inputDirection);
            _dashController.OnUpdate(moveDirection, _characterController);
            _movementController.OnUpdate(inputDirection, moveDirection, _characterController);
        }
    }
}
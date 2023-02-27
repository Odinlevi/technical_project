using Features.Gameplay;
using Features.Lobby;
using Features.Player.Camera;
using Features.Player.Color;
using Features.Player.Contact;
using Features.Player.Dash;
using Features.Player.Direction;
using Features.Player.Gravity;
using Features.Player.Health;
using Features.Player.Movement;
using Features.Player.Score;
using Features.UI;
using Mirror;
using UnityEngine;

namespace Features.Player.Controller
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(MovementController))]
    [RequireComponent(typeof(GravityController))]
    [RequireComponent(typeof(DashController))]
    [RequireComponent(typeof(DirectionController))]
    [RequireComponent(typeof(NetworkTransform))]
    [RequireComponent(typeof(ColorController))]
    [RequireComponent(typeof(ContactController))]
    [RequireComponent(typeof(HealthController))]
    [RequireComponent(typeof(ScoreController))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private DirectionController _directionController;
        [SerializeField] private DashController _dashController;
        [SerializeField] private MovementController _movementController;
        [SerializeField] private GravityController _gravityController;
        [SerializeField] private ColorController _colorController;
        [SerializeField] private ContactController _contactController;
        [SerializeField] private HealthController _healthController;
        [SerializeField] private ScoreController _scoreController;
        
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
            if (_gravityController == null)
                _gravityController = GetComponent<GravityController>();
            if (_colorController == null)
                _colorController = GetComponent<ColorController>();
            if (_contactController == null)
                _contactController = GetComponent<ContactController>();
            if (_healthController == null)
                _healthController = GetComponent<HealthController>();
            if (_scoreController == null)
                _scoreController = GetComponent<ScoreController>();
        
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
            if (_characterController == null)
                return;
                
            if (_healthController.isInvincible)
                _colorController.ChangeColorToHurt();
            else
                _colorController.ChangeColorToDefault();
            
            _scoreController.OnUpdate();
            
            if (!isLocalPlayer ||  !_characterController.enabled)
                return;
            
            var inputDirection = _directionController.GetInputDirection();
            var moveDirection = _directionController.GetMovementDirection(inputDirection);
            _dashController.OnUpdate(moveDirection, _characterController);
            _movementController.OnUpdate(inputDirection, moveDirection, _characterController);
            _gravityController.OnUpdate(_characterController);
            _healthController.OnUpdate();
        }

        public void ServerOnPlayerHit()
        {
            _scoreController.AddScore();

            if (_scoreController.score >= _scoreController.scoreToWin)
            {
                if (!GameplayManager.Instance.isRestarting)
                {
                    RpcFollowUpOnSomeoneWon(netId.ToString(), _scoreController.score);
                    GameplayManager.Instance.ServerOnPlayerHasWinScore();
                }

            }
        }
        
        public void ServerOnPlayerGotHit()
        {
            _healthController.ChangeInvincibilityState(true);
        }
        
        public void RpcOnPlayerHit()
        {
            
        }

        public void RpcOnPlayerGotHit()
        {
            _healthController.StartCooldown();
        }
        
        [ClientRpc]
        private void RpcFollowUpOnSomeoneWon(string winnerName, int winnerScore)
        {
            GameplayManager.Instance.OnPlayerHasWinScore(winnerName, winnerScore);
        }

        [ServerCallback]
        public void ServerResetPlayer()
        {
            _healthController.isInvincible = false;
            _dashController.isDashing = false;
            _scoreController.score = 0;
            
            RpcResetPlayer();
        }
        
        [ClientRpc]
        private void RpcResetPlayer()
        {
            GameplayUIManager.Instance.HideWinScreen();
            _characterController.enabled = false;
            _colorController.ChangeColorToDefault();
            var networkManager = NetworkManager.singleton as BouncerNetworkManager;
            var startPos = networkManager.GetStartPosition();
            transform.position = startPos.position;
            transform.rotation = startPos.rotation;
            
            _characterController.enabled = isLocalPlayer;
        }
    }
}
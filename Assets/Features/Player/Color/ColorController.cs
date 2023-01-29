using Mirror;
using UnityEngine;

namespace Features.Player.Color
{
    public class ColorController : NetworkBehaviour
    {
        [SerializeField] private UnityEngine.Color _localPlayerColor;
        [SerializeField] private UnityEngine.Color _otherPlayerColor;
        [SerializeField] private UnityEngine.Color _hurtColor;

        [SerializeField] private MeshRenderer _meshRenderer;
        
        private UnityEngine.Color _defaultColor;
        
        public override void OnStartLocalPlayer()
        {
            SetupDefaultColor(isLocalPlayer);
        }
        
        private void SetupDefaultColor(bool isLocal)
        {
            _defaultColor = isLocal ? _localPlayerColor : _otherPlayerColor;

            _meshRenderer.materials[0].color = _defaultColor;
        }

        public void ChangeColorToHurt()
        {
            _meshRenderer.materials[0].color = _hurtColor;
        }

        public void ChangeColorToDefault()
        {
            _meshRenderer.materials[0].color = _defaultColor;
        }
    }
}

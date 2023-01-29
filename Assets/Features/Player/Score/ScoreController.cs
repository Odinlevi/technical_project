using Mirror;
using TMPro;
using UnityEngine;

namespace Features.Player.Score
{
    public class ScoreController : NetworkBehaviour
    {
        [SerializeField] private TextMeshPro _textMesh;
        
        [SyncVar]
        public int score = 0;
        public int scoreToWin = 3;

        [ServerCallback]
        public void AddScore()
        {
            score += 1;
        }

        public void OnUpdate()
        {
            _textMesh.text = $"{score}";
            _textMesh.transform.LookAt(UnityEngine.Camera.main.transform);
        }
    }
}
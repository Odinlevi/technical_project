using Mirror;

namespace Features.Player.Score
{
    public class ScoreController : NetworkBehaviour
    {
        [SyncVar]
        public int score; 

        [ServerCallback]
        public void GetScore()
        {
            score += 1;
        }
        
        
    }
}
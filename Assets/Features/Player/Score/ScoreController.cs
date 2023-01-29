using Mirror;

namespace Features.Player.Score
{
    public class ScoreController : NetworkBehaviour
    {
        public int Score { get; private set; }

        [ServerCallback]
        public void GetScore()
        {
            Score += 1;
        }
    }
}
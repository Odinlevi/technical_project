using Features.Utility;
using TMPro;
using UnityEngine;

namespace Features.UI
{
    public class GameplayUIManager : Singleton<GameplayUIManager>
    {
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private TextMeshProUGUI _winnerText;
        
        public void ShowWinScreen(string winner, int winnerScore)
        {
            _winPanel.SetActive(true);
            _winnerText.text = $"Winner: {winner}, score: {winnerScore}";
        }

        public void HideWinScreen()
        {
            _winPanel.SetActive(false);
        }
    }
}
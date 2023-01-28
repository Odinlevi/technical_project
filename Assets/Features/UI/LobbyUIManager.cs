using Features.Lobby;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.UI
{
    public class LobbyUIManager : MonoBehaviour
    {
        [Header("JoinRoomPanel")]
        [SerializeField] private GameObject _joinRoomPanel;
        
        [SerializeField] private Button _joinRoomButton;
        [SerializeField] private Button _createRoomButton;

        [SerializeField] private TMP_InputField _joinInput;
        [SerializeField] private TMP_InputField _createInput;

        [SerializeField] private TextMeshProUGUI _statusLabel;
        
        [Header("WaitForHostPanel")]
        [SerializeField] private GameObject _waitForHostPanel;
        [SerializeField] private Button _cancelAsClientButton;
        
        [Header("HostControlPanel")]
        [SerializeField] private GameObject _hostPanel;

        [SerializeField] private TextMeshProUGUI _currentPlayersLabel;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _cancelAsHostButton;

        private BouncerNetworkManager _networkManager;

        #region Events

        private void Start()
        {
            _joinRoomButton.onClick.AddListener(OnJoinRoomButton);
            _createRoomButton.onClick.AddListener(OnCreateRoomButton);
            _cancelAsClientButton.onClick.AddListener(OnCancelAsClientButton);
            _cancelAsHostButton.onClick.AddListener(OnCancelAsHostButton);
            _startGameButton.onClick.AddListener(OnStartButton);
        }

        private void OnEnable()
        {
            _networkManager = NetworkManager.singleton as BouncerNetworkManager;
            _networkManager.OnClientConnected += OnClientConnected;
            _networkManager.OnClientDisconnected += OnClientDisconnected;
            _networkManager.OnSomeoneConnected += OnSomeoneConnected;
            _networkManager.OnSomeoneDisconnected += OnSomeoneDisconnected;
        }

        private void OnDisable()
        {
            _networkManager.OnClientConnected -= OnClientConnected;
            _networkManager.OnClientDisconnected -= OnClientDisconnected;
            _networkManager.OnSomeoneConnected -= OnSomeoneConnected;
            _networkManager.OnSomeoneDisconnected -= OnSomeoneDisconnected;
        }

        #endregion


        #region ButtonControls

        private void OnJoinRoomButton()
        {
            _networkManager.networkAddress = _joinInput.text;
            _networkManager.StartClient();
        }

        private void OnCreateRoomButton()
        {
            _networkManager.networkAddress = _createInput.text;
            _networkManager.StartHost();
        }

        private void OnStartButton()
        {
            _networkManager.allPlayersReady = true;
            _networkManager.ServerChangeScene(_networkManager.GameplayScene);
        }

        private void OnCancelAsClientButton()
        {
            _networkManager.StopClient();
        }
        
        private void OnCancelAsHostButton()
        {
            // if (_networkManager.numPlayers == 1)
            //     _networkManager.StopClient();
            // else
            _networkManager.StopHost();
        }

        #endregion
        

        private void OnClientConnected()
        {
            var isClient = _networkManager.mode == NetworkManagerMode.ClientOnly;
            
            _joinRoomPanel.SetActive(false);
            _hostPanel.SetActive(!isClient);
            _waitForHostPanel.SetActive(isClient);
        }

        private void OnClientDisconnected()
        {
            _joinRoomPanel.SetActive(true);
            _hostPanel.SetActive(false);
            _waitForHostPanel.SetActive(false);
        }

        private void OnSomeoneConnected()
        {
            _currentPlayersLabel.text = $"Current players: {_networkManager.numPlayers}";
        }
        
        private void OnSomeoneDisconnected()
        {
            _currentPlayersLabel.text = $"Current players: {_networkManager.numPlayers}";
        }
    }
}
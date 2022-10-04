using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using Mono.Cecil.Cil;

namespace MudPuppyGames.CardGame
{
    public class Launcher : MonoBehaviour
    {
        string gameVersion = "0.1.0";
        [SerializeField] private NetworkRunner _networkRunnerPrefab = null;
        [SerializeField] private PlayerData _playerDataPrefab = null;
        [SerializeField] private TMP_InputField _nickname = null;
        [SerializeField] private TMP_InputField _roomName = null;
        [SerializeField] private TextMeshProUGUI _versionLabel = null;
        [SerializeField] private string _gameSceneName = null;

        private NetworkRunner _runnerInstance = null;

        private void Awake()
        {
            _versionLabel.text = gameVersion;
        }

        public string RandomizeRoomName()
        {
            string name = "";
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for(int i = 0; i < 5; i++)
                name += chars[Random.Range(0,chars.Length)];

            return name;
        }

        public void SetPlayerData()
        {
            var playerData = FindObjectOfType<PlayerData>();
            if(playerData == null)
                playerData = Instantiate(_playerDataPrefab);

            playerData.Nickname = _nickname.text;
        }

        public void StartHost()
        {
            SetPlayerData();
            StartGame(GameMode.AutoHostOrClient);
        }

        public void StartClient()
        {
            SetPlayerData();
            StartGame(GameMode.Client);
        }

        private async void StartGame(GameMode mode, string roomName = "")
        {
            _runnerInstance = FindObjectOfType<NetworkRunner>();
            if (_runnerInstance == null)
                _runnerInstance = Instantiate(_networkRunnerPrefab);

            _runnerInstance.ProvideInput = true;

            var startGameArgs = new StartGameArgs()
            {
                GameMode = mode,
                SessionName = mode == GameMode.Client ? _roomName.text : RandomizeRoomName(),
                //ObjectPool = _runnerInstance.GetComponent<NetworkObjectPoolDefault>(),
            };

            await _runnerInstance.StartGame(startGameArgs);

            _runnerInstance.SetActiveScene(_gameSceneName);
        }
    }

}

using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MudPuppyGames.CardGame
{
    // A purely utilitarian class which manages the display of player information (Nickname, Lives and Score)
    // All methods are called from PlayerDataNetworked when a change is detected there.
    public class PlayerOverviewPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerOverviewEntryPrefab = null;

        private Dictionary<PlayerRef, TextMeshProUGUI>
            _playerListEntries = new Dictionary<PlayerRef, TextMeshProUGUI>();

        private Dictionary<PlayerRef, string> _playerNickNames = new Dictionary<PlayerRef, string>();

        // Creates a new Overview Entry
        public void AddEntry(PlayerRef playerRef, PlayerDataNetworked playerDataNetworked)
        {
            Debug.Log("Player spawned");
            if (_playerListEntries.ContainsKey(playerRef)) return;
            if (playerDataNetworked == null) return;

            var entry = Instantiate(_playerOverviewEntryPrefab, this.transform);
            entry.transform.localScale = Vector3.one;

            string nickName = String.Empty;

            _playerNickNames.Add(playerRef, nickName);

            _playerListEntries.Add(playerRef, entry);

            UpdateEntry(playerRef, entry);
        }

        // Removes an existing Overview Entry
        public void RemoveEntry(PlayerRef playerRef)
        {
            if (_playerListEntries.TryGetValue(playerRef, out var entry) == false) return;

            if (entry != null)
            {
                Destroy(entry.gameObject);
            }

            _playerNickNames.Remove(playerRef);

            _playerListEntries.Remove(playerRef);
        }

        public void UpdateNickName(PlayerRef player, string nickName)
        {
            if (_playerListEntries.TryGetValue(player, out var entry) == false) return;

            _playerNickNames[player] = nickName;
            UpdateEntry(player, entry);
        }

        private void UpdateEntry(PlayerRef player, TextMeshProUGUI entry)
        {
            var nickName = _playerNickNames[player];

            entry.text = $"{nickName}";
        }
    }
}

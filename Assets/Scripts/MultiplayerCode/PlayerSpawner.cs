using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MudPuppyGames.CardGame
{
    public class PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft, ISpawned
    {
        [SerializeField] private NetworkPrefabRef _playerNetworkPrefab = NetworkPrefabRef.Empty;
        private bool _gameIsReady = false;

        public void Spawned()
        {
            if (Object.HasStateAuthority == false)
                return;

        }


        public void PlayerJoined(PlayerRef player)
        {
            SpawnPlayer(player);
        }

        private void SpawnPlayer(PlayerRef player)
        {
            var PlayerObject = Runner.Spawn(_playerNetworkPrefab,new Vector3(0,0,0), Quaternion.identity, player);
        }

        public void PlayerLeft(PlayerRef player)
        {
            throw new System.NotImplementedException();
        }
    }
}

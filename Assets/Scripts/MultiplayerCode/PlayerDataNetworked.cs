using Fusion;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MudPuppyGames.CardGame
{
    public class PlayerDataNetworked : NetworkBehaviour
    {
        private PlayerOverviewPanel _overviewPanel = null;

        [HideInInspector]
        [Networked(OnChanged = nameof(OnNickNameChanged))]
        public NetworkString<_16> NickName { get; set; }

        public override void Spawned()
        {
            Debug.Log("Spawned!");
            // --- Client
            // Find the local non-networked PlayerData to read the data and communicate it to the Host via a single RPC 
            if (Object.HasInputAuthority)
            {
                var nickName = FindObjectOfType<PlayerData>().Nickname;
                RpcSetNickName(nickName);
            }
            // --- Host & Client
            // Set the local runtime references.
            _overviewPanel = FindObjectOfType<PlayerOverviewPanel>();
            // Add an entry to the local Overview panel with the information of this spaceship
            _overviewPanel.AddEntry(Object.InputAuthority, this);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _overviewPanel.RemoveEntry(Object.InputAuthority);
        }

        // RPC used to send player information to the Host
        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
        private void RpcSetNickName(string nickName)
        {
            if (string.IsNullOrEmpty(nickName)) return;
            NickName = nickName;
        }

        // Updates the player's nickname displayed in the local Overview Panel entry.
        public static void OnNickNameChanged(Changed<PlayerDataNetworked> playerInfo)
        {
            playerInfo.Behaviour._overviewPanel.UpdateNickName(playerInfo.Behaviour.Object.InputAuthority,
                playerInfo.Behaviour.NickName.ToString());
        }
    }
}

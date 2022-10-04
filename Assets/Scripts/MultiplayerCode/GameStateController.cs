using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace MudPuppyGames.CardGame
{
    public class GameStateController : NetworkBehaviour
    {
        enum GameState
        {
            InLobby,
            Starting,
            Running,
            Ending
        }
    }

}

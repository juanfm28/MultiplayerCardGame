using MudPuppyGames.CardGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;

public class DebugGameLogic : MonoBehaviour
{
    public StupidGameLogic logic;
    public TextMeshProUGUI deck;
    public TextMeshProUGUI pileTop;
    public List<TextMeshProUGUI> playerText;

    public bool locking;
    public bool gameReadyToStart;
    public int activePlayer = -1;

    public void DisplayGameState()
    {
        Debug.Log("Refreshing game. Active Player ="+activePlayer);
        deck.text = "On Deck: "+logic.CardsOnDeck;
        for(int i =0; i < playerText.Count; i++)
        {
            playerText[i].color = activePlayer == i ? new Color(1f,0f,0f) : new Color(1f,1f,1f);
            playerText[i].text = logic.players[i].ToString();
        }
        pileTop.text = logic.IsActive ? "Ready to start" : "Waiting for players";
    }

    public void SetActivePlayer(int p)
    {
        activePlayer = p;
        DisplayGameState();
    }

    public void SendPlayerReady(int i)
    {
        logic.PlayerReady(i);
        activePlayer = -1;
        DisplayGameState();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            logic.players[activePlayer].LockDownCard(0);
            DisplayGameState();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            logic.players[activePlayer].LockDownCard(1);
            DisplayGameState();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            logic.players[activePlayer].LockDownCard(2);
            DisplayGameState();
        }
    }
}

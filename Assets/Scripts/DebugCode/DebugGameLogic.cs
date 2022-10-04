using MudPuppyGames.CardGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugGameLogic : MonoBehaviour
{
    public StupidGameLogic logic;
    public TextMeshProUGUI deck;
    public TextMeshProUGUI pileTop;
    public List<TextMeshProUGUI> playerText;

    public bool locking;
    public bool gameReadyToStart;
    public int activePlayer = -1;

    private void Start()
    {
        logic.OnPlay.AddListener(ReadActivePlayerFromGame);
        logic.OnPlay.AddListener(DisplayGameState);
        logic.OnGameStart.AddListener(ReadActivePlayerFromGame);
        logic.OnGameStart.AddListener(DisplayGameState);
        logic.OnGameFinished.AddListener(DisplayGameState);
        foreach (StupidPlayer player in logic.players)
            player.OnSelectHandCards.AddListener(DisplayGameState);
    }

    public void DisplayGameState()
    {
        deck.text = "On Deck: "+logic.CardsOnDeck;
        for(int i =0; i < playerText.Count; i++)
        {
            playerText[i].color = activePlayer == i ? new Color(1f,0f,0f) : new Color(1f,1f,1f);
            playerText[i].text = logic.players[i].ToString();
        }
        pileTop.text = logic.IsActive ? "Ready to start" : "Waiting for players";

        if (logic.deck.PeekGameStack() != null)
            pileTop.text = logic.deck.PeekGameStack().ToString();
    }

    public void SetActivePlayer(int p)
    {
        activePlayer = p;
        DisplayGameState();
    }

    public void ReadActivePlayerFromGame()
    {
        activePlayer = logic.players.IndexOf(logic.playerInTurn); 
    }

    public void Play()
    {
        Debug.Log("Player "+activePlayer+" made a move");
        if(logic.IsActive)
        {
            logic.PlayTurn();
        }
        else
        {
            logic.players[activePlayer].MakePlay();
        }
        DisplayGameState();
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            logic.players[activePlayer].SelectCardToPlay(0);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            logic.players[activePlayer].SelectCardToPlay(1);
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            logic.players[activePlayer].SelectCardToPlay(2);
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            logic.players[activePlayer].SelectCardToPlay(3);
        }
        else if (Input.GetKeyUp(KeyCode.T))
        {
            logic.players[activePlayer].SelectCardToPlay(4);
        }
        else if (Input.GetKeyUp(KeyCode.Y))
        {
            logic.players[activePlayer].SelectCardToPlay(5);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            logic.players[activePlayer].SelectCardToPlay(6);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            logic.players[activePlayer].SelectCardToPlay(7);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            logic.players[activePlayer].SelectCardToPlay(8);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            logic.players[activePlayer].SelectCardToPlay(9);
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            logic.players[activePlayer].SelectCardToPlay(10);
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            logic.players[activePlayer].SelectCardToPlay(11);
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            logic.players[activePlayer].SelectCardToPlay(12);
        }
    }
}

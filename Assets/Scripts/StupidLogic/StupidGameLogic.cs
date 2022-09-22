using JetBrains.Annotations;
using MudPuppyGames.CardGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhase { Dealing, Standby, Playing }

public class StupidGameLogic : MonoBehaviour
{
    public int numPlayers;
    public int numCardsDown;
    public int initialPlayer;
    public List<StupidPlayer> players;
    [SerializeField]
    public PlayingDeck deck;
    public bool IsActive;
    public StupidPlayer turn;
    public bool ccw = true;
    public bool lower;

    public int NextTurn
    {
        get
        {
            int next = players.IndexOf(turn);
            if (ccw)
                next--;
            else
                next++;
            if (next >= numPlayers)
                next = 0;
            else if(next < 0)
                next = numPlayers - 1;
            return next;
        }
    }
    /*DebugOnly*/

    public int CardsOnDeck
    {
        get { return deck.RemainingCards; }
    }


    public void SetUpGame(int numPlayers, int numCardsDown)
    {
        this.numPlayers = numPlayers;
        this.numCardsDown = numCardsDown;
        SetUpGame();
    }

    public void SetUpGame()
    {
        //Create Deck
        deck = new PlayingDeck();
        deck.Shuffle();
        //Seat players
        for(int i = 0; i < this.numPlayers; i++)
        {
            players[i].SitDown(this.numCardsDown);
            players[i].name = "Player " + (i + 1);
            players[i].OnPlayerReady.AddListener(StartGameWhenReady);
        }
        //Deal cards
        for(int i = this.numPlayers - 1; i >= 0; i--)
        {
            for(int j = numCardsDown -1; j >=0; j--)
            {
                players[i].HideCard(deck.GetCard());
            }
        }
        for (int i = this.numPlayers - 1; i >= 0; i--)
        {
            for (int j = numCardsDown * 2 - 1; j >= 0; j--)
            {
                players[i].Draw(deck.GetCard());
            }
        }
    }

    public void StartGameWhenReady()
    {
        foreach(StupidPlayer player in players)
        {
            if (!player.isReady)
                return;
        }
        IsActive = true;
        Debug.Log("Game Started");
        StartCoroutine(GameLoop());
    }

    public IEnumerator GameLoop()
    {
        turn = players[initialPlayer];
        turn.SetActivePlayer(true);
        deck.StartGameStack();
        while(true)
        {
            yield return new WaitForEndOfFrame();
        }
    }
}

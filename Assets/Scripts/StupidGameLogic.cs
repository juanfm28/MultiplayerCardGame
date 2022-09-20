using JetBrains.Annotations;
using MudPuppyGames.CardGame;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public enum GamePhase { Dealing, Standby, Playing }

public class StupidGameLogic : MonoBehaviour
{
    [SerializeField] int numPlayers;
    [SerializeField] int numCardsDown;
    [SerializeField] int initialPlayer;
    [HideInInspector]
    public Seat[] players;
    [SerializeField]
    PlayingDeck deck;
    public bool IsActive;
    public int turn;
    public bool ccw = true;

    public int NextTurn
    {
        get
        {
            int next = turn;
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
        deck.Initialize(new Deck());
        deck.Shuffle();
        //Seat players
        players = new Seat[this.numPlayers];
        for(int i = 0; i < this.numPlayers; i++)
        {
            players[i] = new Seat();
            players[i].Init(this.numCardsDown);
            players[i].name = "Player " + (i + 1);
        }
        //Deal cards
        for(int i = this.numPlayers - 1; i >= 0; i--)
        {
            for(int j = numCardsDown -1; j >=0; j--)
            {
                players[i].ReceiveHiddenCard(deck.GiveCard());
            }
        }
        for (int i = this.numPlayers - 1; i >= 0; i--)
        {
            for (int j = numCardsDown * 2 - 1; j >= 0; j--)
            {
                players[i].ReceiveHandCard(deck.GiveCard());
            }
        }
    }

    public void PlayerReady(int i)
    {
        players[i].ready = true;
        foreach(Seat s in players)
        {
            if (!s.ready)
                return;
        }
        IsActive = true;
    }

    
}

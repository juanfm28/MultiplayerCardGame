using MudPuppyGames.CardGame;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public StupidPlayer playerInTurn;
    public bool ccw = true;
    public bool lower;
    bool playAgain;

    public UnityEvent OnPlay;
    public UnityEvent OnGameStart;
    public UnityEvent OnGameFinished;
    /*DebugOnly*/
    private void Awake()
    {
        OnPlay = new UnityEvent();
        OnGameStart = new UnityEvent();
    }
    public int CardsOnDeck
    {
        get { return deck.RemainingCards; }
    }

    public const int ResetCard = 2;

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
            players[i].OnPlayerReady.AddListener(OnPlayerReady);
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

    public void OnPlayerReady()
    {
        foreach(StupidPlayer player in players)
        {
            if (!player.isReady)
                return;
        }
        StartGame();
    }

    public void StartGame()
    {
        IsActive = true;
        playerInTurn = players[initialPlayer];
        playerInTurn.isPlaying = true;
        deck.StartGameStack();
        OnGameStart.Invoke();
        Debug.Log("Game Started");
    }

    public void PlayTurn()
    {
        if(playerInTurn.CanPlay(deck.PeekGameStack(),lower))
        {
            ApplyRules(playerInTurn.MakePlay());
            EndTurn();
        }
        else
        {
            ApplyRules(null);
            playAgain = true;
            SetNextTurn();
        }
    }

    public void EndTurn()
    {
        if (deck.RemainingCards > 0)
            playerInTurn.Draw(deck.GetCard());
        if (playerInTurn.HasWon)
        {
            FinishGame();
            return;
        }
        else
        {
            playerInTurn.isPlaying = false;
            SetNextTurn();
            playerInTurn.isPlaying = true;
        }
        OnPlay.Invoke();
    }

    public void FinishGame()
    {
        initialPlayer = players.IndexOf(playerInTurn);
        OnGameFinished.Invoke();
    }

    public void SetNextTurn()
    {
        if(playAgain)
        {
            playAgain = false;
            return;
        }
        int next = players.IndexOf(playerInTurn);
        if (ccw)
            next--;
        else
            next++;
        if (next >= numPlayers)
            next = 0;
        else if (next < 0)
            next = numPlayers - 1;

        playerInTurn = players[next];
    }

    public void ApplyRules(List<Card> cardsPlayed)
    {
        if(cardsPlayed == null)
        {
            playerInTurn.EatAllToHand(deck.EatAll());
            return;
        }

        deck.Put(cardsPlayed);

        if (FourOfAKind())
        {
            DiscardStack();
            return;
        }

        Card top = cardsPlayed[0];

        switch(top.CardValue)
        {
            case 4:
                Reverse();
                GoHigher();
                break;
            case 7:
                GoLower();
                break;
            case 8:
                SetNextTurn();
                break;
            case 10:
                DiscardStack();
                break;
            case 2:
                GoHigher();
                break;
        }
    }

    public bool FourOfAKind()
    {
        if (deck.GameStackSize < 4)
            return false;

        int rank = deck.PeekGameStack().CardValue;

        for(int i = deck.gameStack.Count - 1; i >= 0; i--)
        {
            if (deck.gameStack[i].CardValue != rank)
                return false;
        }

        return true;
    }

    public void EatAllCards()
    {
        playerInTurn.EatAllToHand(deck.EatAll());
    }

    public void SkipTurn()
    {
        SetNextTurn();
    }

    public void Reverse()
    {
        ccw = !ccw;
    }

    public void GoLower()
    {
        lower = true;
    }

    public void GoHigher()
    {
        lower = false;
    }

    public void DiscardStack()
    {
        deck.DiscardGameStack();
        playAgain = true;
    }
}

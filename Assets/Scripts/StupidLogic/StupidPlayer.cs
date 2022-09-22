using MudPuppyGames.CardGame;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public enum PlayerState { Locking, Drawing, NoDraw, NoHand, NoLocked}

public class StupidPlayer : CardPlayer
{
    public PlayerState state;
    [SerializeField] List<Card> hiddenCards;
    [SerializeField] List<Card> lockedCards;


    /*public UnityEvent<List<int>> OnSelectCards;
    public UnityEvent<int> OnDeselectCard;
    public UnityEvent<List<int>> OnLockCards;
    public UnityEvent<List<int>> OnPlayCards;
    public UnityEvent<List<int>> OnEatCards;
    public UnityEvent<bool> OnDrawCard;
    public UnityEvent OnHideCard;*/

    public StupidGameLogic gameRef;

    public bool HasWon
    {
        get { return hiddenCards.Count + lockedCards.Count + hand.Count == 0; }
    }

    public void SitDown(int faceDownCards)
    {
        hiddenCards = new List<Card>();
        lockedCards = new List<Card>();
        hand = new List<Card>();
        selectedCards = new List<int>();
        state = new PlayerState();

    }

    public void HideCard(Card card)
    {
        card.faceDown = true;
        hiddenCards.Add(card);
        //OnHideCard.Invoke();
    }
    public void LockCard()
    {
        foreach (int i in selectedCards)
            lockedCards.Add(hand[i]);

        for (int i = 0; i < selectedCards.Count; i++)
            hand.RemoveAt(i);

        selectedCards.Clear();
        //OnLockCards.Invoke(selectedCards);
    }
    public void UnlockCard(int lockedIdx)
    {
        Card clicked = lockedCards[lockedIdx];
        for(int i= 0; i < lockedCards.Count; i++)
        {
            if (clicked.Equals(lockedCards[i]))
            {
                hand.Add(lockedCards[i]);
            }
        }

    }
    
    public void TouchCard(int handIdx)
    {
        if (selectedCards.Contains(handIdx))
        {
            UnselectCard(handIdx);
            //OnDeselectCard.Invoke(handIdx);
        }
        else
        {
            if (state == PlayerState.Locking)
            {
                if (selectedCards.Count < gameRef.numCardsDown)
                    SelectCard(handIdx);
            }
            else if (state == PlayerState.NoLocked)
                SelectCard(handIdx);
            else
                SelectAllCardsWithSameValue(handIdx);
        }
    }

    public void SelectAllCardsWithSameValue(int handIdx)
    {
        Card clicked = hand[handIdx];
        for (int i = 0; i < hand.Count; i++)
        {
            if (clicked.Equals(hand[i]))
                SelectCard(i);
        }
        //OnSelectCards.Invoke(selectedCards);
    }

    public List<Card> MakePlay()
    {
        List<Card> play = null;
        switch (state)
        {
            case PlayerState.Locking:
                LockCard();
                CallReady();
                state = PlayerState.Drawing;
                break;
            case PlayerState.Drawing:
            case PlayerState.NoDraw:
                play = PlayFrom(hand);
                break;
            case PlayerState.NoHand:
                play = PlayFrom(lockedCards);
                break;
            case PlayerState.NoLocked:
                play = PlayFrom(hiddenCards);
                play[0].faceDown = false;
                break;
        }
        selectedCards.Clear();
        return play;
    }

    public void iadn()
    {
        Debug.Log("Ran this");
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(playerName + "= \nF:");
        foreach (Card card in hiddenCards)
        {
            sb.Append(card.ToString());
            sb.Append("||");
        }
        sb.Append("\nL:");
        foreach (Card card in lockedCards)
        {
            sb.Append(card.ToString());
            sb.Append("|");
        }
        sb.Append("\nH:");
        foreach (Card card in hand)
        {
            sb.Append(card.ToString());
            sb.Append("|");
        }

        return sb.ToString();
    }
}

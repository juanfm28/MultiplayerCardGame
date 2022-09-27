using MudPuppyGames.CardGame;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public enum PlayerState { Locking, WithHand, NoHand, NoLocked}

public class StupidPlayer : CardPlayer
{
    public PlayerState state;
    [SerializeField] List<Card> hiddenCards;
    [SerializeField] List<Card> lockedCards;


    public UnityEvent OnSelectHandCards;
    /*public UnityEvent<int> OnDeselectCard;
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

        foreach (Card locked in lockedCards)
            hand.Remove(locked);

        selectedCards.Clear();
        //OnLockCards.Invoke(selectedCards);
    }
    public void UnlockCard(int lockedIdx)
    {
        List<Card> unlocked = new List<Card>();
        Card clicked = lockedCards[lockedIdx];
        for(int i= 0; i < lockedCards.Count; i++)
        {
            if (clicked.CardValue == lockedCards[i].CardValue)
            {
                unlocked.Add(lockedCards[i]);
            }
        }

        foreach (Card u in unlocked)
            lockedCards.Remove(u);


    }
    
    public void SelectCardToPlay(int handIdx)
    {
        if (handIdx >= hand.Count)
            return;

        Debug.Log("Interacting with card: " + handIdx);
        switch (state)
        {
            case PlayerState.Locking:
                if(lockedCards.Count < gameRef.numCardsDown)
                {
                    selectedCards.Add(handIdx);
                    OnSelectHandCards.Invoke();
                }
                break;
            case PlayerState.WithHand:
                //If there is nothing, just select all with same value
                SelectAllCardsWithSameValue(handIdx);
                OnSelectHandCards.Invoke();
                break;
            case PlayerState.NoHand:
                SelectLockedCardsWithSameValue(handIdx);
                break;
            case PlayerState.NoLocked:
                if (handIdx >= gameRef.numCardsDown)
                    return;
                selectedCards.Clear();
                SelectCard(handIdx);
                break;
        }
    }

    public void SelectAllCardsWithSameValue(int handIdx)
    {
        selectedCards.Clear();
        Card clicked = hand[handIdx];
        for (int i = 0; i < hand.Count; i++)
        {
            if (clicked.CardValue == hand[i].CardValue)
                SelectCard(i);
        }
        //OnSelectCards.Invoke(selectedCards);
    }

    public void SelectLockedCardsWithSameValue(int lockedIdx)
    {
        if (lockedIdx >= gameRef.numCardsDown)
            return;
        selectedCards.Clear();
        Card wanted = lockedCards[lockedIdx];
        for(int i = 0; i < lockedCards.Count; i++)
        {
            if (wanted.CardValue == lockedCards[i].CardValue)
                SelectCard(i);
        }
        //OnSelectLockedCards.Invoke();
    }

    public List<Card> MakePlay()
    {
        List<Card> play = null;
        switch (state)
        {
            case PlayerState.Locking:
                LockCard();
                CallReady();
                state = PlayerState.WithHand;
                break;
            case PlayerState.WithHand:
                play = PlayFrom(hand);
                if (hand.Count == 0)
                    state = lockedCards.Count > 0 ? PlayerState.NoHand : PlayerState.NoLocked;
                break;
            case PlayerState.NoHand:
                play = PlayFrom(lockedCards);
                if (lockedCards.Count == 0)
                    state = PlayerState.NoLocked;
                break;
            case PlayerState.NoLocked:
                play = PlayFrom(hiddenCards);
                play[0].faceDown = false;
                break;
        }
        selectedCards.Clear();
        return play;
    }

    public bool CanPlay(Card heapTop,bool needsLower)
    {
        if (heapTop == null)
            return true;
        //If no hand and no locked, play hidden
        if (hand.Count == 0 && lockedCards.Count == 0)
        {
            return hiddenCards.Count > 0;
        }
        //else play from hand or locked
        else
        {
            List<Card> GroupToPlayFrom;
            //if no hand, play from locked. Otherwise play from hand
            if(hand.Count == 0)
                GroupToPlayFrom = lockedCards;
            else
                GroupToPlayFrom = hand;

            //check all cards in group until you find one that matches
            foreach(Card card in GroupToPlayFrom)
            {
                if (card.CardValue == StupidGameLogic.ResetCard || (card == heapTop) || (card < heapTop == needsLower))
                    return true;
            }
        }
        return false;
    }

    public void EatAllToHand(List<Card> cardsEaten)
    {
        Eat(hand, cardsEaten);
        state = PlayerState.WithHand;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(playerName + "= \nF:");
        foreach (Card card in hiddenCards)
        {
            sb.Append(card.ToString());
            if(state == PlayerState.NoLocked)
            {
                if(selectedCards.Contains(hiddenCards.IndexOf(card)))
                    sb.Append('*');
            }
            sb.Append("||");
        }
        sb.Append("\nL:");
        foreach (Card card in lockedCards)
        {
            sb.Append(card.ToString());
            if (state == PlayerState.NoHand)
            {
                if (selectedCards.Contains(lockedCards.IndexOf(card)))
                    sb.Append('*');
            }
            sb.Append("|");
        }
        sb.Append("\nH:");
        foreach (Card card in hand)
        {
            sb.Append(card.ToString());
            if (state == PlayerState.WithHand)
            {
                if (selectedCards.Contains(hand.IndexOf(card)))
                    sb.Append('*');
            }
            sb.Append("|");
        }

        return sb.ToString();
    }
}

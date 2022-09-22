using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace MudPuppyGames.CardGame
{
    public enum Suit
    {
        Hearts = 1,
        Clubs = 2,
        Diamonds = 3,
        Spades = 4
    }

    [Serializable]
    public class Card : IComparable
    {
        [SerializeField] private Suit _suit;
        [SerializeField] private short _rank;
        /*[HideInInspector]*/
        public bool faceDown;

        //public Sprite face;

        public Card(){ }

        public Card(Suit s, short r)
        {
            _suit = s;
            _rank = r;
        }

        public override string ToString()
        {
            string rank = "";
            string Suit = "";

            if(faceDown)
            {
                return "XX";
            }

            switch (_rank)
            {
                case 1:
                    rank = "A";
                    break;
                case 11:
                    rank = "J";
                    break;
                case 12:
                    rank = "Q";
                    break;
                case 13:
                    rank = "K";
                    break;
                default:
                    rank = "" + _rank;
                    break;
            }

            switch (_suit)
            {
                case CardGame.Suit.Hearts:
                    Suit = "H";
                    break;
                case CardGame.Suit.Clubs:
                    Suit = "C";
                    break;
                case CardGame.Suit.Diamonds:
                    Suit = "D";
                    break;
                case CardGame.Suit.Spades:
                    Suit = "S";
                    break;
            }

            return rank + Suit;
        }

        public override bool Equals(object value)
        {
            Card card = value as Card;
            return _rank == card._rank;
        }

        public override int GetHashCode()
        {
            return this._rank.GetHashCode();
        }

        int IComparable.CompareTo(object obj)
        {
            Card other = obj as Card;

            if (other > this)
                return 1;
            else if (other < this)
                return -1;
            else 
                return 0;
        }

        public static bool operator <(Card left, Card right)
        {
            return (left._rank == 1 ? 14 : left._rank) < right._rank;
        }

        public static bool operator >(Card left, Card right)
        {
            return (left._rank == 1 ? 14 : left._rank) > right._rank;
        }

        public static bool operator <=(Card left, Card right)
        {
            return (left._rank == 1 ? 14 : left._rank) <= right._rank;
        }

        public static bool operator >=(Card left, Card right)
        {
            return (left._rank == 1 ? 14 : left._rank) >= right._rank;
        }

    }
    
    public class Deck
    {
        [SerializeField]
        public List<Card> cards;
        public Deck()
        {
            cards = new List<Card>();
            foreach(Suit suit in (Suit[])Enum.GetValues(typeof(Suit)))
            {
                for (int i = 1; i < 14; i++)
                {
                    Card newCard = new Card(suit, (short)i);
                    cards.Add(newCard);
                }
            }
        }
    }

    [Serializable]
    public class PlayingDeck : Deck
    {
        [SerializeField]
        int numberOfDecks;

        List<Card> allPlayingCards;
        Stack<Card> drawStack;
        List<Card> discardPile;
        Stack<Card> gameStack;

        [SerializeField]
        public int cardBackIndex;
        public Sprite cardBack;

        public int RemainingCards
        {
            get { return drawStack.Count; }
        }

        public PlayingDeck(int deckMultiplier = 1) : base()
        { 
            allPlayingCards = new List<Card>();
            drawStack = new Stack<Card>();
            discardPile = new List<Card>();
            numberOfDecks = deckMultiplier;
            //cardBack = deck.backs[cardBackIndex];
            foreach (var card in cards)
            {
                for (int i = 0; i < numberOfDecks; i++)
                {
                    card.faceDown = true;
                    allPlayingCards.Add(card);
                }
            }

            Debug.Log("Deck initialized");
        }

        public void Shuffle()
        {
            int n = allPlayingCards.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                (allPlayingCards[n], allPlayingCards[k]) = (allPlayingCards[k], allPlayingCards[n]);
            }

            drawStack = new Stack<Card>(allPlayingCards);

            Debug.Log("Cards shuffled");
        }

        public Card GetCard()
        {
            if (RemainingCards == 0)
                return null;

            return drawStack.Pop();
        }

        public List<Card> EatAll()
        {
            List<Card> eatenCards = new List<Card>(gameStack);
            gameStack.Clear();
            return eatenCards;
        }

        public void DiscardGameStack()
        {
            DiscardAll(gameStack);
            gameStack.Clear();
        }

        public void DiscardAll(IEnumerable<Card> discardedCards)
        {
            discardPile.AddRange(discardedCards);
        }

        public Card PeekGameStack()
        {
            return gameStack.Peek();
        }

        public void StartGameStack()
        {
            Card first = drawStack.Pop();
            first.faceDown = false;
            gameStack = new Stack<Card>();
            gameStack.Push(first);
        }

        public void Put(List<Card> play)
        {
            foreach(Card card in play)
            {
                card.faceDown = false;
                gameStack.Push(card);
            }
        }
    }

    public static class ThreadSafeRandom
    {
        [ThreadStatic] private static System.Random Local;

        public static System.Random ThisThreadsRandom
        {
            get { return Local ?? (Local = new System.Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MudPuppyGames.CardGame
{
    public class CardPlayer : MonoBehaviour
    {
        public string playerName;
        public bool isPlaying;
        public bool isReady;
        [SerializeField] protected List<Card> hand;
        [SerializeField] protected List<int> selectedCards;
        public UnityEvent OnPlayerReady;

        public void SetActivePlayer(bool state)
        {
            isPlaying = state;
        }

        public List<Card> PlayFrom(List<Card> container)
        {
            List<Card> play = new List<Card>();
            for (int i = 0; i < selectedCards.Count; i++)
                play.Add(container[i]);
            return play;
        }

        public void Draw(Card card)
        {
            card.faceDown = false;
            hand.Add(card);
        }

        public void Eat(List<Card> container, List<Card> cardsToEat)
        {
            container.AddRange(cardsToEat);
        }

        public void UnselectCard(int handIdx)
        {
            if (selectedCards.Contains(handIdx))
                selectedCards.Remove(handIdx);
        }

        public void SelectCard(int handIdx)
        {
            if (selectedCards.Contains(handIdx))
                return;
            else
                selectedCards.Add(handIdx);
        }

        public void CallReady()
        {
            isReady = true;
            Debug.Log("d");
            OnPlayerReady.Invoke();
            Debug.Log(playerName+" is ready");
        }
    }
}

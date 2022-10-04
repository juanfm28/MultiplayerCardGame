using MudPuppyGames.CardGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFactory : MonoBehaviour
{
    public CardFaces cardFaces;
    public GameObject cardPrefab;

    public void CreateCard(Card cardToCreate, bool isFacingDown = false)
    {
        GameObject newCard = Instantiate(cardPrefab, transform);
        PhysicalCard pc = newCard.GetComponent<PhysicalCard>();
        pc.face = cardFaces.faces.Find(x => x.name == cardToCreate.ToString()).face;
        pc.back = cardFaces.back;
        pc.value = cardToCreate;
        pc.owner = transform;
        pc.isFacingDown = isFacingDown;
    }
}

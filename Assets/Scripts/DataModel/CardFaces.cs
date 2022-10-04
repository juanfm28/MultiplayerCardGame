using MudPuppyGames.CardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CardFaces", menuName = "ScriptableObjects/CardFaces", order = 1)]
public class CardFaces : ScriptableObject
{
    public Sprite back;
    public List<CardFace> faces;

    public CardFaces()
    {
        faces = new List<CardFace>();
    }
}

[Serializable]
public class CardFace
{
    public string name;
    public Sprite face;
}

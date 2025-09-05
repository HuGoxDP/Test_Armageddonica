using System.Collections.Generic;
using _Project.Scripts.Architecture.Cards.Data;
using UnityEngine;

namespace _Project.Scripts.Architecture.Cards.Deck
{
    [CreateAssetMenu(menuName = "Game/CardSystem/New Card Deck")]
    public class CardDeck : ScriptableObject
    {
       [field: SerializeField] public List<BaseCardData> Cards {get; private set; } = new();
    }
}
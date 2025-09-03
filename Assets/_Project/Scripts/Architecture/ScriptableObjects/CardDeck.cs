using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Architecture.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Game/CardSystem/New Card Deck")]
    public class CardDeck : ScriptableObject
    {
       [field: SerializeField] public List<BaseCardData> Cards {get; private set; } = new();
    }
}
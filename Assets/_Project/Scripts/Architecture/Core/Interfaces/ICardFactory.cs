using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Cards.Deck;
using _Project.Scripts.Architecture.Cards.Runtime;
using UnityEngine;

namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface ICardFactory
    {
        CardUI CreateCardFromDeck(CardDeck cardDeck, int index, Transform parent);
        CardUI CreateCardFromDeck(CardDeck cardDeck, Transform parent);
        CardUI CreateCard(BaseCardData cardData, Transform parent);
    }
}
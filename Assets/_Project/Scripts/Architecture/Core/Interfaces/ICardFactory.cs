using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Cards.Deck;
using _Project.Scripts.Architecture.Cards.Runtime;

namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface ICardFactory
    {
        CardUI CreateCardFromDeck(CardDeck cardDeck, int index);
        CardUI CreateCardFromDeck(CardDeck cardDeck);
        CardUI CreateCard(BaseCardData cardData);
    }
}
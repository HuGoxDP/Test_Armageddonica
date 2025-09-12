using System.Collections.Generic;
using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Cards.Deck;

namespace _Project.Scripts.Architecture.SelectCardMenu
{
    public interface ICardGeneratorStrategy
    {
        List<BaseCardData> GenerateCards(CardDeck deck, int count);
    }
}
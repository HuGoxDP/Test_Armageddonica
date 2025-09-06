using System.Collections.Generic;
using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Cards.Runtime;

namespace _Project.Scripts.Architecture.Core.Interfaces
{
    interface IPlayerHand
    {
        public event System.EventHandler<CardUI> OnCardAdded;
        public event System.EventHandler<CardUI> OnCardRemoved;

        public IReadOnlyList<CardUI> Cards { get; }

        public void AddCard(BaseCardData cardData);
        public void RemoveCard(CardUI card);
        public int GetCardIndex(CardUI card);
    }
}
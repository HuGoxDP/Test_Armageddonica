using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Cards.Runtime;

namespace _Project.Scripts.Architecture.Core.Interfaces
{
    interface IPlayerCardHand
    {
        public void AddCard(BaseCardData cardData);
        public void RemoveCard(CardUI card);
    }
}
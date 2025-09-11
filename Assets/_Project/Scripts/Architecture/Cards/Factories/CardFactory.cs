using System;
using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Cards.Deck;
using _Project.Scripts.Architecture.Cards.Runtime;
using _Project.Scripts.Architecture.Core.Dependency_Injection;
using _Project.Scripts.Architecture.Core.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Architecture.Cards.Factories
{
    public class CardFactory : MonoBehaviour, ICardFactory
    {
        [SerializeField] private CardUI _cardPrefab;
        [SerializeField] private Transform _cardsParent;
        private void Start()
        {
           ServiceLocator.Register<ICardFactory>(this);
        }
        
        public CardUI CreateCardFromDeck(CardDeck cardDeck, int index)
        {
            if (cardDeck == null)
            {
                throw new ArgumentNullException(nameof(cardDeck), "Card deck cannot be null");
            }

            if (index < 0 || index >= cardDeck.Cards.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range of the card deck");
            }

            var cardData = cardDeck.Cards[index];
            return CreateCard(cardData);
        }

        public CardUI CreateCardFromDeck(CardDeck cardDeck)
        {
            if (cardDeck == null)
            {
                throw new ArgumentNullException(nameof(cardDeck), "Card deck cannot be null");
            }
            if (cardDeck.Cards.Count == 0)
            {
                throw new InvalidOperationException("Card deck is empty");
            }
            var randomIndex = UnityEngine.Random.Range(0, cardDeck.Cards.Count);
            var cardData = cardDeck.Cards[randomIndex];
            return CreateCard(cardData);
        }
        
        public CardUI CreateCard(BaseCardData cardData)
        {
            if (cardData == null)
            {
                throw new ArgumentNullException(nameof(cardData), "Card data cannot be null");
            }

            var cardInstance = Instantiate(_cardPrefab, _cardsParent);
            cardInstance.SetCardData(cardData);
            return cardInstance;
        }
    }
}
using System;
using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Cards.Deck;
using _Project.Scripts.Architecture.Cards.Runtime;
using UnityEngine;

namespace _Project.Scripts.Architecture.Cards.Factories
{
    public class CardFactory : MonoBehaviour
    {
        // Відповідає за створення карток 
        private static CardFactory _instance;
        public static CardFactory Instance => _instance ??= FindFirstObjectByType<CardFactory>();
        
        [SerializeField] private CardUI _cardPrefab;
        [SerializeField] private Transform _cardsParent;
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
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
using System;
using _Project.Scripts.Architecture.ScriptableObjects;
using NaughtyAttributes;
using UnityEngine;

namespace _Project.Scripts.Architecture.Services
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private CardDeck _cardDeck;
        [SerializeField] private PlayerCardHand _playerCardHand;

        public void Awake()
        {
            if (_cardDeck == null)
            {
                Debug.LogError("Card deck is not assigned.");
            }
            if (_playerCardHand == null)
            {
                Debug.LogError("Player card hand is not assigned.");
            }
        }

        private void Start()
        {
            for (int i = 0; i < 3; i++)
            {
                AddCardToPlayerHand();
            }
        }

        [Button("Add Card From Deck To Player Hand")]
        private void AddCardToPlayerHand()
        {
            if (_cardDeck == null || _playerCardHand == null) return;
            
            var cardData = GetRandomCardFromDeck(_cardDeck);
            _playerCardHand.AddCard(cardData);
        }

        private BaseCardData GetRandomCardFromDeck(CardDeck cardDeck)
        {
            if(cardDeck.Cards.Count == 0)
            {
                Debug.LogWarning("Card deck is empty.");
                return null;
            }
            var randomIndex = UnityEngine.Random.Range(0, cardDeck.Cards.Count);
            return cardDeck.Cards[randomIndex];
        }
    }
}
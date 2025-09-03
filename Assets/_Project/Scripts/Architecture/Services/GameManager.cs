using System;
using _Project.Scripts.Architecture.Factories;
using _Project.Scripts.Architecture.ScriptableObjects;
using _Project.Scripts.Architecture.UI;
using NaughtyAttributes;
using UnityEngine;

namespace _Project.Scripts.Architecture.Services
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private CardDeck _cardDeck;
        [SerializeField] private PlayerCardHolder _playerCardHolder;

        private CardFactory _cardFactory;
        private ICard _lastAddedCard;

        private void Awake()
        {
            _cardFactory = CardFactory.Instance;
            Validate();
        }

        private void Validate()
        {
            if (_cardFactory == null)
            {
                throw new Exception("CardFactory instance is not found in the scene.");
            }
        }

        [Button("Add Card From Deck To Player Hand")]
        private void AddCardToPlayerHand()
        {
            var card = _cardFactory.CreateCardFromDeck(_cardDeck);
            _playerCardHolder.AddCard(card.RectTransform);
            _lastAddedCard = card;
        }

        [Button("Remove Card From Player Hand")]
        private void RemoveCardFromPlayerHand()
        {
            var card = _lastAddedCard;
            if (card != null)
            {
                _playerCardHolder.RemoveCard(card.RectTransform);
                _lastAddedCard = null;
            }
        }
    }
}
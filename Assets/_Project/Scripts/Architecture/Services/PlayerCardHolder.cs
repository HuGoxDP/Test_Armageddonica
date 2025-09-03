using System.Collections.Generic;
using _Project.Scripts.Architecture.UI;
using UnityEngine;

namespace _Project.Scripts.Architecture.Services
{
    [RequireComponent(typeof(CardLayoutController))]
    public class PlayerCardHolder : MonoBehaviour
    {
        private CardLayoutController _cardLayoutController;
        private List<RectTransform> _cards;

        private void Awake()
        {
            _cardLayoutController = GetComponent<CardLayoutController>();
            _cards = new List<RectTransform>();
        }

        private void OnDestroy()
        {
            RemoveAllCards();
        }

        /// <summary>
        /// Adds a card to the player's hand and updates the layout.
        /// </summary>
        /// <param name="card">The card to add.</param>
        public void AddCard(RectTransform card)
        {
            _cards.Add(card);
            UpdateCardLayout();
        }
        
        /// <summary>
        /// Removes a card from the player's hand and updates the layout.
        /// </summary>
        /// <param name="card">The card to remove</param>
        public void RemoveCard(RectTransform card)
        {
            if (_cards.Contains(card))
            {
                _cards.Remove(card);
                card.gameObject.SetActive(false);
                
                UpdateCardLayout();
            }
        }
        
        private void RemoveAllCards()
        {
            foreach (var card in _cards)
            {
                card.gameObject.SetActive(false);
            }
            _cards.Clear();
        }

        private void UpdateCardLayout()
        {
            _cardLayoutController.UpdateLayout(_cards);
        }
    }
}
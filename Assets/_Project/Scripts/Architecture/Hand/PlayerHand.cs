using System;
using System.Collections.Generic;
using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Cards.Factories;
using _Project.Scripts.Architecture.Cards.Runtime;
using _Project.Scripts.Architecture.Core.GameStates;
using _Project.Scripts.Architecture.Core.Interfaces;
using _Project.Scripts.Architecture.Layout;
using UnityEngine;


namespace _Project.Scripts.Architecture.Hand
{
    /// <summary> Manages the player's hand of cards, including adding, removing, and arranging cards. </summary>
    [RequireComponent(typeof(CardPlacementHandler))]
    public class PlayerHand : GameControllable, IPlayerHand
    {
        public event EventHandler<CardUI> OnCardAdded;
        public event EventHandler<CardUI> OnCardRemoved;
        public IReadOnlyList<CardUI> Cards => _cards.AsReadOnly();

        [SerializeField] private LayoutController _layoutController;

        private List<CardUI> _cards;
        private CardFactory _cardFactory;

        private void Awake()
        {
            _cards = new List<CardUI>();
            _cardFactory = CardFactory.Instance;
            _layoutController.Initialize(new LinearLayoutStrategy());
        }

        protected override void OnDestroy()
        {
            //use remove method
            var count = _cards.Count - 1;

            for (var i = count; i >= 0; i--)
            {
                RemoveCard(_cards[i]);
            }
        }

        public override void OnGameStateChanged(object sender, GameState newState) =>
            gameObject.SetActive(newState == GameState.CardPlacementTurn);


        /// <summary> Adds a card to the player's hand. </summary>
        public void AddCard(BaseCardData cardData)
        {
            if (cardData == null)
            {
                Debug.LogWarning("Cannot add a null card to the player's hand.");
                return;
            }

            var card = _cardFactory.CreateCard(cardData);
            if (card == null)
            {
                Debug.LogWarning("CardFactory failed to create a card.");
                return;
            }

            if (card.TryGetComponent<CardInteractionHandler>(out var interactionHandler))
            {
                interactionHandler.OnCardDragStarted += OnCardDraggedStarted;
                interactionHandler.OnCardDragged += OnCardDragged;
                interactionHandler.OnCardDragEnded += OnCardDraggedEnded;
                interactionHandler.OnCardHovered += OnCardHovered;
            }

            _cards.Add(card);
            _layoutController.AddCard(card.transform);
            OnCardAdded?.Invoke(this, card);
        }
        
        /// <summary> Removes a card from the player's hand. </summary>
        public void RemoveCard(CardUI card)
        {
            if (card == null)
            {
                Debug.LogWarning("Cannot remove a null card from the player's hand.");
                return;
            }

            if (!_cards.Contains(card))
            {
                Debug.LogWarning("The specified card is not in the player's hand.");
                return;
            }
            
            if (card.TryGetComponent<CardInteractionHandler>(out var interactionHandler))
            {
                interactionHandler.OnCardDragStarted -= OnCardDraggedStarted;
                interactionHandler.OnCardDragged -= OnCardDragged;
                interactionHandler.OnCardDragEnded -= OnCardDraggedEnded;
                interactionHandler.OnCardHovered -= OnCardHovered;
            }

            _cards.Remove(card);
            _layoutController.RemoveCard(card.transform);
            OnCardRemoved?.Invoke(this, card);
        }
        
        public int GetCardIndex(CardUI card) => _cards.IndexOf(card);
        
        private void OnCardDraggedStarted(object sender, DragEventArgs e)
        {
        }
        

        private void OnCardDragged(object sender, DragEventArgs e)
        {
        }
        

        private void OnCardDraggedEnded(object sender, DragEventArgs e)
        {
        }

        private void OnCardHovered(object sender, CardUI e)
        {
            _layoutController.UpdateHoverLayout(GetCardIndex(e));
        }

    }
}
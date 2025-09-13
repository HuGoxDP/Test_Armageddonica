using System;
using System.Collections.Generic;
using _Project.Scripts.Architecture.Cards;
using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Cards.Runtime;
using _Project.Scripts.Architecture.Core.Dependency_Injection;
using _Project.Scripts.Architecture.Core.GameStates;
using _Project.Scripts.Architecture.Core.Interfaces;
using _Project.Scripts.Architecture.Enums;
using _Project.Scripts.Architecture.Layout;
using UnityEngine;


namespace _Project.Scripts.Architecture.Hand
{
   
    /// <summary> Manages the player's hand of cards, including adding, removing, and arranging cards. </summary>
    public class PlayerHand : GameControllable, IPlayerHand
    {
        public event EventHandler<CardUI> OnCardAdded;
        public event EventHandler<CardUI> OnCardRemoved;
        public IReadOnlyList<CardUI> Cards => _cards.AsReadOnly();

        [SerializeField] private LayoutController _layoutController;
        [SerializeField] private CardPlacementHandler _cardPlacementHandler;

        private List<CardUI> _cards;
        private ICardFactory _cardFactory;
        private Vector3 _originalDraggedCardPosition;
        private float _originalDraggedCardScale;
        private CanvasGroup _originalDraggedCardCanvasGroup;
        private int _originalDraggedCardSiblingIndex;
        private CardUI _hoveredCard;
        private bool _isCardDragging;
        
        private void Awake()
        {
            ServiceLocator.Register<IPlayerHand>(this);
            _cards = new List<CardUI>();
            _layoutController.Initialize(new LinearLayoutStrategy());
        }
       
        private void Start()
        {
            _cardFactory ??= ServiceLocator.Get<ICardFactory>();

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

        public override void SetMatchController(MatchController matchController)
        {
            base.SetMatchController(matchController);
            matchController.OnGameStateChanged += OnGameStateChanged;
            _cardFactory ??= ServiceLocator.Get<ICardFactory>();
        }
        
        /// <summary> Adds a card to the player's hand. </summary>
        public void AddCard(BaseCardData cardData)
        {
            if (cardData == null)
            {
                Debug.LogWarning("Cannot add a null card to the player's hand.");
                return;
            }

            var card = _cardFactory?.CreateCard(cardData, _layoutController.transform);
            if (card == null)
            {
                Debug.LogWarning("CardFactory failed to create a card.");
                return;
            }

            if (card.TryGetComponent<CardEventHandler>(out var interactionHandler))
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
            
            if (card.TryGetComponent<CardEventHandler>(out var interactionHandler))
            {
                interactionHandler.OnCardDragStarted -= OnCardDraggedStarted;
                interactionHandler.OnCardDragged -= OnCardDragged;
                interactionHandler.OnCardDragEnded -= OnCardDraggedEnded;
                interactionHandler.OnCardHovered -= OnCardHovered;
            }

            _cards.Remove(card);
            _layoutController.RemoveCard(card.transform);
            OnCardRemoved?.Invoke(this, card);
            Destroy(card.gameObject);
        }
        
        public int GetCardIndex(CardUI card) => _cards.IndexOf(card);
        
        
        private void OnCardDraggedStarted(object sender, DragEventArgs e)
        {
            _isCardDragging = true;
            var card = e.Card;

            _originalDraggedCardPosition = card.transform.position;
            _originalDraggedCardScale = card.transform.localScale.x;
            _originalDraggedCardCanvasGroup = card.GetComponent<CanvasGroup>();
            _originalDraggedCardSiblingIndex = card.transform.GetSiblingIndex();
            
            card.transform.SetAsLastSibling();
            card.transform.localScale = Vector3.one * 0.25f;
            
            if (_originalDraggedCardCanvasGroup != null)
            {
                _originalDraggedCardCanvasGroup.blocksRaycasts = false;
                _originalDraggedCardCanvasGroup.alpha = 0.5f;
            }
            _cardPlacementHandler.StartPlacingCard(card);
        }

        private void OnCardDragged(object sender, DragEventArgs e)
        {
            var card = e.Card;
            var newPosition = e.EventData.position;
            card.transform.position = newPosition;
        }
        
        private async void OnCardDraggedEnded(object sender, DragEventArgs e)
        {
            try
            {
                var card = e.Card;

                card.transform.position = _originalDraggedCardPosition;
                card.transform.localScale = Vector3.one * _originalDraggedCardScale;
                card.transform.SetSiblingIndex(_originalDraggedCardSiblingIndex);

                if (_originalDraggedCardCanvasGroup != null)
                {
                    _originalDraggedCardCanvasGroup.blocksRaycasts = true;
                    _originalDraggedCardCanvasGroup.alpha = 1f;
                }

                var placed = await _cardPlacementHandler.TryPlaceCard(card, e.EventData.position);
                if (placed)
                {
                    _originalDraggedCardCanvasGroup = null;
                    _originalDraggedCardScale = 0;
                    _originalDraggedCardPosition = Vector3.zero;
                    _originalDraggedCardSiblingIndex = -1;

                    _isCardDragging = false;
                    RemoveCard(card);
                    MatchController.NextTurn();
                    return;
                }

                _isCardDragging = false;
                _layoutController.UpdateHoverLayout(GetCardIndex(card));
                _originalDraggedCardCanvasGroup = null;
                _originalDraggedCardScale = 0;
                _originalDraggedCardPosition = Vector3.zero;
                _originalDraggedCardSiblingIndex = -1;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private void OnCardHovered(object sender, CardUI e)
        {
            if (_isCardDragging) return;
            if (_hoveredCard != null && _hoveredCard == e) return;
            _hoveredCard = e;
            _layoutController.UpdateHoverLayout(GetCardIndex(e));
        }

        protected override void OnGameStateChanged(object sender, GameState newState) =>
            gameObject.SetActive(newState == GameState.CardPlacementTurn);
    }
}
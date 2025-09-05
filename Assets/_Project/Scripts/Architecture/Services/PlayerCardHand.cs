using System.Collections.Generic;
using _Project.Scripts.Architecture.Factories;
using _Project.Scripts.Architecture.ScriptableObjects;
using _Project.Scripts.Architecture.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.Architecture.Services
{
    public class PlayerCardHand : MonoBehaviour
    {
        private readonly List<CardUI> _cards = new();
        private CardFactory _cardFactory;
        private CardLineLayoutController _cardLineLayoutGroup;
        
        private readonly LinearLayoutStrategy _linearLayoutStrategy = new();
        private readonly HoverLinearLayoutStrategy _hoverLinearLayoutStrategy = new();
        
        private Vector3 _draggedCardOriginalPosition;
        private CardUI _draggedCard;
        
        private void Awake()
        {
            _cardFactory = CardFactory.Instance;
            _cardLineLayoutGroup = GetComponent<CardLineLayoutController>();
        }
        
        private void OnDestroy()
        {
            foreach (var card in _cards)
            {
                if (card.TryGetComponent<CardInteractionHandler>(out var interactionHandler))
                {
                    interactionHandler.OnCardHovered -= OnCardHovered;
                }
            }
            
            _cards.Clear();
        }

        /// <summary> Adds a card to the player's hand. </summary>
        public void AddCard(BaseCardData cardData)
        {
            if (cardData == null)
            {
                Debug.LogWarning("Cannot add a null card to the player's hand.");
                return;
            }

            var cardUI = _cardFactory.CreateCard(cardData);
            _cards.Add(cardUI);
            if (cardUI.TryGetComponent<CardInteractionHandler>(out var interactionHandler))
            {
                interactionHandler.OnCardHovered += OnCardHovered;
                interactionHandler.OnCardDragStarted += OnCardDragStarted;
                interactionHandler.OnCardDragEnded += OnCardDragEnded;
                interactionHandler.OnCardDragged += OnCardDragged;
            }
            
            _cardLineLayoutGroup.UpdateLayout(_linearLayoutStrategy, new LayoutStrategyData(_cards));
        }

        private void OnCardDragged(CardUI cardUI, PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        private void OnCardDragEnded(CardUI cardUI, PointerEventData eventData)
        {
            CanvasGroup canvasGroup = cardUI.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.blocksRaycasts = true;
            }
        }

        private void OnCardDragStarted(CardUI cardUI, PointerEventData eventData)
        {
            _draggedCard = cardUI;
            _draggedCardOriginalPosition = cardUI.transform.position;
            
            CanvasGroup canvasGroup = cardUI.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.blocksRaycasts = false;
            }
        }

        /// <summary> Removes a card from the player's hand. </summary>
        public void RemoveCard(CardUI cardUI)
        {
            if (cardUI == null)
            {
                Debug.LogWarning("Cannot remove a null card from the player's hand.");
                return;
            }
            
            if (_cards.Contains(cardUI))
            {
                if (cardUI.TryGetComponent<CardInteractionHandler>(out var interactionHandler))
                {
                    interactionHandler.OnCardHovered -= OnCardHovered;
                }
                
                _cards.Remove(cardUI);
                Destroy(cardUI.gameObject);
                _cardLineLayoutGroup.UpdateLayout(_linearLayoutStrategy, new LayoutStrategyData(_cards));
            }
            else
            {
                Debug.LogWarning("The specified card is not in the player's hand.");
            }
        }

        private void OnCardHovered(CardUI card)
        {
            _cardLineLayoutGroup.UpdateLayout(_hoverLinearLayoutStrategy, new HoveredLayoutStrategyData(_cards, card));
        }
    }
}
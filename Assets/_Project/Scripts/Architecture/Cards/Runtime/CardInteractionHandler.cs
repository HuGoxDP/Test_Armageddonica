using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.Architecture.Cards.Runtime
{
    /// <summary> Handles user interactions with a card, such as hovering and dragging./// </summary>
    /// <remarks> Attach this script to the CardUI prefab. </remarks>
    public class CardInteractionHandler : MonoBehaviour, IPointerEnterHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public event EventHandler<CardUI> OnCardHovered;
        public event EventHandler<DragEventArgs> OnCardDragStarted;
        public event EventHandler<DragEventArgs> OnCardDragEnded;
        public event EventHandler<DragEventArgs> OnCardDragged;
        
        private CardUI _cardUi;

        private void Awake() => _cardUi = GetComponent<CardUI>();
        
        public void OnPointerEnter(PointerEventData eventData) => OnCardHovered?.Invoke(this, _cardUi);
        
        public void OnDrag(PointerEventData eventData) => OnCardDragged?.Invoke(this, new DragEventArgs(_cardUi, eventData));

        public void OnBeginDrag(PointerEventData eventData) => OnCardDragStarted?.Invoke(this, new DragEventArgs(_cardUi, eventData));

        public void OnEndDrag(PointerEventData eventData) => OnCardDragEnded?.Invoke(this, new DragEventArgs(_cardUi, eventData));
    }
    
    public class DragEventArgs : EventArgs
    {
        public CardUI Card { get; }
        public PointerEventData EventData { get; }

        public DragEventArgs(CardUI card, PointerEventData eventData)
        {
            Card = card;
            EventData = eventData;
        }
    }
}
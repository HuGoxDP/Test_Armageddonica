using System;
using _Project.Scripts.Architecture.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.Architecture.Services
{
    public class CardInteractionHandler : MonoBehaviour, IPointerEnterHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public event Action<CardUI> OnCardHovered;
        public event Action<CardUI, PointerEventData> OnCardDragStarted;
        public event Action<CardUI, PointerEventData> OnCardDragEnded;
        public event Action<CardUI, PointerEventData> OnCardDragged;
        
        private CardUI _cardUi;

        private void Awake() => _cardUi = GetComponent<CardUI>();
        
        public void OnPointerEnter(PointerEventData eventData) => OnCardHovered?.Invoke(_cardUi);
        
        public void OnDrag(PointerEventData eventData) => OnCardDragged?.Invoke(_cardUi, eventData);

        public void OnBeginDrag(PointerEventData eventData) => OnCardDragStarted?.Invoke(_cardUi, eventData);

        public void OnEndDrag(PointerEventData eventData) => OnCardDragEnded?.Invoke(_cardUi, eventData);
    }
}
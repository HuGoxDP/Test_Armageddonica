using System;
using _Project.Scripts.Architecture.Core.Dependency_Injection;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.Architecture.Cards.Runtime
{
    /// <summary>
    ///  This class is used to handle events from CardUI.
    /// </summary>
    public class CardEventHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerExitHandler
    {
        public event EventHandler<CardUI> OnCardClicked;
        public event EventHandler<CardUI> OnCardHovered;
        public event EventHandler<CardUI> OnCardUnhover;
        public event EventHandler<DragEventArgs> OnCardDragStarted;
        public event EventHandler<DragEventArgs> OnCardDragEnded;
        public event EventHandler<DragEventArgs> OnCardDragged;
        
        private CardUI _cardUi;
        private void Awake() => _cardUi ??= GetComponent<CardUI>();
        public void OnPointerClick(PointerEventData eventData)
        {
            OnCardClicked?.Invoke(this, _cardUi);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnCardHovered?.Invoke(this, _cardUi);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnCardDragged?.Invoke(this, new DragEventArgs(_cardUi, eventData));
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnCardDragStarted?.Invoke(this, new DragEventArgs(_cardUi, eventData));
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnCardDragEnded?.Invoke(this, new DragEventArgs(_cardUi, eventData));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnCardUnhover?.Invoke(this, _cardUi);
        }
    }
}
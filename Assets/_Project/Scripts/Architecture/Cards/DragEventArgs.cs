using System;
using UnityEngine.EventSystems;

namespace _Project.Scripts.Architecture.Cards.Runtime
{
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
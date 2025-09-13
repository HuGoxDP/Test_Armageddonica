using System;
using _Project.Scripts.Architecture.Cards.Runtime;
using UnityEngine.EventSystems;

namespace _Project.Scripts.Architecture.Cards
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
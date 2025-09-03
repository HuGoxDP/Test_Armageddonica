using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.Architecture
{
    public class PointerEnterExitHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event EventHandler PointerEnter;
        public event EventHandler PointerExit;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEnter?.Invoke(this, EventArgs.Empty);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExit?.Invoke(this, EventArgs.Empty);
        }
    }
}
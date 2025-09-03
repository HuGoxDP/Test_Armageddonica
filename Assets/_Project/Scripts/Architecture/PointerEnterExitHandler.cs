using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.Architecture
{
    public class PointerEnterExitHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event EventHandler<PointerEnterExitHandler> PointerEnter;
        public event EventHandler<PointerEnterExitHandler> PointerExit;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEnter?.Invoke(this, this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExit?.Invoke(this, this);
        }
    }
}
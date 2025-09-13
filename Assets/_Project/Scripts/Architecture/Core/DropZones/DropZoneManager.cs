using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Scripts.Architecture.Cards.Runtime;
using _Project.Scripts.Architecture.Core.Dependency_Injection;
using _Project.Scripts.Architecture.Core.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Architecture.Core.DropZones
{
    public class DropZoneManager : MonoBehaviour
    {
        private readonly List<IDropZone> _dropZones = new List<IDropZone>();

        private void Awake()
        {
            ServiceLocator.Register(this);
        }

        public void RegisterDropZone(IDropZone dropZone)
        {
            if (!_dropZones.Contains(dropZone))
            {
                _dropZones.Add(dropZone);
            }
        }
        
        public void UnregisterDropZone(IDropZone dropZone)
        {
            _dropZones.Remove(dropZone);
        }
        
        public void StartDragPreview(CardUI card)
        {
            foreach (var zone in _dropZones)
            {
                if (zone.CanAcceptCard(card))
                {
                    zone.StartDragPreview(card);
                }
            }
        }
        
        public void EndDragPreview()
        {
            foreach (var zone in _dropZones)
            {
                zone.EndDragPreview();
            }
        }

        public async Task<bool> TryDropCard(CardUI card, Vector2 screenPosition)
        {
            foreach (var zone in _dropZones)
            {
                if (zone.IsPositionInZone(screenPosition) && zone.CanAcceptCard(card))
                {
                    return await zone.TryDropCard(card, screenPosition);
                }
            }
            
            return false;
        }
    }
}
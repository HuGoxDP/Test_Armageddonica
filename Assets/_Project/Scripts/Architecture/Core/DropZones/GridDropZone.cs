using System;
using System.Threading.Tasks;
using _Project.Scripts.Architecture.Cards.Runtime;
using _Project.Scripts.Architecture.Core.Dependency_Injection;
using _Project.Scripts.Architecture.Core.Interfaces;
using _Project.Scripts.Architecture.Grid.Core;
using UnityEngine;

namespace _Project.Scripts.Architecture.Core.DropZones
{
    public class GridDropZone : MonoBehaviour, IDropZone
    {
        private GridSystem _gridSystem;
        private Camera _mainCamera;
        private VFXManager.VFXManager _vfxManager;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            ServiceLocator.Get<DropZoneManager>().RegisterDropZone(this);
            _gridSystem = ServiceLocator.Get<GridSystem>();
            _vfxManager = ServiceLocator.Get<VFXManager.VFXManager>();
        }

        private void OnDestroy()
        {
            ServiceLocator.Get<DropZoneManager>()?.UnregisterDropZone(this);
        }

        public bool CanAcceptCard(CardUI card)
        {
            return true;
        }

        public async Task<bool> TryDropCard(CardUI card, Vector2 screenPosition)
        {
            var worldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, _mainCamera.nearClipPlane));
            var position = new Vector3(worldPosition.x, worldPosition.y, 0);
            
            _gridSystem.UnhighlightedCells();
            _gridSystem.EnableTooltips(true);
            
            var isPlaced = await _gridSystem.TryPlaceCardOnGrid(card, position);
            if (!isPlaced)
                return false;
            
            if(_gridSystem.TryGetCell(worldPosition, out var cellAtPosition))
            { 
                PlayPlaceEffect(cellAtPosition);
            }
            await Task.Delay(300);
            return true;
        }

        private async void PlayPlaceEffect(IGridCell cellAtPosition)
        {
            try
            {
                var effect = _vfxManager.PlayEffect("Smoke_PlaceCard", cellAtPosition.GameObject.transform.position, Quaternion.identity);
                await Task.Delay(500);
                _vfxManager.ReturnEffectToPool(effect,"Smoke_PlaceCard");
            }
            catch (Exception e)
            {
                 Debug.LogException(e);
            }
        }

        public void StartDragPreview(CardUI card)
        {
            _gridSystem.HighlightSuitableCells(card);
            _gridSystem.EnableTooltips(false);
        }

        public void EndDragPreview()
        {
            _gridSystem.UnhighlightedCells();
            _gridSystem.EnableTooltips(true);
        }

        public bool IsPositionInZone(Vector2 screenPosition)
        {
            var worldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, _mainCamera.nearClipPlane));
            var position = new Vector3(worldPosition.x, worldPosition.y, 0);
            
            return _gridSystem.TryGetCell(position, out _);
        }
    }
}
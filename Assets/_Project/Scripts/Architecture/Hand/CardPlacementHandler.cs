using _Project.Scripts.Architecture.Cards.Runtime;
using _Project.Scripts.Architecture.Grid.Core;
using UnityEngine;

namespace _Project.Scripts.Architecture.Hand
{
    /// <summary> Handles the placement of cards onto the grid. </summary>
    public class CardPlacementHandler: MonoBehaviour
    {
        [SerializeField] private GridSystem _gridSystem;
        
        Camera _mainCamera;
        Vector3 _eventDataPosition;
        
        private void Awake()
        {
            _mainCamera = Camera.main;
            
            if(_gridSystem == null)
                Debug.LogError("GridSystem reference is not assigned in CardPlacementHandler.");
        }

        public bool TryPlaceCard(CardUI card, Vector2 eventDataPosition)
        {
            var worldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(eventDataPosition.x, eventDataPosition.y, _mainCamera.nearClipPlane));
            _eventDataPosition = new Vector3(worldPosition.x, worldPosition.y, 0);
            _gridSystem.UnhighlightedCells();
            return _gridSystem.TryPlaceCardOnGrid(card, _eventDataPosition);
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_eventDataPosition, 0.2f);
        }

        public void StartPlacingCard(CardUI card)
        {
            _gridSystem.HighlightSuitableCells(card);
        }
    }
}
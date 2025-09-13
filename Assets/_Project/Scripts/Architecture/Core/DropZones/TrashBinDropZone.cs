using System;
using System.Threading.Tasks;
using _Project.Scripts.Architecture.Cards.Runtime;
using _Project.Scripts.Architecture.Core.Dependency_Injection;
using _Project.Scripts.Architecture.Core.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Architecture.Core.DropZones
{
    public class TrashBinDropZone : MonoBehaviour, IDropZone
    {
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }
        
        private void Start()
        {
            ServiceLocator.Get<DropZoneManager>().RegisterDropZone(this);
        }
        
        public bool CanAcceptCard(CardUI card)
        {
            return true;
        }

        public Task<bool> TryDropCard(CardUI card, Vector2 screenPosition)
        {
            return Task.FromResult(true);
        }

        public void StartDragPreview(CardUI card)
        {
           // highlight trash
        }

        public void EndDragPreview()
        {
            // unhighlight trash
        }

        public bool IsPositionInZone(Vector2 screenPosition)
        {
            var worldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, _mainCamera.nearClipPlane));
            var position = new Vector3(worldPosition.x, worldPosition.y, 0);
            if(Vector2.Distance(position, transform.position) > 3)
            {
                return false;
            }

            return true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 3);
        }
    }
}
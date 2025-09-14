using System.Threading.Tasks;
using _Project.Scripts.Architecture.Cards.Runtime;
using _Project.Scripts.Architecture.Core.Dependency_Injection;
using _Project.Scripts.Architecture.Core.DropZones;
using UnityEngine;

namespace _Project.Scripts.Architecture.Hand
{
    public class CardPlacementHandler: MonoBehaviour
    {
        private DropZoneManager _dropZoneManager;

        private void Start()
        {
            _dropZoneManager ??= ServiceLocator.Get<DropZoneManager>();
            
        }

        public async Task<bool> TryPlaceCard(CardUI card, Vector2 eventDataPosition)
        {
            _dropZoneManager.EndDragPreview();
            return await _dropZoneManager.TryDropCard(card, eventDataPosition);
        }

        public void StartPlacingCard(CardUI card)
        {
            _dropZoneManager.StartDragPreview(card);
        }
    }
}
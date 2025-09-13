using System.Threading.Tasks;
using _Project.Scripts.Architecture.Cards.Runtime;
using UnityEngine;

namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface IDropZone
    {
        bool CanAcceptCard(CardUI card);
        Task<bool> TryDropCard(CardUI card, Vector2 screenPosition);
        void StartDragPreview(CardUI card);
        void EndDragPreview();
        bool IsPositionInZone(Vector2 screenPosition);
    }
}
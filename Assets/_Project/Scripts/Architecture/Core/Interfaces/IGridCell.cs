using _Project.Scripts.Architecture.Entities.Base;
using _Project.Scripts.Architecture.Enums;
using UnityEngine;

namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface IGridCell
    {
        Vector2Int Position { get; }
        GameObject GameObject { get; }
        Entity OccupiedEntity { get; }
        bool IsOccupied { get; }
        
        void HighlightCell(HighlightType type);
        void Initialize(float cellSize, Vector2Int position);
        void SetEntity(Entity entity);
        void ClearEntity();
    }
}
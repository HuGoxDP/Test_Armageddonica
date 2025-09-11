using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Grid.Core;
using UnityEngine;

namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface IGridContext
    {
        Vector2Int GridSize { get; }
        Vector2 GridOffset { get; }
        float CellSize { get; }
        Transform GridTransform { get; }
        IGridCell[,] Cells { get; }
        GridCell CellPrefab { get; }
        
        bool CanPlaceAt(BaseCardData cardData, IGridCell cell);
        bool TryGetCell(Vector3 position, out IGridCell cell);
    }
}
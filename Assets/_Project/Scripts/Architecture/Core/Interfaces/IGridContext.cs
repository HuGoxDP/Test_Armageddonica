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
        public Transform GridTransform { get; }
        public IGridCell[,] Cells { get; }
        GridCell CellPrefab { get; }

        public bool CanPlaceAt(BaseCardData cardData, IGridCell cell);
        public bool TryGetCell(Vector3 position, out IGridCell cell);
    }
}
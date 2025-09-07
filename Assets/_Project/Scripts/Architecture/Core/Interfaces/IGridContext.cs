using System;
using System.Collections.Generic;
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
        
        bool TryGetGridComponent<TComponent>(out TComponent component) where TComponent : IGridComponent;
    }
}
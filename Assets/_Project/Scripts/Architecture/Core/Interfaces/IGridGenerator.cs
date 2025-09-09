using System;
using _Project.Scripts.Architecture.Grid.Core;
using UnityEngine;

namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface IGridGenerator : IGridComponent
    {
        event EventHandler<IGridCell[,]> OnGridGenerated;
        void GenerateGrid();
        Vector3 GetWorldPosition(int x, int y);
        Vector3 GetWorldPosition(IGridContext context, int x, int y);
        (int x, int y) GetCellCoordinates(Vector3 worldPosition);
        (int x, int y) GetCellCoordinates(IGridContext context, Vector3 worldPosition);
        
        IGridCell GetCellAt(Vector3 worldPosition);
        IGridCell GetCellAt(int x, int y);
    }
}
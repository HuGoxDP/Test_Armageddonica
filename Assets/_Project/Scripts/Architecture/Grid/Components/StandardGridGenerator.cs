using System;
using _Project.Scripts.Architecture.Core.Interfaces;
using _Project.Scripts.Architecture.Grid.Core;
using UnityEngine;

namespace _Project.Scripts.Architecture.Grid.Components
{
    public class StandardGridGenerator : MonoBehaviour, IGridGenerator
    {
        public event EventHandler<IGridCell[,]> OnGridGenerated;

        public bool IsEnabled { get; set; }
        private IGridContext _context;
        
        public void Initialize(IGridContext gridContext)
        {
            _context = gridContext;
            IsEnabled = true;
        }

        public void GenerateGrid()
        {
            if(!IsEnabled) return;
            
            var grid = new IGridCell[_context.GridSize.x, _context.GridSize.y];
            
            for (var x = 0; x < _context.GridSize.x; x++)
            {
                for (var y = 0; y < _context.GridSize.y; y++)
                {
                   var cellPosition = GetWorldPosition(x, y);
                   var newCell = Instantiate(_context.CellPrefab, cellPosition, Quaternion.identity, _context.GridTransform);
                   
                   newCell.Initialize(_context.CellSize, new Vector2Int(x,y));
                   newCell.name = $"Cell_{x}_{y}";
                   grid[x, y] = newCell;
                }
            }
            
            OnGridGenerated?.Invoke(this, grid);
        }
        
        public (int x, int y) GetCellCoordinates(Vector3 worldPosition)
        {
            var gridWorldOrigin = _context.GridTransform.position + new Vector3(_context.GridOffset.x, _context.GridOffset.y, 0);
            var localPos = worldPosition - gridWorldOrigin;
            
            int x = Mathf.FloorToInt(localPos.x / _context.CellSize);
            int y = Mathf.FloorToInt(localPos.y / _context.CellSize);
            return (x, y);
        }
        
        public (int x, int y) GetCellCoordinates(IGridContext context, Vector3 worldPosition)
        {
            var gridWorldOrigin = context.GridTransform.position + new Vector3(context.GridOffset.x, context.GridOffset.y, 0);
            var localPos = worldPosition - gridWorldOrigin;
            
            int x = Mathf.FloorToInt(localPos.x / context.CellSize);
            int y = Mathf.FloorToInt(localPos.y / context.CellSize);
            return (x, y);
        }

        public IGridCell GetCellAt(Vector3 worldPosition)
        {
            if(!IsEnabled) return null;
            
            var (x, y) = GetCellCoordinates(worldPosition);
            if (x < 0 || x >= _context.GridSize.x || y < 0 || y >= _context.GridSize.y)
                return null;

            return _context.Cells[x, y];
        }

        public IGridCell GetCellAt(int x, int y)
        {
            if (x < 0 || x >= _context.GridSize.x || y < 0 || y >= _context.GridSize.y)
                return null;
            
            return _context.Cells[x, y];
        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            if(!IsEnabled) return Vector3.zero;
            
            var gridWorldOrigin = _context.GridTransform.position + new Vector3(_context.GridOffset.x, _context.GridOffset.y, 0);
            return gridWorldOrigin + new Vector3(x * _context.CellSize + _context.CellSize / 2, y * _context.CellSize + _context.CellSize / 2, 0);
        }
        
        public Vector3 GetWorldPosition(IGridContext context,int x, int y)
        {
            if(!IsEnabled) return Vector3.zero;
            
            var gridWorldOrigin = context.GridTransform.position + new Vector3(context.GridOffset.x, context.GridOffset.y, 0);
            return gridWorldOrigin + new Vector3(x * context.CellSize + context.CellSize / 2, y * context.CellSize + context.CellSize / 2, 0);
        }
    }
}
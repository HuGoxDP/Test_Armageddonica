using System.Collections.Generic;
using _Project.Scripts.Architecture.Cards.Runtime;
using _Project.Scripts.Architecture.Core.Interfaces;
using _Project.Scripts.Architecture.Enums;
using UnityEngine;

namespace _Project.Scripts.Architecture.Grid.Components
{
    public class HighlightSystem : MonoBehaviour, IHighlightSystem
    {
        public bool IsEnabled { get; set; }

        private IGridContext _context;
        private readonly HashSet<IGridCell> _suitableCells  = new();
        private IGridCell _lastHoveredCell;

        public void Initialize(IGridContext gridContext)
        {
            _context = gridContext;
            IsEnabled = true;
        }

        public void HighlightSuitableCells(CardUI card)
        {
            if (!IsEnabled) return;
            
            _suitableCells.Clear();
            var cells = _context.Cells;
            var width = _context.GridSize.x;
            var height = _context.GridSize.y;
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var cell = cells[x, y];
                    if (_context.CanPlaceAt(card.CardData, cell))
                    {
                        HighlightCell(cell, HighlightType.Valid);
                        _suitableCells.Add(cell);
                    }
                    else
                    {
                        HighlightCell(cell, HighlightType.None);
                    }
                }
            }
        }
        
        public void UnhighlightedCells()
        {
            if (!IsEnabled) return;
            
            _suitableCells.Clear();
            var cells = _context.Cells;
            var width = _context.GridSize.x;
            var height = _context.GridSize.y;
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var cell = cells[x, y];
                    ClearHighlight(cell);
                }
            }
            
            _lastHoveredCell = null;
        }
        
        public void HighlightCell(IGridCell cell, HighlightType highlightType)
        {
            if (!IsEnabled) return;
            
            cell.HighlightCell(highlightType);
        }

        public void ClearHighlight(IGridCell cell)
        {
            if (!IsEnabled) return;
            
            cell.HighlightCell(HighlightType.None);
        }

        public void HighlightHoverCell(IGridCell cell)
        {
            if (!IsEnabled || cell == null) return;
            if (!IsCellSuitable(cell)) return;

            _lastHoveredCell = cell;
            HighlightCell(cell, HighlightType.Hovered);
        }
        
        public void ClearHoverHighlight()
        {
            if (!IsEnabled || _lastHoveredCell == null) return;
            if (!IsCellSuitable(_lastHoveredCell)) return;
            
            HighlightCell(_lastHoveredCell, HighlightType.Valid);
            _lastHoveredCell = null;
        }
        
        public bool IsCellSuitable(IGridCell cell)
        {
            return _suitableCells.Contains(cell);
        }
    }
}
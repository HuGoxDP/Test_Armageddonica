using System;
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
        private IGridInputSystem _gridInputSystem;
        private readonly HashSet<IGridCell> _suitableCells  = new();
        private IGridCell _lastHoveredCell;

        private void OnDestroy()
        {
            if (_gridInputSystem != null)
            {
                _gridInputSystem.OnCellHoverEnter -= OnCellHoverEnter;
                _gridInputSystem.OnCellHoverExit -= OnCellHoverExit;
            }
        }

        public void Initialize(IGridContext gridContext)
        {
            _context = gridContext;
            IsEnabled = true;
        }

        public void RegisterGridInputSystem(IGridInputSystem gridInputSystem)
        {
            _gridInputSystem = gridInputSystem;
            if (_gridInputSystem != null)
            {
                _gridInputSystem.OnCellHoverEnter += OnCellHoverEnter;
                _gridInputSystem.OnCellHoverExit += OnCellHoverExit;
            }
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
                        cell.HighlightCell(HighlightType.Valid);
                        _suitableCells.Add(cell);
                    }
                    else
                    {
                        cell.HighlightCell(HighlightType.None);
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
                    cell.HighlightCell(HighlightType.None);
                }
            }
            
            _lastHoveredCell = null;
        }
        
        public void HighlightHoverCell(IGridCell cell)
        {
            if (!IsEnabled || cell == null) return;
            if (!_suitableCells.Contains(cell)) return;

            _lastHoveredCell = cell;
            cell.HighlightCell(HighlightType.Hovered);
        }
        
        public void ClearHoverHighlight()
        {
            if (!IsEnabled || _lastHoveredCell == null) return;
            if (!_suitableCells.Contains(_lastHoveredCell)) return;
            
            _lastHoveredCell.HighlightCell(HighlightType.Valid);
            _lastHoveredCell = null;
        }

        private void OnCellHoverEnter(IGridCell cell)
        {   if (cell == null) return;
            HighlightHoverCell(cell);
        }
        
        private void OnCellHoverExit(IGridCell cell)
        {
            ClearHoverHighlight();
        }
    }
}
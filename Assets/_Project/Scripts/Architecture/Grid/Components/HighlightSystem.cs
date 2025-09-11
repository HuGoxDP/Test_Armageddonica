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
        public void Initialize(IGridContext gridContext)
        {
            _context = gridContext;
            IsEnabled = true;
        }

        public void HighlightSuitableCells(CardUI card)
        {
            if (!IsEnabled) return;
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
    }
}
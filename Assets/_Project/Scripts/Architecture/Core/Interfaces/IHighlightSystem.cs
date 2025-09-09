using _Project.Scripts.Architecture.Cards.Runtime;
using _Project.Scripts.Architecture.Enums;
using _Project.Scripts.Architecture.Grid.Core;

namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface IHighlightSystem : IGridComponent
    {
        void UnhighlightedCells(); 
        void HighlightSuitableCells(CardUI card);
        void HighlightCell(IGridCell cell, HighlightType highlightType);
        void ClearHighlight(IGridCell cell);
    }
}
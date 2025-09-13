using _Project.Scripts.Architecture.Cards.Runtime;
using _Project.Scripts.Architecture.Enums;

namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface IHighlightSystem : IGridComponent
    {
        void UnhighlightedCells(); 
        void HighlightSuitableCells(CardUI card);
        void HighlightCell(IGridCell cell, HighlightType highlightType);
        void ClearHighlight(IGridCell cell);
        void HighlightHoverCell(IGridCell cell);
        void ClearHoverHighlight();
        bool IsCellSuitable(IGridCell cell);
    }
}
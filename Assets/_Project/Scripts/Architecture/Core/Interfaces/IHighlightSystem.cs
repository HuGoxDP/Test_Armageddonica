using _Project.Scripts.Architecture.Cards.Runtime;
using _Project.Scripts.Architecture.Enums;

namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface IHighlightSystem : IGridComponent
    {
        void RegisterGridInputSystem(IGridInputSystem gridInputSystem);
        void UnhighlightedCells(); 
        void HighlightSuitableCells(CardUI card);
    }
}
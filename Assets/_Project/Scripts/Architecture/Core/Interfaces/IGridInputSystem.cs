using System;

namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface IGridInputSystem : IGridComponent
    {
        event Action<IGridCell> OnCellHoverEnter;
        event Action<IGridCell> OnCellHoverExit;
    }
}
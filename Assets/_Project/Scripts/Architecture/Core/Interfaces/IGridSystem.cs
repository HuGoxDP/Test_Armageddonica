using _Project.Scripts.Architecture.Cards.Runtime;
using _Project.Scripts.Architecture.Core.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Architecture.Grid.Core
{
    public interface IGridSystem
    {
        void HighlightSuitableCells(CardUI card);
        bool TryPlaceCardOnGrid(CardUI card, Vector2 worldPosition);
        void UnhighlightedCells();
        
    }
}
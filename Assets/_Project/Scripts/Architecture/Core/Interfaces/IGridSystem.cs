using System.Collections.Generic;
using _Project.Scripts.Architecture.Cards.Runtime;
using _Project.Scripts.Architecture.Entities.Base;
using UnityEngine;

namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface IGridSystem
    {
        List<Entity> GetEntitiesInRange(Vector2Int position, int range);
        void HighlightSuitableCells(CardUI card);
        bool TryPlaceCardOnGrid(CardUI card, Vector2 worldPosition);
        void UnhighlightedCells();
        
    }
}
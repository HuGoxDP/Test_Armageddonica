using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Cards.Runtime;
using _Project.Scripts.Architecture.Entities.Base;
using _Project.Scripts.Architecture.Grid;
using UnityEngine;

namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface IGridSystem
    {
        event EventHandler<EntityPlacementEventArgs> OnEntityPlaced;
        event EventHandler<EntityPlacementEventArgs> OnEntityRemoved;
        Transform GridTransform { get; }
        IGridCell[,] Cells { get; }
        void HighlightSuitableCells(CardUI card);
        void UnhighlightedCells();
        Task<bool> TryPlaceCardOnGrid(CardUI card, Vector2 worldPosition);
        bool CanPlaceAt(BaseCardData cardData, IGridCell cell);
        bool TryGetCell(Vector3 position, out IGridCell cell);
        List<Entity> GetEntitiesInRange(Vector2Int position, int range);
        List<Entity> GetEntities();
        void EnableTooltips(bool b);
    }
}
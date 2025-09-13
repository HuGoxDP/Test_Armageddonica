using System;
using System.Threading.Tasks;
using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Grid;
using _Project.Scripts.Architecture.Grid.Core;
using _Project.Scripts.Architecture.Grid.Validators;
using UnityEngine;

namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface IPlacementSystem : IGridComponent
    {
        public event EventHandler<EntityPlacementEventArgs> OnEntityPlaced;
        public event EventHandler<EntityPlacementEventArgs> OnEntityRemoved;
        void RegisterValidator(BasePlacementValidator validator);
        bool CanPlaceAt(BaseCardData cardData, Vector3 position);
        bool CanPlaceAt(BaseCardData cardData, IGridCell cell);
        Task<bool> TryPlaceCard(BaseCardData cardData, Vector3 position);
        void RemoveEntity(Vector3 position);
    }
}
using System;
using _Project.Scripts.Architecture.Entities.Base;

namespace _Project.Scripts.Architecture.Grid.Core
{
    public class EntityPlacementEventArgs : EventArgs
    {
        public Entity Entity { get; }
        public bool IsPlaced { get; }
        
        public EntityPlacementEventArgs(Entity entity, bool isPlaced)
        {
            Entity = entity;
            IsPlaced = isPlaced;
        }
    }
}
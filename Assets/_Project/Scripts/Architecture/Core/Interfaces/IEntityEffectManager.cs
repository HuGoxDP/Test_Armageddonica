using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Scripts.Architecture.Entities.Base;
using _Project.Scripts.Architecture.Enums;
using _Project.Scripts.Architecture.Grid;
using _Project.Scripts.Architecture.Grid.Core;
using UnityEngine;

namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface IEntityEffectManager
    {
        List<Entity> GetEntitiesInRange(Vector2Int position, int range, StatType statType);
        Task CalculateAllEffects();
        void UpdateStatCache(object sender, EntityPlacementEventArgs e);
        void UpdateStatCache(Entity entity, bool add);
    }
}
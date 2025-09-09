using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Scripts.Architecture.Core.GameStates;
using _Project.Scripts.Architecture.Enums;
using _Project.Scripts.Architecture.Grid.Core;
using UnityEngine;

namespace _Project.Scripts.Architecture.Entities.Base
{
    public class EntityEffectManager : GameControllable
    {
        [SerializeField] private GridSystem _gridSystem;

        public List<Entity> GetEntitiesInRange(Vector2Int position, int range, StatType statType)
        {
            var entitiesInRange = _gridSystem.GetEntitiesInRange(position, range);
            if (entitiesInRange.Count == 0) return null;
            
            var entitiesInRangeWithStat = new List<Entity>(entitiesInRange.Count);
            var entitiesCount = entitiesInRange.Count;
            
            for (var i = 0; i < entitiesCount; i++)
            {
                var entity = entitiesInRange[i];
                if (entity.HasStat(statType) != null)
                {
                    entitiesInRangeWithStat.Add(entity);
                }
            }
            
            return entitiesInRangeWithStat.Count > 0 ? entitiesInRangeWithStat : null;
        }
        
        public async Task CalculateAllEffects()
        {
            var entities = _gridSystem.GetEntities();
            if (entities == null || entities.Count == 0) return;
            
            var entitiesCount = entities.Count;
            var tasks = new List<Task>(entitiesCount);
            
            for (var i = 0; i < entitiesCount; i++)
            {
                var entity = entities[i];
                var effectApplicators = entity.GetAllEffectApplicators();
                var effectApplicatorsCount = effectApplicators.Count;

                for (var j = 0; j < effectApplicatorsCount; j++)
                {
                    var effectApplicator = effectApplicators[j];
                    tasks.Add(effectApplicator.ApplyEffect(this));
                }
            }

            if (tasks.Count >= 0)
            {
                await Task.WhenAll(tasks);
            }
        }
        
        protected override void OnGameStateChanged(object sender, GameState newState)
        {
            gameObject.SetActive(newState == GameState.BuffTurn);
        }
    }
}
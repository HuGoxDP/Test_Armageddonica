using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Project.Scripts.Architecture.Core.Dependency_Injection;
using _Project.Scripts.Architecture.Core.GameStates;
using _Project.Scripts.Architecture.Core.Interfaces;
using _Project.Scripts.Architecture.Entities.Base;
using _Project.Scripts.Architecture.Enums;
using _Project.Scripts.Architecture.Grid;
using _Project.Scripts.Architecture.Grid.Core;
using UnityEngine;

namespace _Project.Scripts.Architecture.EffectApplicators
{
    public class EntityEffectManager : GameControllable, IEntityEffectManager
    {
        private IGridSystem _gridSystem;

        private readonly Dictionary<StatType, HashSet<Entity>> _entitiesByStatType = new();

        private void Awake()
        {
            ServiceLocator.Register<IEntityEffectManager>(this);
        }

        private void Start()
        {
            _gridSystem = ServiceLocator.Get<IGridSystem>();
            if (_gridSystem == null) return;
            _gridSystem.OnEntityPlaced += UpdateStatCache;
            _gridSystem.OnEntityRemoved += UpdateStatCache;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_gridSystem == null) return;
            _gridSystem.OnEntityPlaced -= UpdateStatCache;
            _gridSystem.OnEntityRemoved -= UpdateStatCache;
        }

        public List<Entity> GetEntitiesInRange(Vector2Int position, int range, StatType statType)
        {
            var entitiesInRange = _gridSystem.GetEntitiesInRange(position, range);
            if (entitiesInRange == null || entitiesInRange.Count == 0) 
                return null;

            if (!_entitiesByStatType.TryGetValue(statType, out var entitiesWithStat) || entitiesWithStat.Count == 0)
                return null;

            var result = new List<Entity>(entitiesInRange.Count);
            result.AddRange(entitiesInRange.Where(entity => entitiesWithStat.Contains(entity)));

            return result.Count > 0 ? result : null;
        }


        public async Task CalculateAllEffects()
        {
            var entities = _gridSystem.GetEntities();
            if (entities == null || entities.Count == 0) return;

            var tasks = new List<Task>(entities.Count * 2);
            
            foreach (var entity in entities)
            {
                var effectApplicators = entity.GetAllEffectApplicators();
                foreach (var effectApplicator in effectApplicators)
                {
                    tasks.Add(effectApplicator.ApplyEffect(entity, this));
                }
            }
            
            if (tasks.Count > 0)
            {
                await Task.WhenAll(tasks);
            }
        }

        public void UpdateStatCache(object sender, EntityPlacementEventArgs e)
        {
            UpdateStatCache(e.Entity, e.IsPlaced);
        }

        public void UpdateStatCache(Entity entity, bool add)
        {
            foreach (StatType statType in Enum.GetValues(typeof(StatType)))
            {
                if (entity.HasStat(statType) != null)
                {
                    if (!_entitiesByStatType.ContainsKey(statType))
                        _entitiesByStatType[statType] = new HashSet<Entity>();

                    if (add)
                        _entitiesByStatType[statType].Add(entity);
                    else
                        _entitiesByStatType[statType].Remove(entity);
                }
            }
        }

        protected override void OnGameStateChanged(object sender, GameState newState)
        {
            gameObject.SetActive(newState == GameState.BuffTurn);
        }
    }
}
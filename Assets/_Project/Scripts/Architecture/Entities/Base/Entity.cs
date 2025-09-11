using System;
using System.Collections.Generic;
using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Core.Interfaces;
using _Project.Scripts.Architecture.Enums;
using NaughtyAttributes;
using UnityEngine;

namespace _Project.Scripts.Architecture.Entities.Base
{
    public sealed class Entity : MonoBehaviour, IGridPlaceable
    {
        [field: SerializeField, ReadOnly, TextArea(3, 9), Space(20)] public string StatsLabel { get; private set; }
        
        public Vector2Int Position { get; private set; }
        public EntityCardData CardData { get; private set; }
        
        private StatsContainer Stats { get; set; }
        private Dictionary<Type, IComponent> Components { get; set; }
        private Dictionary<Type, IEffectApplicator> EffectApplicators { get; set; }

        private List<IEffectApplicator> _cachedEffectApplicators;


        public void Initialize(EntityCardData cardData)
        {
            CardData = cardData;
            Stats = cardData.Stats;
            InitializeComponents();
            UpdateStats();
        }

        public void InitializePosition(Vector2Int position)
        {
            Position = position;
        }

        public Entity HasStat(StatType statType)
        {
            return Stats.HasStat(statType) ? this : null;
        }

        private void UpdateStats()
        {
            StatsLabel = Stats.GetVisibleStats();
        }

        public float GetStat(StatType statType, StatValueSource statValueSource = StatValueSource.Current)
        {
            return Stats.GetStat(statType, statValueSource);
        }

        public void ApplyStatModifier(StatType statType, CalculationMethod calculationMethod,  float value)
        {
            Stats.ApplyStatModifier(statType, calculationMethod, value);
            UpdateStats();
        }

        public TComponent GetEntityComponent<TComponent>() where TComponent : IComponent
        {
            if (Components == null) return default;

            Type componentType = typeof(TComponent);
            if (Components.TryGetValue(componentType, out var component))
            {
                return (TComponent)component;
            }

            return default;
        }

        public TEffectApplicator GetEffectApplicator<TEffectApplicator>() where TEffectApplicator : IEffectApplicator
        {
            if (EffectApplicators == null) return default;

            Type applicatorType = typeof(TEffectApplicator);
            if (EffectApplicators.TryGetValue(applicatorType, out var applicator))
            {
                return (TEffectApplicator)applicator;
            }

            return default;
        }

        public List<IEffectApplicator> GetAllEffectApplicators()
        {
            _cachedEffectApplicators ??= new List<IEffectApplicator>(EffectApplicators.Values);

            return _cachedEffectApplicators;
        }

        private void InitializeComponents()
        {
            var allComponents = GetComponents<MonoBehaviour>();
            Components = new Dictionary<Type, IComponent>();
            EffectApplicators = new Dictionary<Type, IEffectApplicator>();

            foreach (var component in allComponents)
            {
                if (component is IComponent iComponent)
                {
                    iComponent.Initialize(this);
                    Components.Add(component.GetType(), iComponent);
                }
        
                if (component is IEffectApplicator effectApplicator)
                {
                    EffectApplicators.Add(component.GetType(), effectApplicator);
                }
            }
        }
    }
}
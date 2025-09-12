using System;
using System.Threading.Tasks;
using _Project.Scripts.Architecture.Core.Interfaces;
using _Project.Scripts.Architecture.EffectApplicators;
using _Project.Scripts.Architecture.Entities.Base;
using _Project.Scripts.Architecture.Enums;
using UnityEngine;

namespace _Project.Scripts.Architecture.Entities.Components
{
    public class StatScalingEffectApplicator : BasicEffectApplicator, IComponent
    {
        [field: SerializeField] public StatScalingEffectData EffectData { get; private set; }
        public bool IsEnabled { get; set; }
        
        private float _lastScaledValue = 0;

        public override StatType[] GetRequiredStats()
        {
            return new[] { EffectData.ScalableStatType, EffectData.MultiplierStatType };
        }
        
        public void Initialize(Entity entity)
        {
            IsEnabled = true;    
        }

        public override Task ApplyEffect(Entity effectApplicator, IEntityEffectManager effectManager)
        {

            if (!IsEnabled)
                return Task.CompletedTask;
            try
            {
                if (EffectData == null)
                {
                    Debug.LogError("EffectData is null in StatScalingEffectApplicator");
                    return Task.CompletedTask;
                }

                if (effectApplicator == null)
                {
                    Debug.LogError("effectApplicator is null in StatScalingEffectApplicator");
                    return Task.CompletedTask;
                }
                
                var baseValue = effectApplicator.GetStat(EffectData.ScalableStatType, EffectData.ScalableBaseValueSource);
                var multiplierValue = effectApplicator.GetStat(EffectData.MultiplierStatType, EffectData.MultiplierValueSource);
                var scaledValue = baseValue * multiplierValue;

                if (Mathf.Approximately(scaledValue, _lastScaledValue))
                    return Task.CompletedTask;

                var diff = scaledValue - _lastScaledValue;

                effectApplicator.ApplyStatModifier(EffectData.ScalableStatType, CalculationMethod.Flat, diff);

                _lastScaledValue = scaledValue;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error in ApplyEffect: {e.Message}");
            }

            return Task.CompletedTask;
        }

    }
}
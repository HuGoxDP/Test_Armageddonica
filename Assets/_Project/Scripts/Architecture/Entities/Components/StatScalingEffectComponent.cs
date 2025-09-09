using System;
using System.Threading.Tasks;
using _Project.Scripts.Architecture.Entities.Base;
using _Project.Scripts.Architecture.Enums;
using UnityEngine;

namespace _Project.Scripts.Architecture.Entities.Components
{
    public class StatScalingEffectComponent : BaseEffectApplicatorComponent
    {
        [field: SerializeField] public StatScalingEffectData EffectData { get; private set; }

        private float _lastScaledValue = 0;


        public override Task ApplyEffect(EntityEffectManager effectManager)
        {
            if (!IsEnabled)
                return Task.CompletedTask;

            try
            {
                var baseValue = entity.GetStat(EffectData.ScalableStatType, EffectData.ScalableBaseValueSource);
                var multiplierValue = entity.GetStat(EffectData.MultiplierStatType, EffectData.MultiplierValueSource);
                var scaledValue = baseValue * multiplierValue;

                if (Mathf.Approximately(scaledValue, _lastScaledValue))
                    return Task.CompletedTask;

                var diff = scaledValue - _lastScaledValue;

                entity.ApplyStatModifier(
                    EffectData.ScalableStatType,
                    CalculationMethod.Flat,
                    EffectData.ScalableValueSource,
                    diff
                );

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
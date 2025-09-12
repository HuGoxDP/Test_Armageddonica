using System;
using System.Threading.Tasks;
using _Project.Scripts.Architecture.Core.Interfaces;
using _Project.Scripts.Architecture.Entities.Base;
using _Project.Scripts.Architecture.Enums;
using UnityEngine;

namespace _Project.Scripts.Architecture.EffectApplicators
{
    public class BuffStatEffectApplicator : BasicEffectApplicator
    {
        [field: SerializeField] public RadiusBasedEffectData EffectData { get; private set; }
        
        public override StatType[] GetRequiredStats()
        {
            return new [] { EffectData.StatType };
        }

        public override Task ApplyEffect(Entity effectApplicator, IEntityEffectManager effectManager)
        {
            try
            {
                if (EffectData == null)
                {
                    Debug.LogError("EffectData is null in BuffStatEffectApplicator");
                    return Task.CompletedTask;
                }

                if (effectApplicator == null)
                {
                    Debug.LogError("effectApplicator is null in BuffStatEffectApplicator");
                    return Task.CompletedTask;
                }

                if (effectManager == null)
                {
                    Debug.LogError("effectManager is null in BuffStatEffectApplicator");
                    return Task.CompletedTask;
                }
                
                if (EffectData.Radius == 0)
                {
                    effectApplicator.ApplyStatModifier(EffectData.StatType, EffectData.CalculationMethod, EffectData.Value);
                }
                else
                {
                    var entities = effectManager.GetEntitiesInRange(effectApplicator.Position, EffectData.Radius, EffectData.StatType);
                    
                    if (entities != null && entities.Count > 0)
                    {
                        foreach (var targetEntity in entities)
                        {
                            targetEntity.ApplyStatModifier(EffectData.StatType, EffectData.CalculationMethod, EffectData.Value);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error in ApplyEffect: {e.Message}");
            }
            
            
            return Task.CompletedTask;
        }

    }
}
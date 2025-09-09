using System;
using System.Threading.Tasks;
using _Project.Scripts.Architecture.Entities.Base;
using UnityEngine;

namespace _Project.Scripts.Architecture.Entities.Components
{
    public class BuffStatComponent : BaseEffectApplicatorComponent
    {
        [field: SerializeField] public RadiusBasedEffectData EffectData { get; private set; }
        
        
        public override Task ApplyEffect(EntityEffectManager effectManager)
        {
            if (!IsEnabled)
                return Task.CompletedTask;
            try
            {
                if (EffectData.Radius == 0)
                {
                    entity.ApplyStatModifier(EffectData.StatType, EffectData.CalculationMethod, EffectData.StatValueSource, EffectData.Value);
                }
                else
                {
                    var entities = effectManager.GetEntitiesInRange(entity.Position, EffectData.Radius, EffectData.StatType);
                    
                    if (entities != null && entities.Count > 0)
                    {
                        foreach (var targetEntity in entities)
                        {
                            Debug.Log($"Applying effect to {targetEntity.gameObject.name}");
                            targetEntity.ApplyStatModifier(EffectData.StatType, EffectData.CalculationMethod, EffectData.StatValueSource, EffectData.Value);
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
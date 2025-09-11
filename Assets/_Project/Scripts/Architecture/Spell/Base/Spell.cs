using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Scripts.Architecture.Core.Dependency_Injection;
using _Project.Scripts.Architecture.Core.Interfaces;
using _Project.Scripts.Architecture.EffectApplicators;
using _Project.Scripts.Architecture.Entities.Base;
using UnityEngine;

namespace _Project.Scripts.Architecture.Spell.Base
{
    public class Spell : MonoBehaviour, IGridPlaceable
    {
        public async Task<bool> TryUseEffects(Entity entity)
        {
            if (entity == null) 
                return false;
    
            try
            {
                return await ApplyEffects(entity);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error applying spell effects: {ex.Message}");
                return false;
            }
        }
        
        private async Task<bool> ApplyEffects(Entity entity)
        {
            var effectApplicators = GetComponents<IEffectApplicator>();
            int applicatorCount = effectApplicators.Length;
            
            if (applicatorCount == 0)
                return false;
            
            var effectManager = ServiceLocator.Get<IEntityEffectManager>();
            var tasks = new List<Task>(applicatorCount);

            for (int i = 0; i < applicatorCount; i++)
            {
                tasks.Add(effectApplicators[i].ApplyEffect(entity, effectManager));
            }
            
            await Task.WhenAll(tasks);
            return true;
        }
    }
}
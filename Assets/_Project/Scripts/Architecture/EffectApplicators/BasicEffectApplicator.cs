using System.Threading.Tasks;
using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Core.Interfaces;
using _Project.Scripts.Architecture.Entities.Base;
using _Project.Scripts.Architecture.Enums;
using UnityEngine;

namespace _Project.Scripts.Architecture.EffectApplicators
{
    public abstract class BasicEffectApplicator : MonoBehaviour, IEffectApplicator, IStatRequirements
    { 
        
        public abstract Task ApplyEffect(Entity effectApplicator, IEntityEffectManager effectManager);
        public abstract StatType[] GetRequiredStats();
    }
}
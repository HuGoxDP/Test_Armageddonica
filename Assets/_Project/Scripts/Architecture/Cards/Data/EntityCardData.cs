using System.Collections.Generic;
using _Project.Scripts.Architecture.Core.Interfaces;
using _Project.Scripts.Architecture.Entities.Base;
using _Project.Scripts.Architecture.Enums;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Architecture.Cards.Data
{
    public abstract class EntityCardData : BaseCardData
    {
        [field: SerializeField]  public Entity EntityPrefab { get; protected set;  }
        [field: SerializeField] public StatsContainer Stats { get; protected set; }
        
        [field: SerializeField]  private StatType[] _requiredStats;
        
        public StatType[] RequiredStats => _requiredStats;
        
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateRequiredStats();
        }

        [ContextMenu("Update Required Stats")]
        public void UpdateRequiredStats()
        {
            if (EntityPrefab == null) return;
            
            var requirements = EntityPrefab.GetComponents<IStatRequirements>();
            var statSet = new HashSet<StatType>();

            foreach (var requirement in requirements)
            {
                foreach (var statType in requirement.GetRequiredStats())
                {
                    statSet.Add(statType);
                }
            }
            
            _requiredStats = new StatType[statSet.Count];
            statSet.CopyTo(_requiredStats);
            
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
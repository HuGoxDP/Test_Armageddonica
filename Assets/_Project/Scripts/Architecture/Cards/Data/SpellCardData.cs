using System.Collections.Generic;
using _Project.Scripts.Architecture.Core.Interfaces;
using _Project.Scripts.Architecture.Enums;
using _Project.Scripts.Editor;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Architecture.Cards.Data
{
    [CreateAssetMenu(menuName = "Game/CardSystem/New Spell Card Data")]
    public class SpellCardData : BaseCardData
    {
        [field: SerializeField]  public Spell.Base.Spell SpellPrefab { get; protected set;  }
        [SerializeField] private TargetType _targetType;
        [field: SerializeField]  private StatType[] _requiredStats;
        
        public TargetType TargetType => _targetType;
        public StatType[] RequiredStats => _requiredStats;
        public override CardType CardType => CardType.Spell;
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateRequiredStats();
        }

        [ContextMenu("Update Required Stats")]
        public void UpdateRequiredStats()
        {
            if (SpellPrefab == null) return;
            
            var requirements = SpellPrefab.GetComponents<IStatRequirements>();
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
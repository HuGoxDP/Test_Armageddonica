using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _Project.Scripts.Architecture.Enums;
using UnityEngine;

namespace _Project.Scripts.Architecture.Entities.Base
{
    [Serializable]
    public class EntityStatsContainer
    {
        [SerializeField] private List<EntityStat> _stats;

        private Dictionary<StatType, EntityStat> _statsDictionary;

        /// <summary> Returns stat value. </summary>
        public float GetStat(StatType statType, StatValueSource statValueSource)
        {
            InitializeStatsDictionary();

            return statValueSource switch
            {
                StatValueSource.Current => _statsDictionary[statType].CurrentValue,
                StatValueSource.Base => _statsDictionary[statType].BaseValue,
                _ => throw new ArgumentOutOfRangeException(nameof(statValueSource), statValueSource, null)
            };
        }
        
        /// <summary> Sets stat value. </summary>
        public void SetStat(StatType statType, StatValueSource statValueSource, float value)
        {
            InitializeStatsDictionary();
            
            switch (statValueSource)
            {
                case StatValueSource.Current:
                    _statsDictionary[statType].CurrentValue = value;
                    return;
                case StatValueSource.Base:
                    _statsDictionary[statType].BaseValue = value;
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statValueSource), statValueSource, null);
            }
        }
        
        /// <summary> Returns true if stat exists. </summary>
        public bool HasStat(StatType statType)
        {
            InitializeStatsDictionary();
            return _statsDictionary.ContainsKey(statType);
        }

        /// <summary> Returns string with all visible stats. </summary>
        public string GetVisibleStats()
        {
            var sb = new StringBuilder();
            foreach (var stat in _stats.Where(stat => stat.Visible))
            {
                sb.Append($"{stat.StatType}: {GetStat(stat.StatType, StatValueSource.Current)}\n");
            }

            return sb.ToString();
        }
        
        /// <summary> Applies stat modifier. </summary>
        public void ApplyStatModifier(StatType statType, CalculationMethod calculationMethod, StatValueSource statValueSource, float value)
        {
            InitializeStatsDictionary();
            if (!_statsDictionary.TryGetValue(statType, out var stat))
            {
                Debug.LogError($"Stat {statType} not found in EntityStatsContainer");
                return;
            }
            
            var statValue = GetStat(statType, statValueSource);
            
            switch (calculationMethod)
            {
                case CalculationMethod.Flat:
                    statValue += value;
                    break;
                case CalculationMethod.Percent:
                    statValue *= (1 + value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(calculationMethod), calculationMethod, null);
            }
            
            SetStat(statType, statValueSource, statValue);
        }
        
        private void InitializeStatsDictionary()
        {
            if (_statsDictionary == null)
            {
                _statsDictionary = new Dictionary<StatType, EntityStat>();
                foreach (var stat in _stats)
                {
                    if (_statsDictionary.ContainsKey(stat.StatType))
                    {
                        Debug.LogError($"Duplicate stat {stat.StatType} in EntityStatsContainer");
                        continue;
                    }

                    stat.CurrentValue = stat.BaseValue;
                    _statsDictionary.Add(stat.StatType, stat);
                }
            }
        }
    }
}
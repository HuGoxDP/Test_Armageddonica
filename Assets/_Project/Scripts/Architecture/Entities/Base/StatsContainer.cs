using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _Project.Scripts.Architecture.Enums;
using UnityEngine;

namespace _Project.Scripts.Architecture.Entities.Base
{
    [Serializable]
    public class StatsContainer
    {
        public event Action<StatType, float> OnStatChanged;

        [SerializeField] private StatData[] _stats;

        private Dictionary<StatType, int> _statIndexMap;

        /// <summary> Returns stat value. </summary>
        public float GetStat(StatType type, StatValueSource source)
        {
            InitializeStatsDictionary();

            if (_statIndexMap.TryGetValue(type, out int index))
            {
                return source == StatValueSource.Base ? _stats[index].BaseValue : _stats[index].CurrentValue;
            }
            
            return 0f;
        }
        
        /// <summary> Sets stat value. </summary>
        public void SetStat(StatType type, float value)
        {
            if (_statIndexMap.TryGetValue(type, out int index) && !_stats[index].IsConst)
            {
                _stats[index].CurrentValue = value;
                OnStatChanged?.Invoke(type, value);
            }
        }
        
        /// <summary> Returns true if stat exists. </summary>
        public bool HasStat(StatType statType)
        {
            InitializeStatsDictionary();
            return _statIndexMap.ContainsKey(statType);
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
        public void ApplyStatModifier(StatType statType, CalculationMethod calculationMethod, float value)
        {
            InitializeStatsDictionary();
            
            if (!_statIndexMap.TryGetValue(statType, out var stat))
            {
                Debug.LogError($"Stat {statType} not found in EntityStatsContainer");
                return;
            }
            
            var statValue = GetStat(statType, StatValueSource.Current);
            
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
            
            SetStat(statType, statValue);
        }
        
        private void InitializeStatsDictionary()
        {
            if (_statIndexMap == null)
            {
                _statIndexMap = new Dictionary<StatType, int>();
                for (int i = 0; i < _stats.Length; i++)
                {
                    _statIndexMap[_stats[i].StatType] = i;
                    _stats[i].CurrentValue = _stats[i].BaseValue;
                }
            }
        }
    }
}
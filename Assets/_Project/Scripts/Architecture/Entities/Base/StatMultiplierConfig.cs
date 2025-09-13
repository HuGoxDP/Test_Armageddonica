using System.Collections.Generic;
using _Project.Scripts.Architecture.Enums;
using UnityEngine;

namespace _Project.Scripts.Architecture.Entities.Base
{
    [CreateAssetMenu(fileName = "StatMultiplierConfig", menuName = "Game/Configs/Stat Multiplier Config")]
    public class StatMultiplierConfig : ScriptableObject
    {
        [System.Serializable]
        public class StatMultiplier
        {
            public StatType statType;
            [Range(0, 1)]public float multiplier;
        }

        [SerializeField] private List<StatMultiplier> _multipliers = new List<StatMultiplier>();

        private Dictionary<StatType, float> _multiplierDictionary;

        public float GetMultiplier(StatType statType)
        {
            if (_multiplierDictionary == null)
                InitializeDictionary();

            return _multiplierDictionary != null && _multiplierDictionary.TryGetValue(statType, out float multiplier) ? multiplier : 1f;
        }

        private void InitializeDictionary()
        {
            _multiplierDictionary = new Dictionary<StatType, float>();
            foreach (var statMultiplier in _multipliers)
            {
                _multiplierDictionary[statMultiplier.statType] = statMultiplier.multiplier;
            }
        }

        public void AddMultiplier(StatType statType, float multiplier)
        {
            if (_multiplierDictionary == null)
                InitializeDictionary();

            if (_multiplierDictionary != null)
                _multiplierDictionary[statType] = multiplier;

            // Also update the serialized list
            var existing = _multipliers.Find(m => m.statType == statType);
            if (existing != null)
            {
                existing.multiplier = multiplier;
            }
            else
            {
                _multipliers.Add(new StatMultiplier { statType = statType, multiplier = multiplier });
            }
        }
    }
}
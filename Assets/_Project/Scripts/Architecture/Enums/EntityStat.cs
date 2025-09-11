using System;
using UnityEngine;

namespace _Project.Scripts.Architecture.Enums
{
    [Serializable]
    public class EntityStat
    {
        [field: SerializeField] public StatType StatType { get; private set; } = StatType.None;
        [field: SerializeField] public bool Visible { get; private set; } = true;
        [field: SerializeField] public bool IsDecimal { get; private set; } = true;
        [field: SerializeField] public bool IsConst { get; private set; } = false;
        
        
        [field: SerializeField] private float _baseValue;
        
        
        public float BaseValue
        {
            get => _baseValue;
            set
            {
                if (IsConst) return;
                if (!IsDecimal)
                {
                    _baseValue = (int) Math.Max(0, value);
                    return;
                }
                
                _baseValue = Math.Max(0, value);
            }
        }

        public float CurrentValue
        {
            get => _currentValue;
            set
            {
                if (IsConst) return;
                if (!IsDecimal)
                {
                    _currentValue = (int) Math.Max(0, value);
                    return;
                }
                
                _currentValue = Math.Max(0, value);
            }
        }

        private float _currentValue;
    }
}
using System;
using _Project.Scripts.Architecture.Enums;
using UnityEngine;

namespace _Project.Scripts.Architecture.Entities.Base
{
    [Serializable]
    public class EntityStat
    {
        [field: SerializeField] public StatType StatType { get; private set; }
        [field: SerializeField] public bool Visible { get; private set; }
        [field: SerializeField] public bool IsDecimal { get; private set; }
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
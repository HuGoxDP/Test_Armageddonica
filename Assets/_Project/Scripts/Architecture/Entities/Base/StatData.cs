using System;
using _Project.Scripts.Architecture.Enums;
using UnityEngine;

namespace _Project.Scripts.Architecture.Entities.Base
{
    [Serializable]
    public class StatData
    {
        public event Action<StatType, float> OnValueChanged;

        [field: SerializeField] public StatType StatType { get; private set; } = StatType.None;
        [field: SerializeField] public bool Visible { get; private set; } = true;
        [field: SerializeField] public bool IsDecimal { get; private set; } = true;
        [field: SerializeField] public bool IsConst { get; private set; } = false;

        [field: SerializeField] private float _baseValue;

        private float _currentValue;
        
        public float BaseValue => _baseValue;
        public float CurrentValue
        {
            get => _currentValue;
            set
            {
                if (IsConst) return;
                if (IsDecimal) value = Mathf.Round(value);
                
                _currentValue = value;
                OnValueChanged?.Invoke(StatType, value);
            }
        }

        public StatData(StatType statType, float baseValue, bool visible = true, bool isDecimal = true, bool isConst = false)
        {
            StatType = statType;
            Visible = visible;
            IsDecimal = isDecimal;
            IsConst = isConst;
            _baseValue = baseValue;
        }
    }
}
using System;
using _Project.Scripts.Architecture.Enums;
using UnityEngine;

namespace _Project.Scripts.Architecture.Entities.Base
{
    [Serializable]
    public class RadiusBasedEffectData
    {
        [field: SerializeField, Range(0, 4)] public int Radius { get; protected set; }
        [field: SerializeField] public StatType StatType { get; protected set; }
        [field: SerializeField] public CalculationMethod CalculationMethod { get; protected set; }
        [field: SerializeField] public float Value { get; protected set; }
    }
    

    [Serializable]
    public class StatScalingEffectData
    {
        [field: SerializeField] public StatType ScalableStatType { get; protected set; }
        [field: SerializeField] public StatType MultiplierStatType  { get; protected set; }
        [field: SerializeField] public StatValueSource MultiplierValueSource  { get; protected set; }
        
        public StatValueSource ScalableBaseValueSource  =>  StatValueSource.Base;
    }
    
}
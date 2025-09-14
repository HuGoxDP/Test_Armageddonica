using _Project.Scripts.Architecture.Enums;
using UnityEngine;

namespace _Project.Scripts.Architecture.Entities.Base
{
    [System.Serializable]
    public class StatMultiplier
    {
        public StatType statType;
        [Range(0, 1)]public float multiplier;
    }
}
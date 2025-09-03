using UnityEngine;

namespace _Project.Scripts.Architecture.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Game/CardSystem/New Building Card Data")]
    public class BuildingCardData : BaseCardData
    {
        public override CardType CardType => CardType.Building;
    }
}
using _Project.Scripts.Architecture.Enums;
using UnityEngine;

namespace _Project.Scripts.Architecture.Cards.Data
{
    [CreateAssetMenu(menuName = "Game/CardSystem/New Building Card Data")]
    public class BuildingCardData : BaseCardData
    {
        public override CardType CardType => CardType.Building;
    }
}
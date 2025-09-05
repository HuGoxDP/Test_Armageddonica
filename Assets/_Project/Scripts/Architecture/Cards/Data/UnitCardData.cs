using UnityEngine;

namespace _Project.Scripts.Architecture.Cards.Data
{
    [CreateAssetMenu(menuName = "Game/CardSystem/New Unit Card Data")]
    public class UnitCardData : BaseCardData
    {
        public override CardType CardType => CardType.Unit;
    }
}
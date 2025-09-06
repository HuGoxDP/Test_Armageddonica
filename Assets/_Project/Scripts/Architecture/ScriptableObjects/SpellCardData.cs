using UnityEngine;

namespace _Project.Scripts.Architecture.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Game/CardSystem/New Spell Card Data")]
    public class SpellCardData : BaseCardData
    {
        public override CardType CardType => CardType.Spell;
    }
}
using UnityEngine;

namespace _Project.Scripts.Architecture.Cards.Data
{
    public abstract class BaseCardData : ScriptableObject
    {
        [field: SerializeField] public string CardName { get; protected set; }
        [field: SerializeField, TextArea(2, 6)] public string Description { get; protected set; }
        [field: SerializeField] public Sprite CardTypeIcon { get; protected set; }
        [field: SerializeField] public Sprite CardImage { get; protected set; }
        public abstract CardType CardType { get; }
    }
}
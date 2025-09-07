using _Project.Scripts.Architecture.Entities.Base;
using UnityEngine;

namespace _Project.Scripts.Architecture.Cards.Data
{
    public abstract class BaseCardData : ScriptableObject
    {
        [Header("Card Visuals Settings")]
        [field: SerializeField] public string CardName { get; protected set; }
        [field: SerializeField, TextArea(2, 6)] public string Description { get; protected set; }
        [field: SerializeField] public Sprite CardTypeIcon { get; protected set; }
        [field: SerializeField] public Sprite CardImage { get; protected set; }
       
        [Header("Entity Settings")]
        [field: SerializeField]  public Entity EntityPrefab { get; protected set;  }
        
        public abstract CardType CardType { get; }
    }
}
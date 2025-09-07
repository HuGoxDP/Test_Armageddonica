using _Project.Scripts.Architecture.Cards.Data;
using UnityEngine;

namespace _Project.Scripts.Architecture.Entities.Base
{
    public class Entity : MonoBehaviour, IPlaceable
    {
        private BaseCardData _cardData;
        
        public virtual void Initialize(BaseCardData cardData)
        {
            _cardData = cardData;
            gameObject.name = cardData.CardName;
        }
    }
}
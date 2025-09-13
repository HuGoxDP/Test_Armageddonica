using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Core.Interfaces;
using _Project.Scripts.Architecture.Enums;
using _Project.Scripts.Architecture.Grid.Components;
using UnityEngine;

namespace _Project.Scripts.Architecture.Grid.Validators
{
    public abstract class BasePlacementValidator : MonoBehaviour
    {
        [SerializeField] protected ValidatedPlacementSystem placementSystem;
        private void Start()
        {
            if (placementSystem != null)
            {
                placementSystem.RegisterValidator(this);
            }
            else
            {
                Debug.LogWarning("No ValidatedPlacementSystem found in the scene.");
            }
        }

        public bool Validate(BaseCardData cardData, IGridCell cell)
        {
            if (cardData == null)
            {
                Debug.LogError("Card data is null");
                return false;
            }
            
            if (cell == null)
            {
                Debug.LogError("Cell is null");
            }

            var isValid = false;
            
            if (cardData.CardType == CardType.Spell)
            {
                var spellCardData = cardData as SpellCardData;
                if (spellCardData == null)
                {
                    Debug.LogError("Spell card data is null");
                    return false;
                }
                
                isValid = ValidateSpellCard(spellCardData, cell);
            }
            else if (cardData.CardType == CardType.Unit || cardData.CardType == CardType.Building)
            {
                var entityCardData = cardData as EntityCardData;
                if (entityCardData == null)
                {
                    Debug.LogError("Entity card data is null");
                    return false;
                }
                
                isValid = ValidateEntityCard(entityCardData, cell);
            }
            return isValid;
        }
        
        protected bool ValidateTargetType(SpellCardData cardData, IGridCell cell)
        {
            if (cell.OccupiedEntity == null)
                return false;
            
            var entityCardData = cell.OccupiedEntity.CardData;
            
            if (cardData.TargetType == TargetType.Unit && entityCardData.CardType == CardType.Unit)
            {
                return true;
            }
            
            if (cardData.TargetType == TargetType.Building && entityCardData.CardType == CardType.Building)
            {
                return true;
            }

            if (cardData.TargetType == TargetType.Both && entityCardData.CardType != CardType.Spell)
            {
                return true;
            }
            
            return false;
        }
        
        protected abstract bool ValidateSpellCard(SpellCardData cardData, IGridCell cell);
        protected abstract bool ValidateEntityCard(EntityCardData cardData, IGridCell cell);

    }
}
using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Grid.Components;
using _Project.Scripts.Architecture.Grid.Core;
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
        public abstract bool Validate(BaseCardData cardData, IGridCell cell);
    }
}
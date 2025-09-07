using System.Collections.Generic;
using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Core.Interfaces;
using _Project.Scripts.Architecture.Entities;
using _Project.Scripts.Architecture.Grid.Core;
using _Project.Scripts.Architecture.Grid.Validators;
using UnityEngine;

namespace _Project.Scripts.Architecture.Grid.Components
{
    public class ValidatedPlacementSystem : MonoBehaviour, IPlacementSystem
    {
        public bool IsEnabled { get; set; }

        private HashSet<BasePlacementValidator> _validators;
        private IGridContext _context;

        public void Initialize(IGridContext gridContext)
        {
            _context = gridContext;
            IsEnabled = true;
        }

        public void RegisterValidator(BasePlacementValidator validator)
        {
            if (_validators == null)
                _validators = new HashSet<BasePlacementValidator>();
            
            Debug.Log($"Validator {validator.GetType().Name} registered.");
            
            _validators.Add(validator);
        }

        public bool CanPlaceAt(BaseCardData cardData, Vector3 position)
        {
            if (!IsEnabled) return false;
            if (!TryGetCell(position, out var cell)) return false;

            return CanPlaceAt(cardData, cell);
        }

        public bool CanPlaceAt(BaseCardData cardData, IGridCell cell)
        {
            if (!IsEnabled) return false;

            Debug.Log($"Checking placement for card {cardData.CardName} at cell {cell.GameObject.name}");
            Debug.Log($"Number of validators: {_validators.Count}");
            
            foreach (var validator in _validators)
            {
                Debug.Log($"Running validator {validator.GetType().Name} for cell {cell.GameObject.name}");
                if (!validator.Validate(cardData, cell))
                {
                    return false;
                }
            }

            return true;
        }

        public bool TryPlaceCard(BaseCardData cardData, Vector3 position)
        {
            if (!IsEnabled) return false;
            if (!TryGetCell(position, out var cell)) return false;
            if (!CanPlaceAt(cardData, cell)) return false;

            if (cardData.CardType == CardType.Spell)
            {
                return TryUseSpell(cardData, cell);
            }

            return TryPlaceEntity(cardData, cell);
        }

        public void RemoveEntity(Vector3 position)
        {
            if (!IsEnabled) return;
            if (!TryGetCell(position, out var cell)) return;
            if (!cell.IsOccupied) return;
            cell.ClearEntity();
        }

        private bool TryGetCell(Vector3 position, out IGridCell cell)
        {
            cell = null;
            if (!_context.TryGetGridComponent<IGridGenerator>(out var gridGenerator))
            {
                Debug.LogWarning("GridGenerator component not found in context.");
                return false;
            }

            cell = gridGenerator.GetCellAt(position);
            if (cell == null)
            {
                Debug.LogWarning("No cell found at the given position.");
                return false;
            }

            return true;
        }

        private bool TryPlaceEntity(BaseCardData cardData, IGridCell cell)
        {
            var entityFactory = EntityFactory.Instance;
            var entity = entityFactory.CreateEntity(cardData, cell.GameObject.transform);

            if (entity == null) return false;

            Debug.Log($"Entity {entity.name} placed at cell {cell.GameObject.name}");
            cell.SetEntity(entity);
            return true;
        }

        private bool TryUseSpell(BaseCardData cardData, IGridCell cell)
        {
            // Implement spell effect logic here
            // For now, just log the spell usage
            Debug.Log($"Spell {cardData.CardName} used at cell {cell.GameObject.name}");
            return true;
        }
    }
}
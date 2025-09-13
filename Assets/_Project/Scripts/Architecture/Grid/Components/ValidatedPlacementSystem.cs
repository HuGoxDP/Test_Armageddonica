using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Core.Dependency_Injection;
using _Project.Scripts.Architecture.Core.Interfaces;
using _Project.Scripts.Architecture.Entities;
using _Project.Scripts.Architecture.Enums;
using _Project.Scripts.Architecture.Grid.Core;
using _Project.Scripts.Architecture.Grid.Validators;
using _Project.Scripts.Architecture.Spell;
using UnityEngine;

namespace _Project.Scripts.Architecture.Grid.Components
{
    public class ValidatedPlacementSystem : MonoBehaviour, IPlacementSystem
    {
        public event EventHandler<EntityPlacementEventArgs> OnEntityPlaced;
        public event EventHandler<EntityPlacementEventArgs> OnEntityRemoved;
        public bool IsEnabled { get; set; }

        private HashSet<BasePlacementValidator> _validators;
        private IGridContext _context;
        
        private IEntityFactory _entityFactory;
        private ISpellFactory _spellFactory;
        
        private void Start()
        {
            _entityFactory = ServiceLocator.Get<IEntityFactory>();
            _spellFactory = ServiceLocator.Get<ISpellFactory>();
        }
        
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
            
            foreach (var validator in _validators)
            {
                if (!validator.Validate(cardData, cell))
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> TryPlaceCard(BaseCardData cardData, Vector3 position)
        {
            if (!IsEnabled) return false;
            if (!TryGetCell(position, out var cell)) return false;
            if (!CanPlaceAt(cardData, cell)) return false;

            if (cardData.CardType == CardType.Spell)
            {
                var spellCardData = cardData as SpellCardData;
                
                if (spellCardData == null)
                    return false;
                
                return await TryUseSpell(spellCardData, cell);
            }
            
            var entityCardData = cardData as EntityCardData;
            
            if (entityCardData == null)
                return false;
            
            return TryPlaceEntity(entityCardData, cell);
        }

        public void RemoveEntity(Vector3 position)
        {
            if (!IsEnabled) return;
            if (!TryGetCell(position, out var cell)) return;
            if (!cell.IsOccupied) return;

            OnEntityRemoved?.Invoke(this, new EntityPlacementEventArgs(cell.OccupiedEntity, false));
            cell.ClearEntity();

        }

        private bool TryGetCell(Vector3 position, out IGridCell cell)
        {
            if (!_context.TryGetCell(position, out cell))
            {
                Debug.LogWarning("No cell found at the given position.");
                return false;
            }

            return true;
        }

        private bool TryPlaceEntity(EntityCardData cardData, IGridCell cell)
        {
            var entity = _entityFactory.CreateEntity(cardData, cell.GameObject.transform);

            if (entity == null) return false;

            cell.SetEntity(entity);
            OnEntityPlaced?.Invoke(this, new EntityPlacementEventArgs(cell.OccupiedEntity, true));
            return true;
        }

        private async Task<bool> TryUseSpell(SpellCardData cardData, IGridCell cell)
        { 
            Spell.Base.Spell spellObject = null;
            try
            {
                spellObject = _spellFactory.CreateSpell(cardData, cell.GameObject.transform);
                if (spellObject == null) return false;
                
                var targetEntity = cell.OccupiedEntity;
                if (targetEntity == null)
                {
                    Debug.LogWarning("No entity in cell to apply spell effects");
                    return false;
                }
                
                var success = await spellObject.TryUseEffects(targetEntity);
                return success;
            }
            finally
            {
                if (spellObject != null)
                    Destroy(spellObject);
            }
        }
    }
}
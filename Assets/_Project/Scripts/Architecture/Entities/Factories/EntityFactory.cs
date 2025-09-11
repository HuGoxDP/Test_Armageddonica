using System;
using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Core.Dependency_Injection;
using _Project.Scripts.Architecture.Entities.Base;
using UnityEngine;

namespace _Project.Scripts.Architecture.Entities.Factories
{
    public class EntityFactory : MonoBehaviour,IEntityFactory
    {
        [SerializeField] private Transform _entityContainer;
        private void Awake()
        {
            ServiceLocator.Register<IEntityFactory>(this);
        }

        public Entity CreateEntity(EntityCardData cardData, Transform cellTransform)
        {
            var prefab = cardData.EntityPrefab;
            if (prefab == null)
                throw new ArgumentNullException(nameof(cardData.EntityPrefab), "Entity prefab cannot be null");
            
            var entity = Instantiate(prefab, cellTransform.position, Quaternion.identity, _entityContainer);
            entity.Initialize(cardData);
            return entity;
        }
    }
}
using System;
using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Entities.Base;
using UnityEngine;

namespace _Project.Scripts.Architecture.Entities
{
    public class EntityFactory : MonoBehaviour
    {
        private static EntityFactory _instance;
        public static EntityFactory Instance => _instance ??= FindFirstObjectByType<EntityFactory>();
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public Entity CreateEntity(BaseCardData cardData, Transform cellTransform)
        {
            Entity prefab = cardData.EntityPrefab;
            if (prefab == null)
                throw new ArgumentNullException(nameof(cardData.EntityPrefab), "Entity prefab cannot be null");
            
            Entity entity = Instantiate(prefab, cellTransform.transform.position, Quaternion.identity, cellTransform);
            entity.Initialize(cardData);
            return entity;
        }
    }
}
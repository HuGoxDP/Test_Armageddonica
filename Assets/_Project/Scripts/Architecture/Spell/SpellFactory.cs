using System;
using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Core.Dependency_Injection;
using _Project.Scripts.Architecture.Core.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Architecture.Spell
{
    public class SpellFactory : MonoBehaviour, ISpellFactory
    {
        private void Awake()
        {
            ServiceLocator.Register<ISpellFactory>(this);      
        }

        public Base.Spell CreateSpell(SpellCardData cardData, Transform cellTransform)
        {
            var prefab = cardData.SpellPrefab;
            if (prefab == null)
            {
                throw new ArgumentNullException(nameof(cardData.SpellPrefab), "Entity prefab cannot be null");
            }

            var spell = Instantiate(prefab, cellTransform.transform.position, Quaternion.identity, cellTransform);
            return spell;
        }
    }
}
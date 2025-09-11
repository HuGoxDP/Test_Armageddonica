using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Entities.Base;
using UnityEngine;

namespace _Project.Scripts.Architecture.Entities
{
    public interface IEntityFactory
    {
        Entity CreateEntity(EntityCardData cardData, Transform cellTransform);
    }
}
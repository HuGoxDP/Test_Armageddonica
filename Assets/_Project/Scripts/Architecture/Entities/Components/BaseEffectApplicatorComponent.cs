using System.Threading.Tasks;
using _Project.Scripts.Architecture.Core.Interfaces;
using _Project.Scripts.Architecture.Entities.Base;
using UnityEngine;

namespace _Project.Scripts.Architecture.Entities.Components
{
    public abstract class BaseEffectApplicatorComponent : MonoBehaviour, IEffectApplicator
    {
        public bool IsEnabled { get; set; }

        protected Entity entity;

        public virtual void Initialize(Entity entity)
        {
            this.entity = entity;
            IsEnabled = true;
        }

        public abstract Task ApplyEffect(EntityEffectManager effectManager);
    }
}
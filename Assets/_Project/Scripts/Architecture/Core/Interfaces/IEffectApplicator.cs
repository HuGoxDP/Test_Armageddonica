using System.Threading.Tasks;
using _Project.Scripts.Architecture.EffectApplicators;
using _Project.Scripts.Architecture.Entities.Base;
using _Project.Scripts.Architecture.Enums;

namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface IEffectApplicator
    {
        Task ApplyEffect(Entity effectApplicator, IEntityEffectManager effectManager);
    }
}
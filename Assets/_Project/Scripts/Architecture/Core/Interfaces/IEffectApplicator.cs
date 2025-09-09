using System.Threading.Tasks;
using _Project.Scripts.Architecture.Entities.Base;

namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface IEffectApplicator : IComponent
    {
        Task ApplyEffect(EntityEffectManager effectManager);
    }
}
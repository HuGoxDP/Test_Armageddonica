using _Project.Scripts.Architecture.Entities.Base;

namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface IComponent
    {
        bool IsEnabled { get; set; }
        void Initialize(Entity entity);
    }
}
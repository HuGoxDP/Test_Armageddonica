using _Project.Scripts.Architecture.Enums;

namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface IStatRequirements
    {
        StatType[] GetRequiredStats();
    }
}
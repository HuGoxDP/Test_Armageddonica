using _Project.Scripts.Architecture.Enums;

namespace _Project.Scripts.Architecture.Cards.Data
{
    public interface IStatRequirements
    {
        StatType[] GetRequiredStats();
    }
}
namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface ISelectCardMenu
    {
        void GenerateCards(ICardGeneratorStrategy cardGeneratorStrategy, int count);
    }
}
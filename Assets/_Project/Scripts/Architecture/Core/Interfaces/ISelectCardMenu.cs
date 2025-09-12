namespace _Project.Scripts.Architecture.SelectCardMenu
{
    public interface ISelectCardMenu
    {
        void GenerateCards(ICardGeneratorStrategy cardGeneratorStrategy, int count);
    }
}
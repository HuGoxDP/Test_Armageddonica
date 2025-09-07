namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface IGridComponent
    {
        void Initialize(IGridContext gridContext);
        bool IsEnabled { get; set; }
    }
}
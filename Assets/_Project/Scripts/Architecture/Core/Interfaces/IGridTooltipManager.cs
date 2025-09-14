namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface IGridTooltipManager: IGridComponent
    {
        void EnableTooltips(bool isEnable);
        void RegisterGridInputSystem(IGridInputSystem gridInputSystem);
    }
}
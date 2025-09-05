using System.Collections.Generic;
using _Project.Scripts.Architecture.UI;

namespace _Project.Scripts.Architecture.Services
{
    public interface ILayoutStrategyData
    {
        public IReadOnlyList<CardUI> Cards { get; }

    }
    
    public struct LayoutStrategyData : ILayoutStrategyData
    {
        public IReadOnlyList<CardUI> Cards { get; }
        
        public LayoutStrategyData(IReadOnlyList<CardUI> cards)
        {
            Cards = cards;
        }
    }
    
    public struct HoveredLayoutStrategyData : ILayoutStrategyData
    {
        public IReadOnlyList<CardUI> Cards { get; }
        public int HoveredCardId { get; }
        
        public HoveredLayoutStrategyData(List<CardUI> cards, CardUI hoveredCard)
        {
            Cards = cards;
            HoveredCardId = cards.IndexOf(hoveredCard);
        }
    }
}
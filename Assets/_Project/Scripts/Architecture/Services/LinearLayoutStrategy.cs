using System;
using System.Collections.Generic;
using _Project.Scripts.Architecture.UI;
using UnityEngine;

namespace _Project.Scripts.Architecture.Services
{
    public interface ILayoutStrategy
    {
        /// <summary> Calculates the layout positions for cards based on the provided data and settings. </summary>
        Dictionary<CardUI, Vector3> CalculateLayout(ILayoutStrategyData data, LayoutSettings parameters);
    }

    public abstract class BaseLinearLayoutStrategy : ILayoutStrategy
    {
        public abstract Dictionary<CardUI, Vector3> CalculateLayout(ILayoutStrategyData data, LayoutSettings parameters);
        
        protected float CalculateAdjustedSpacing(LayoutSettings parameters, int cardCount)
        {
            var totalWidth = cardCount * parameters.CardWidth + (cardCount - 1) * parameters.Spacing;
            var adjustedSpacing = parameters.Spacing;
            if (totalWidth > parameters.MaxWidth)
            {
                var overlapWidth = totalWidth - parameters.MaxWidth;
                var spacingReductionPerCard = overlapWidth / (cardCount - 1);
                adjustedSpacing = Math.Max(parameters.Spacing - spacingReductionPerCard, parameters.MinSpacing);
            }

            return adjustedSpacing;
        }
    }

    public class LinearLayoutStrategy : BaseLinearLayoutStrategy
    {
        private Dictionary<CardUI, Vector3> _positions = new();

        /// <summary> Calculates the layout positions for cards in a linear fashion. </summary>
        public override Dictionary<CardUI, Vector3> CalculateLayout(ILayoutStrategyData data, LayoutSettings parameters)
        {
            if (data == null || data.Cards == null || data.Cards.Count == 0)
                return new Dictionary<CardUI, Vector3>();

            _positions.Clear();
            var cardCount = data.Cards.Count;
            
            var adjustedSpacing = CalculateAdjustedSpacing(parameters, cardCount);
            
            var totalWidth = cardCount * parameters.CardWidth + (cardCount - 1) * adjustedSpacing;
            var startX = -totalWidth / 2 + parameters.CardWidth / 2;

            for (var i = 0; i < cardCount; i++)
            {
                var targetX = startX + i * (parameters.CardWidth + adjustedSpacing);
                var targetPosition = new Vector3(targetX, 0, 0);
                _positions[data.Cards[i]] = targetPosition;
            }

            return _positions;
        }
    }

    public class HoverLinearLayoutStrategy : BaseLinearLayoutStrategy
    {
        private Dictionary<CardUI, Vector3> _positions = new();
        private readonly ILayoutStrategy _baseStrategy = new LinearLayoutStrategy();

        /// <summary> Calculates the layout positions for cards with hover effect. </summary>
        public override Dictionary<CardUI, Vector3> CalculateLayout(ILayoutStrategyData data, LayoutSettings parameters)
        {
            if (data == null || data.Cards == null || data.Cards.Count == 0)
                return new Dictionary<CardUI, Vector3>();
            if (data is not HoveredLayoutStrategyData hoveredData)
                throw new ArgumentException("Data must be of type HoveredLayoutStrategyData", nameof(data));

            _positions.Clear();
            _positions = _baseStrategy.CalculateLayout(data, parameters);

            var hoveredCardIndex = hoveredData.HoveredCardId;
            var cardCount = data.Cards.Count;

            if (hoveredCardIndex >= 0 && hoveredCardIndex < cardCount)
            {
                var hoveredCard = data.Cards[hoveredCardIndex];
                var hoveredPosition = _positions[hoveredCard];

                if (_positions.ContainsKey(hoveredCard))
                {
                    var hoveredPos = _positions[hoveredCard];
                    _positions[hoveredCard] = new Vector3(hoveredPos.x, parameters.LiftHeight, hoveredPos.z);
                }

            
       
                for (int i = 0; i < cardCount; i++)
                {
                    if (i == hoveredCardIndex) continue;

                    var card = data.Cards[i];
                    if (!_positions.ContainsKey(card)) continue;

                    var cardPosition = _positions[card];

                    int gapsBetween = Math.Abs(i - hoveredCardIndex);
                    int maxGaps = Math.Max(hoveredCardIndex, cardCount - 1 - hoveredCardIndex);

                    float effectStrength = maxGaps > 0 ? 1f - (float)(gapsBetween - 1) / maxGaps : 0f;
                    effectStrength = Mathf.Clamp01(effectStrength);

                    float direction = cardPosition.x < hoveredPosition.x ? -1 : 1;
                    float horizontalOffset = direction * parameters.HoverOffset * effectStrength;

                    _positions[card] = new Vector3(cardPosition.x + horizontalOffset, cardPosition.y, cardPosition.z);
                }
            }

            return _positions;
        }
    }
}
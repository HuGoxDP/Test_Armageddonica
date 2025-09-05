using System.Collections.Generic;
using _Project.Scripts.Architecture.Cards.Runtime;
using _Project.Scripts.Architecture.Core.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Architecture.Layout
{
    /// <summary> Implements a linear layout strategy for arranging objects in a horizontal line. </summary>
    public class LinearLayoutStrategy : ILayoutStrategy
    {
        private Dictionary<CardUI, Vector3> _positions;

        /// <summary> Calculates the target positions for a collection of transforms based on the linear layout strategy. </summary>
        public Dictionary<Transform, Vector3> CalculateLayout(ILayoutData data, LayoutSettings settings)
        {
            if (data == null || data.Transforms == null || data.Transforms.Count == 0)
                return new Dictionary<Transform, Vector3>();

            return CalculateBasePositions(data.Transforms, settings);
        }

        /// <summary> Calculates the target positions for a collection of transforms when one is hovered over. </summary>
        public Dictionary<Transform, Vector3> CalculateHoverLayout(ILayoutData data, LayoutSettings settings,
            int hoveredIndex)
        {
            if (data == null || data.Transforms == null || data.Transforms.Count == 0)
                return new Dictionary<Transform, Vector3>();

            var basePositions = CalculateBasePositions(data.Transforms, settings);
            var transformsCount = data.Transforms.Count;
            var hoveredTransform = data.Transforms[hoveredIndex];

            if (hoveredTransform == null)
                return basePositions;

            var hoveredPosition = basePositions[hoveredTransform] + new Vector3(0, settings.HoverHeightOffset, 0);
            basePositions[hoveredTransform] = hoveredPosition;
            
            var hoveredX = hoveredPosition.x;
            var hoveredLeftEdge = hoveredX - settings.ObjectWidth / 2;
            var hoveredRightEdge = hoveredX + settings.ObjectWidth / 2;

            var maxInfluencedCards = transformsCount / 2;

            if (hoveredIndex > 0)
            {
                var firstLeftTransform = data.Transforms[hoveredIndex - 1];
                var firstLeftCurrentPos = basePositions[firstLeftTransform];
                
                var targetRightEdge = hoveredLeftEdge - settings.HoverOffset;
                var targetFirstLeftX = targetRightEdge - settings.ObjectWidth / 2;
                
                var leftShift = targetFirstLeftX - firstLeftCurrentPos.x;
                
                if (leftShift < 0)
                {
                    var leftBound = Mathf.Max(0, hoveredIndex - maxInfluencedCards);
                    for (var i = hoveredIndex - 1; i >= leftBound; i--)
                    {
                        var transform = data.Transforms[i];
                        var currentPos = basePositions[transform];
                        var newPos = new Vector3(currentPos.x + leftShift, currentPos.y, currentPos.z);
                        basePositions[transform] = newPos;
                    }
                }
            }

            if (hoveredIndex < transformsCount - 1)
            {
                var firstRightTransform = data.Transforms[hoveredIndex + 1];
                var firstRightCurrentPos = basePositions[firstRightTransform];
                
                var targetLeftEdge = hoveredRightEdge + settings.HoverOffset;
                var targetFirstRightX = targetLeftEdge + settings.ObjectWidth / 2;
                
                var rightShift = targetFirstRightX - firstRightCurrentPos.x;
                
                if (rightShift > 0)
                {
                    var rightBound = Mathf.Min(transformsCount - 1, hoveredIndex + maxInfluencedCards);
                    for (var i = hoveredIndex + 1; i <= rightBound; i++)
                    {
                        var transform = data.Transforms[i];
                        var currentPos = basePositions[transform];
                        var newPos = new Vector3(currentPos.x + rightShift, currentPos.y, currentPos.z);
                        basePositions[transform] = newPos;
                    }
                }
            }
            
            return basePositions;
        }

        /// <summary> Calculates the base positions for a collection of transforms in a linear layout. </summary>
        private Dictionary<Transform, Vector3> CalculateBasePositions(IReadOnlyList<Transform> transforms,
            LayoutSettings settings)
        {
            var positions = new Dictionary<Transform, Vector3>();

            var transformsCount = transforms.Count;
            var adjustedSpacing = CalculateAdjustedSpacing(settings, transformsCount);
            var totalWidth = transformsCount * settings.ObjectWidth + (transformsCount - 1) * adjustedSpacing;
            var startX = -totalWidth / 2 + settings.ObjectWidth / 2;

            for (var i = 0; i < transformsCount; i++)
            {
                var targetX = startX + i * (settings.ObjectWidth + adjustedSpacing);
                var targetPosition = new Vector3(targetX, 0, 0);
                positions[transforms[i]] = targetPosition;
            }

            return positions;
        }

        /// <summary> Calculates the adjusted spacing between objects to ensure they fit within the maximum width. </summary>
        private float CalculateAdjustedSpacing(LayoutSettings settings, int transformsCount)
        {
            var totalWidth = transformsCount * settings.ObjectWidth + (transformsCount - 1) * settings.Spacing;
            var adjustedSpacing = settings.Spacing;
            if (totalWidth > settings.MaxWidth)
            {
                var overlapWidth = totalWidth - settings.MaxWidth;
                var spacingReductionPerCard = overlapWidth / (transformsCount - 1);
                adjustedSpacing = Mathf.Max(settings.Spacing - spacingReductionPerCard, settings.MinSpacing);
            }

            return adjustedSpacing;
        }
    }
}
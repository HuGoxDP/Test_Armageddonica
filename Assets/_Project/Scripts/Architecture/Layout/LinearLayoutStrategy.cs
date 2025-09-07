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
            
            if (transformsCount == 1)
                return basePositions;
            
            float shift = 0;
            var shiftDirection = CalculateShiftDirection(hoveredIndex);
            var hoveredEdge = hoveredPosition.x + (settings.ObjectWidth / 2) * shiftDirection;
            var firstSideTransform = data.Transforms[hoveredIndex + 1 * shiftDirection];
            var firsSidePos = basePositions[firstSideTransform];
            var targetEdge = hoveredEdge + settings.HoverOffset * shiftDirection;
            var targetSideX = targetEdge + settings.ObjectWidth / 2 * shiftDirection;
            shift =Mathf.Abs(targetSideX - firsSidePos.x);
            
            if (shift > 0)
            {
                for (var i = 0; i < transformsCount; i++)
                {
                    if (i == hoveredIndex) continue;
                    
                    
                    var direction = i < hoveredIndex ? -1 : 1;
                    
                    var transform = data.Transforms[i];
                    var currentPos = basePositions[transform];
                    var newPos = new Vector3(currentPos.x + shift * direction, currentPos.y, currentPos.z);
                    basePositions[transform] = newPos;
                }
            }
            return basePositions;
        }

        private static int CalculateShiftDirection(int hoveredIndex)
        {
            int shiftDirection;
            if (hoveredIndex > 0)
            {
                shiftDirection = hoveredIndex - 1 < hoveredIndex ? -1 : 1;
            }
            else
            {
                shiftDirection = hoveredIndex + 1 < hoveredIndex ? -1 : 1; 
            }

            return shiftDirection;
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
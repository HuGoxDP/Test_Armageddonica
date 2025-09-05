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
        public Dictionary<Transform, Vector3> CalculateHoverLayout(ILayoutData data, LayoutSettings settings, int hoveredIndex)
        {
            if (data == null || data.Transforms == null || data.Transforms.Count == 0)
                return new Dictionary<Transform, Vector3>();

            var basePositions = CalculateBasePositions(data.Transforms, settings);
            var transformsCount = basePositions.Count;
            var hoveredTransform = data.Transforms[hoveredIndex];
            
            if (hoveredTransform != null)
            {
                var hoveredPosition = basePositions[hoveredTransform] + new Vector3(0, settings.HoverHeightOffset, 0);
                basePositions[hoveredTransform] = hoveredPosition;
            }
            else
            {
                return basePositions;
            }
            
            for (var i = 0; i < transformsCount; i++)
            {
                var transform = data.Transforms[i];
                if (transform == hoveredTransform)
                    continue;
                
                var adjustedSpacing = CalculateAdjustedSpacing(settings, transformsCount);
                var effectStrength = CalculateEffectStrength(i, hoveredIndex, transformsCount);
                var horizontalOffset = effectStrength * settings.HoverOffset;
                var targetPosition = basePositions[transform] + new Vector3(horizontalOffset, 0, 0);
                basePositions[transform] = targetPosition;
            }

            
            return basePositions;
        }
        
        /// <summary> Calculates the base positions for a collection of transforms in a linear layout. </summary>
        private Dictionary<Transform, Vector3> CalculateBasePositions(IReadOnlyList<Transform> transforms, LayoutSettings settings)
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
        
        /// <summary> Calculates the effect strength for a transform based on its distance from the hovered transform. </summary>
        private float CalculateEffectStrength(int transformIndex, int hoveredIndex, int totalCards)
        {
            var direction = Mathf.Sign(transformIndex - hoveredIndex);
            
            float distance = Mathf.Abs(transformIndex - hoveredIndex);
            float maxDistance = Mathf.Max(1, totalCards / 4);
            var normalizedDistance = Mathf.Clamp01(distance / maxDistance);
            var strength = (1 - normalizedDistance) * direction;
            return strength;
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
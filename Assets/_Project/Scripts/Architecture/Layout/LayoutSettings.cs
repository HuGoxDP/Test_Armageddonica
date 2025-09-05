using System;
using UnityEngine;

namespace _Project.Scripts.Architecture.Layout
{
   /// <summary> Serializable settings for layout configuration. </summary>
    [Serializable]
    public class LayoutSettings
    {
        public float ObjectWidth => _objectWidth;
        public float Spacing => _spacing;
        public float MaxWidth => _maxWidth;
        public float MinSpacing => _minSpacing;
        public float HoverOffset => _hoverOffset;
        public float HoverHeightOffset => _hoverHeightOffset;
        
        [SerializeField] private float _objectWidth;
        [SerializeField] private float _spacing;
        [SerializeField] private float _maxWidth;
        [SerializeField] private float _minSpacing;
        [SerializeField] private float _hoverOffset;
        [SerializeField] private float _hoverHeightOffset;
    }
}
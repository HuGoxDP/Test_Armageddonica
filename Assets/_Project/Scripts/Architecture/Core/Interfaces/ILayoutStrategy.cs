using System.Collections.Generic;
using _Project.Scripts.Architecture.Layout;
using UnityEngine;

namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface ILayoutStrategy
    {
        Dictionary<Transform, Vector3> CalculateLayout(ILayoutData data, LayoutSettings settings);
        Dictionary<Transform, Vector3> CalculateHoverLayout(ILayoutData data, LayoutSettings settings, int hoveredIndex);
    }
}
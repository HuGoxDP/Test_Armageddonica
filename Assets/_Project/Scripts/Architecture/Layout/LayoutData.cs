using System.Collections.Generic;
using _Project.Scripts.Architecture.Cards.Runtime;
using _Project.Scripts.Architecture.Core.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Architecture.Layout
{
    /// <summary> Data structure representing the layout information for a collection of transforms. </summary>
    public struct LayoutData : ILayoutData
    {
        public IReadOnlyList<Transform> Transforms { get; }
        public LayoutData(IReadOnlyList<Transform> transforms)
        {
            Transforms = transforms;
        }
    }
}
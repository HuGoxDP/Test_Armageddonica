using System.Collections.Generic;
using _Project.Scripts.Architecture.Cards.Runtime;
using UnityEngine;

namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface ILayoutData
    {
        IReadOnlyList<Transform> Transforms { get; }
    }
}
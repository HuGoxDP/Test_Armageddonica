using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface ILayoutData
    {
        IReadOnlyList<Transform> Transforms { get; }
    }
}
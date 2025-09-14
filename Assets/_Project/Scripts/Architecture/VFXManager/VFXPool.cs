using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Architecture.VFXManager
{
    [System.Serializable]
    public class VFXPool
    {
        public string effectName;
        public GameObject prefab;
        public int poolSize = 10;

        [HideInInspector] public Queue<GameObject> availableObjects = new Queue<GameObject>();
        [HideInInspector] public List<GameObject> activeInfiniteEffects = new List<GameObject>();
    }
}
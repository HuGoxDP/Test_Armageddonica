using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Architecture.Core.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Architecture.Layout
{
    /// <summary> Manages the layout of a collection of objects transforms using a specified layout strategy. </summary>
    /// <remarks> Attach this script to an empty GameObject that will serve as the parent for the objects to be laid out. </remarks>
    public class LayoutController : MonoBehaviour
    {
        [SerializeField] private LayoutSettings _layoutSettings;
        
        private ILayoutStrategy _layoutStrategy;
        private List<Transform> _transforms;
        private Dictionary<Transform, Vector3> _positionCache;
        
        private bool _isInitialized;
        private void Awake()
        {
            _transforms = new List<Transform>();
            _positionCache = new Dictionary<Transform, Vector3>();
        }
        
        private void OnDestroy()
        {
            _transforms.Clear();
            _positionCache.Clear();
            _isInitialized = false;
        }

        public void Initialize(ILayoutStrategy layoutStrategy)
        {
            if (_isInitialized)
            {
                Debug.LogWarning("LayoutController: Already initialized");
                return;
            }
            
            if (layoutStrategy == null)
            {
                Debug.LogError("LayoutController: Initialize called with null layoutStrategy");
                return;
            }
            
            _layoutStrategy = layoutStrategy;
            _isInitialized = true;
        }
        
        public void UpdateLayout()
        {
            if(!_isInitialized) return;
            
            _positionCache = _layoutStrategy.CalculateLayout(new LayoutData(_transforms), _layoutSettings);
            ApplyPositions(_positionCache);
            _positionCache.Clear();
        }

        public void UpdateHoverLayout(int hoveredIndex)
        {
            if(!_isInitialized) return;
            
            if (hoveredIndex < 0 || hoveredIndex >= _transforms.Count)
                return;
            
            _positionCache = _layoutStrategy.CalculateHoverLayout(new LayoutData(_transforms), _layoutSettings, hoveredIndex);
            ApplyPositions(_positionCache);
            _positionCache.Clear();
        }

        public void AddCard(Transform t)
        {
            if(!_isInitialized) return;
            
            _transforms.Add(t);
            UpdateLayout();
        }

        public void RemoveCard(Transform t)
        {
            if(!_isInitialized) return;
            
            _transforms.Remove(t);
            UpdateLayout();
        }

        public void SetCards(IReadOnlyList<Transform> transforms)
        {
            if(!_isInitialized) return;
            
            _transforms.Clear();
            _transforms.AddRange(transforms);
            UpdateLayout();
        }

        private void ApplyPositions(Dictionary<Transform, Vector3> positions)
        {
            var count = _transforms.Count;
            for (var i = 0; i < count; i++)
            {
                var cardTransform = _transforms[i];
                if (positions.TryGetValue(cardTransform, out var targetPosition))
                {
                    cardTransform.localPosition = targetPosition;
                }
            }
        }
    }
}
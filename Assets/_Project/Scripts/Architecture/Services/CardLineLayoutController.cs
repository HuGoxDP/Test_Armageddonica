using System;
using System.Collections.Generic;
using _Project.Scripts.Architecture.UI;
using UnityEngine;

namespace _Project.Scripts.Architecture.Services
{
    [Serializable]
    public class  LayoutSettings
    {
        [field: SerializeField] public float Spacing { get; private set; } = 5f;
        [field: SerializeField] public float CardWidth { get; private set; } = 160f;
        [field: SerializeField] public float MaxWidth { get; private set; } = 800f;
        [field: SerializeField] public float MinSpacing { get; private set; } = -120f;
        
        [field: SerializeField] public float HoverOffset { get; private set; } = 5f;
        [field: SerializeField] public float Exponent { get; private set; } = 1.5f;
        [field: SerializeField] public float LiftHeight  { get; private set; } = 50f;

    }
    
    public class CardLineLayoutController : MonoBehaviour
    {
        [SerializeField] private LayoutSettings _layoutSettings;

        
        private Dictionary<CardUI, Vector3> _positionCache = new();

        /// <summary> Updates the layout of the cards in the player's hand. </summary>
        public void UpdateLayout(ILayoutStrategy layoutStrategy, ILayoutStrategyData layoutStrategyData)
        {
            if (layoutStrategy == null)
            {
                Debug.LogWarning("Layout strategy is null. Cannot update layout.");
                return;
            }
            if (layoutStrategyData == null)
            {
                Debug.LogWarning("Layout strategy data is null. Cannot update layout.");
                return;
            }
            
            
            _positionCache = layoutStrategy.CalculateLayout(layoutStrategyData, _layoutSettings);

            foreach (var card in layoutStrategyData.Cards)
            {
                if (_positionCache.TryGetValue(card, out var targetPosition))
                {
                    card.transform.localPosition = targetPosition;
                }
            }
            
            _positionCache.Clear();
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Architecture.UI
{
    public class CardLayoutController : MonoBehaviour
    {
        [SerializeField] private float _spacing = 5;
        [SerializeField] private float _cardWidth = 200;
        [SerializeField] private float _maxWidth = 800;
        [SerializeField] private float _minSpacing = -160;

        private int _currentCardCount = 0;
        
        public void UpdateLayout(List<RectTransform> cards)
        {
            if (cards == null || cards.Count == 0) return;
    
            _currentCardCount = cards.Count;
            var totalWidth = _currentCardCount * _cardWidth + (_currentCardCount - 1) * _spacing;
            var adjustedSpacing = _spacing;
            
            if(totalWidth > _maxWidth)
            {
                var overlapWidth = totalWidth - _maxWidth;
                var overlapPerCard = overlapWidth / (_currentCardCount - 1);
                adjustedSpacing = Mathf.Max(_spacing - overlapPerCard, _minSpacing);
            }
            
            totalWidth = _currentCardCount * _cardWidth + (_currentCardCount - 1) * adjustedSpacing;
            var startX = -totalWidth / 2 + _cardWidth / 2;
            
            for (int i = 0; i < _currentCardCount; i++)
            {
                var targetX = startX + i * (_cardWidth + adjustedSpacing);
                var targetPosition = new Vector3(targetX, 0, 0);
                cards[i].anchoredPosition = targetPosition;
            }
    
        }
    }
}
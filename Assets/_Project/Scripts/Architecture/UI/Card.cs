using System;
using _Project.Scripts.Architecture.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Architecture.UI
{
    public interface ICard
    {
        RectTransform RectTransform { get; }
        BaseCardData CardData { get; }
        void Initialize(BaseCardData cardData);
        void UseCard(GridCell targetCell);
    }

    public class Card : MonoBehaviour, ICard
    {
        public RectTransform RectTransform => _rectTransform ?? GetComponent<RectTransform>();
        public BaseCardData CardData {get; private set; }
        
        [SerializeField] private TextMeshProUGUI _nameTextMeshPro;
        [SerializeField] private TextMeshProUGUI _descriptionTextMeshPro;
        [SerializeField] private Image _cardTypeIcon;
        [SerializeField] private Image _cardImage;
        
        private CardType _cardType;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Initialize(BaseCardData cardData)
        {
            if (cardData == null) throw new System.ArgumentNullException(nameof(cardData), "Card data cannot be null");
            
            CardData = cardData;
            
            _nameTextMeshPro.text = cardData.CardName;
            _descriptionTextMeshPro.text = cardData.Description;
            _cardTypeIcon.sprite = cardData.CardTypeIcon;
            _cardImage.sprite = cardData.CardImage;
            _cardType = cardData.CardType;
        }

        public void UseCard(GridCell targetCell)
        {
            throw new System.NotImplementedException();
        }
    }
}
